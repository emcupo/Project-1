using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private bool _inFlight;
    [SerializeField] private float _maxSpeed = 5f;
    [Tooltip("Time in seconds before the object destroys itself. If the value is set to 0, the object will not be destroyed")]
    [SerializeField] private float _duration = 10f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        // if the duration is > than 0, destroy projectile object at the assigned time. At 0 the object never disappears
        if (_duration > 0)
            Destroy(gameObject, _duration);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_inFlight)
        {
            if (!collision.CompareTag("Trap"))
                Destroy(gameObject);

            // Upon being triggered by the player, respawn the player
            if (collision.CompareTag("Player"))
                Respawn.playerDied?.Invoke();
        }
    }

    // Makes the projectile fly upward
    private void FixedUpdate()
    {
        if (_rb.velocity.magnitude <= _maxSpeed && _inFlight)
            _rb.AddForce(transform.up, ForceMode2D.Impulse);
    }
    // used to set how long before the projectile self destructs
    public void setDuration(float duration)
    {
        _duration = duration;
    }

    public void isInFlight(bool setFlight)
    {
        _inFlight = setFlight;
    }
}
