using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class SpikeTrap : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private bool _active;
    private bool _startedUp;
    private bool _inTrap;

    private Sprite _inactiveSprite;
    [Tooltip("How the trap will look while active, when inactive it will revert to its original sprite")]
    [SerializeField] private Sprite _activeSprite;

    [SerializeField] private float _triggerSpeed = 1f;
    [SerializeField] private float _triggerDuration = 2f;

    private WaitForSeconds _speed;
    private WaitForSeconds _duration;
    private GameObject _player;

    private void Awake()
    {
        if (_renderer == null)
            _renderer = GetComponent<SpriteRenderer>();

        _inactiveSprite = _renderer.sprite;

        if (_activeSprite == null)
            _activeSprite = _inactiveSprite;

        _speed = new WaitForSeconds(_triggerSpeed);
        _duration = new WaitForSeconds(_triggerDuration);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _inTrap = true;
        if (collision.CompareTag("Player"))
        {
            _player = collision.gameObject;
            if (!_startedUp)
                StartCoroutine(TriggerTrap());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _inTrap = false;
    }

    private void FixedUpdate()
    {
        if (_inTrap)
        {
            if (_active)
                hurtPlayer();

            if (!_startedUp)
                StartCoroutine(TriggerTrap());
        }
    }
    private void hurtPlayer()
    {
        Debug.Log(_player.name + " has been hurt");
    }

    private IEnumerator TriggerTrap()
    {
        _startedUp = true;

        yield return _speed;
        _active = true;
        _renderer.sprite = _activeSprite;

        yield return _duration;
        _active = false;
        _renderer.sprite = _inactiveSprite;

        _startedUp = false;
    }
}
