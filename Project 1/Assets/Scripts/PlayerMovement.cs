using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Initialize")]
    private Rigidbody2D _rb;
    private Animator _animator;

    [Header("Settings")]
    [SerializeField] private float _speed = 1f;
    private Vector2 _movementInput = Vector2.zero;

    // assigns any unassigned variables
    private void Awake()
    {
        if (_rb == null)
            _rb = GetComponent<Rigidbody2D>();

        if (_animator == null)
            _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Respawn.playerDied += KillMovement;
        Respawn.playerRespawned += UnkillMovement;
    }

    private void OnDisable()
    {
        Respawn.playerDied -= KillMovement;
        Respawn.playerRespawned -= UnkillMovement;
    }

    // changes player velocity to match the input multiplied by the speed
    private void FixedUpdate()
    {
        _rb.velocity = _movementInput * _speed;
        if (_animator != null) // passes information to the animator to properly sync movement
        {
            _animator.SetFloat("Horizontal", _movementInput.x);
            _animator.SetFloat("Vertical", _movementInput.y);

        }

    }

    public void KillMovement()
    {
        _rb.velocity = Vector2.zero;
        _animator.SetBool("Alive", false);
    }

    public void UnkillMovement()
    {
        _animator.SetBool("Alive", true);
    }

    // reads the players input using Unity's input system
    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }

}
