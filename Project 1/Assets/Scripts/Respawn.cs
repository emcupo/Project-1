using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [Tooltip("How long it takes for the scene to restart when the player needs to respawn")]
    [SerializeField] private float _respawnTimer = 1.5f;
    private WaitForSeconds _respawn;
    private Vector3 _startPosition;

    private PlayerMovement _movement;
    private Collider2D _collider;

    private AudioSource _audioSource;


    [SerializeField] private GameObject _deadBody;
    private List<GameObject> _remains = new List<GameObject>();
    private int _maxCount = -1;

    public static Action playerDied;
    public static Action playerRespawned;

    public static int deaths = 0;

    private void Awake()
    {
        // assigns unassigned variables
        _collider = GetComponent<Collider2D>();
        _movement = GetComponent<PlayerMovement>();

        _audioSource = GetComponent<AudioSource>();

        // creates the WaitForSeconds used by the restart scene coroutine
        _respawn = new WaitForSeconds(_respawnTimer);
    }

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void OnEnable()
    {
        playerDied += RespawnPlayer;
    }

    private void OnDisable()
    {
        playerDied -= RespawnPlayer;
    }
    public void RespawnPlayer()
    {
        deaths++;
        _audioSource.Play();
        enablePlayer(false);
        StartCoroutine(RestartScene());
    }

    private IEnumerator RestartScene()
    {
        yield return _respawn;
        SpawnRemain();
        transform.position = _startPosition;
        enablePlayer(true);
        _audioSource.Stop();
        playerRespawned?.Invoke();

    }

    private void SpawnRemain()
    {
        RemoveOldest();

        string key = "remainAlpha";
        float alpha = 1;
        if (PlayerPrefs.HasKey(key))
            alpha = (255 - PlayerPrefs.GetFloat(key)) / 255;

        if (alpha <= 0)
            return;

        GameObject remain = Instantiate(_deadBody, transform.position, Quaternion.identity);
        _remains.Add(remain);

        SpriteRenderer[] r = remain.GetComponentsInChildren<SpriteRenderer>();
        r[0].color = new Color(r[0].color.r, r[0].color.g, r[0].color.b, alpha);
        r[1].color = new Color(r[1].color.r, r[1].color.g, r[1].color.b, alpha);


    }

    private void RemoveOldest()
    {
        if (PlayerPrefs.HasKey("remainMax"))
            _maxCount = (int)PlayerPrefs.GetFloat("remainMax");

        if (_remains.Count == 0 || _maxCount <= 0)
            return;

        while (_remains.Count >= _maxCount)
        {
            GameObject previous = _remains[0];
            _remains.Remove(previous);
            Destroy(previous);
        }
    }

    private void enablePlayer(bool enabled)
    {
        _movement.enabled = enabled;
        _collider.enabled = enabled;
    }

    // Fades sprite color within a time frame
    /*IEnumerator Fade(Color start, Color end, float time)
    {
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            _sprite.color = Color.Lerp(start, end, elapsedTime / time);
            _shadowSprite.color = Color.Lerp(start, end, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _sprite.color = end;
    }*/
}
