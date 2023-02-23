using UnityEngine;

public class Key : MonoBehaviour
{
    private SpriteRenderer _renderer;
    [SerializeField] private GameObject[] _doors = new GameObject[1];

    private Color _color;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _color = _renderer.color;

        foreach (GameObject door in _doors)
        {
            door.GetComponent<SpriteRenderer>().color = _color;
            door.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            foreach (GameObject door in _doors)
            {
                door.SetActive(false);
            }
            gameObject.SetActive(false);
        }
    }
}
