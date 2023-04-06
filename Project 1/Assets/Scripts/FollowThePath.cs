using UnityEngine;

public class FollowThePath : MonoBehaviour
{
    public float speed;
    public float startWaitTime;
    public Transform[] moveSpots;

    private int randomSpot;
    private float waitTime;

    [SerializeField] private LineRenderer _lr;

    private void Awake()
    {
        if (_lr == null)
            _lr = gameObject.GetComponentInChildren<LineRenderer>();

        GenerateMark();
    }

    private void Start()
    {
        randomSpot = Random.Range(0, moveSpots.Length);

    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);

        // Check if the distance between this and the movepoint is less than 0.2f (tollerange range)
        if (Vector2.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                randomSpot = Random.Range(0, moveSpots.Length);

                waitTime = startWaitTime;
            }
            else
            {
                waitTime = -Time.deltaTime;
            }
        }
    }

    private void GenerateMark()
    {
        Vector3[] positions = new Vector3[moveSpots.Length];
        for (int i = 0; i < moveSpots.Length; i++)
        {
            positions[i] = moveSpots[i].position;
        }
        _lr.positionCount = moveSpots.Length;
        _lr.SetPositions(positions);
    }
}