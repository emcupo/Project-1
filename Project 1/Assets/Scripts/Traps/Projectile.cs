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
        // if the duration is > than 0, destroy projectile object at the assigned time. At 0 the object never disappears
        if (_duration > 0)
            Destroy(gameObject, _duration);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Trap"))
            Destroy(gameObject);

        // Upon being triggered by the player, respawn the player
        if (collision.CompareTag("Player"))
            collision.GetComponent<Respawn>().RespawnPlayer();

    }

    // Makes the projectile fly upward
    private void FixedUpdate()
    {
        if (_rb.velocity.magnitude <= _maxSpeed)
            _rb.AddForce(transform.up, ForceMode2D.Impulse);
    }
    // used to set how long before the projectile self destructs
    public void setDuration(float duration)
    {
        _duration = duration;
    }
}
