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

    private void Awake()
    {
        if (_rb == null)
            _rb = GetComponent<Rigidbody2D>();

        if (_animator == null)
            _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        _rb.velocity = _movementInput * _speed;
        if (_animator != null)
        {
            _animator.SetFloat("Horizontal", _movementInput.x);
            _animator.SetFloat("Vertical", _movementInput.y);
        }

    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }

}
