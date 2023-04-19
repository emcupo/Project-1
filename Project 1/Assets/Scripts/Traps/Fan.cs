using UnityEngine;

public class Fan : MonoBehaviour
{
    [SerializeField] private float _power = 100f;
    [SerializeField] private ParticleSystem _wind;
    [SerializeField] private Transform[] points;

    [SerializeField] private float mult = 0.5f;

    private RaycastHit2D _hit;
    private Rigidbody2D _rb;

    private void Update()
    {
        float[] distances = new float[points.Length];
        Transform point;
        for (int i = 0; i < points.Length; i++)
        {
            point = points[i];
            _hit = Physics2D.Raycast(point.position, transform.up);
            if (_hit.transform.CompareTag("Player"))
            {
                if (_rb == null)
                    _rb = _hit.transform.GetComponent<Rigidbody2D>();
                push(_rb);
            }
            Debug.DrawRay(point.position, transform.up * _hit.distance);
            distances[i] = _hit.distance;
        }

        float min = distances[0];
        foreach (var item in distances)
        {
            if (item > min)
                continue;
            else
                min = item;
        }
        var main = _wind.main;
        main.startLifetime = min * mult;

    }

    /*
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                push(collision.GetComponent<Rigidbody2D>());
            }
        }*/

    private void push(Rigidbody2D rb)
    {
        rb.AddForce(transform.up * _power * Time.deltaTime);
    }
}
