using UnityEngine;

public class Key : MonoBehaviour
{
    private AudioSource _audioSource;
    private SpriteRenderer _renderer;
    [SerializeField] private GameObject[] _doors = new GameObject[1];

    [SerializeField] private Color _color;
    private bool _locked = true;

    private void OnEnable()
    {
        Respawn.playerRespawned += LockDoors;
    }

    private void OnDisable()
    {
        Respawn.playerRespawned -= LockDoors;
    }

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _color = _renderer.color;
        _audioSource = GetComponent<AudioSource>();
        foreach (GameObject door in _doors)
        {
            door.GetComponent<SpriteRenderer>().color = _color;
            door.SetActive(true);
        }
    }

    private void OnValidate()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _color = _renderer.color;

        foreach (GameObject door in _doors)
        {
            door.GetComponent<SpriteRenderer>().color = _color;
            door.SetActive(true);
        }
    }

    private void LockDoors()
    {
        _renderer.enabled = true;
        GetComponent<Collider2D>().enabled = true;
        foreach (GameObject door in _doors)
        {
            if (door != null)
                door.SetActive(true);
        }
    }

    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && _locked)
        {
            foreach (GameObject door in _doors)
            {
                if (door != null)
                    door.SetActive(false);
                _audioSource.Play();
            }
            _renderer.enabled = false;
        }
        GetComponent<Collider2D>().enabled = false;
    }
}
