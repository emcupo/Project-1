using UnityEngine;

public class Fan : MonoBehaviour
{
    [SerializeField] private float _power = 100f;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Rigidbody2D>().AddForce(transform.up * _power);
        }
    }
}
