using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float _maxSpeed = 5f;
    [Tooltip("Time in seconds before the object destroys itself. If the value is set to 0, the object will not be destroyed")]
    [SerializeField] private float _duration = 10f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        if (_duration > 0)
            Destroy(gameObject, _duration);
    }

    private void FixedUpdate()
    {
        if (_rb.velocity.magnitude <= _maxSpeed)
            _rb.AddForce(transform.up, ForceMode2D.Impulse);
    }

    public void setDuration(float duration)
    {
        _duration = duration;
    }
}
