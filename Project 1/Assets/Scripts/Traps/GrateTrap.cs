using System.Collections;
using UnityEngine;

public class GrateTrap : MonoBehaviour
{
    [Header("Apperance")]
    private SpriteRenderer[] _grates;

    [Tooltip("Use 4 slots and they will be active for the following... " +
        "0. Inactive period 1. warning period 2. active period 3. reset period")]
    [SerializeField] private Sprite[] sprites = new Sprite[3];
    [SerializeField] private int _index = 0;


    [Header("Timing")]
    [SerializeField] private float _initialDelay = 0f;

    [Space][SerializeField] private float _changeTimer = 1f;

    private WaitForSeconds _changeState;
    private WaitForSeconds _resetDuration;

    private void Awake()
    {
        _changeState = new WaitForSeconds(_changeTimer);
        _resetDuration = new WaitForSeconds(_changeTimer / 2);

        _grates = GetComponentsInChildren<SpriteRenderer>();

    }

    private void Start()
    {
        Invoke("StartTrap", _initialDelay);
    }

    private void StartTrap()
    {
        StartCoroutine(NextState());
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && _index == 3)
        {
            collision.GetComponent<Respawn>().RespawnPlayer();
        }
    }

    private IEnumerator NextState()
    {

        if (_index < _grates.Length - 1)
        {
            _index++;
            yield return _changeState;
        }
        else
        {
            _index = 0;
            yield return _resetDuration;
        }
        ChangeSprites();
        StartCoroutine(NextState());
    }

    private void ChangeSprites()
    {
        for (int i = 0; i < _grates.Length; i++)
        {
            _grates[i].sprite = sprites[_index];
        }

    }
}
