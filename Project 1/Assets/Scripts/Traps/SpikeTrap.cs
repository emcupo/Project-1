using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class SpikeTrap : MonoBehaviour
{
    private enum trapState { HIDDEN, INACTIVE, STARTED, ACTIVE };

    private SpriteRenderer _renderer;
    [SerializeField] private trapState _state = trapState.HIDDEN;

    private bool _motionDetected;

    [SerializeField] private Sprite[] _sprites;

    [SerializeField] private float _triggerSpeed = 1f;
    [SerializeField] private float _triggerDuration = 2f;

    private AudioSource audioSource;

    private WaitForSeconds _speed;
    private WaitForSeconds _duration;
    private Respawn _player;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (_renderer == null)
            _renderer = GetComponent<SpriteRenderer>();

        if (_state == trapState.HIDDEN)
        {
            _renderer.color = Color.clear;
        }
        _speed = new WaitForSeconds(_triggerSpeed);
        _duration = new WaitForSeconds(_triggerDuration);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_state == trapState.HIDDEN)
        {
            _renderer.color = Color.white;
            _state = trapState.INACTIVE;
        }

        _motionDetected = true;
        if (collision.CompareTag("Player"))
        {
            _player = collision.GetComponent<Respawn>();
            if (_state == trapState.INACTIVE)
                StartCoroutine(TriggerTrap());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _motionDetected = false;
        _player = null;
    }

    private void FixedUpdate()
    {
        if (_motionDetected)
        {
            if (_state == trapState.ACTIVE && _player != null)
                Respawn.playerDied?.Invoke();
            if (_state == trapState.INACTIVE)
                StartCoroutine(TriggerTrap());
        }
    }

    private IEnumerator TriggerTrap()
    {
        _state = trapState.STARTED;
        _renderer.sprite = _sprites[(int)_state];
        yield return _speed;
        _state = trapState.ACTIVE;
        _renderer.sprite = _sprites[(int)_state];
        audioSource.Play();
        yield return _duration;
        _state = trapState.INACTIVE;
        _renderer.sprite = _sprites[(int)_state];
    }
}
