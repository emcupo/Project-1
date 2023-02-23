using System.Collections;
using UnityEngine;

public class Crossbow : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Projectile _arrow;


    [Header("Crossbow")]
    [SerializeField] private float InitialDelay = 1.0f;
    [SerializeField] private float _restTime = 1f;
    [SerializeField] private float _shotCooldown = 1f;

    private AudioSource _audioSource;

    private WaitForSeconds _rest;
    private WaitForSeconds _shot;

    [Header("Projectile")]
    [SerializeField] private float _duration = 5f;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _rest = new WaitForSeconds(_restTime);
        _shot = new WaitForSeconds(_shotCooldown);
    }

    private void Start()
    {
        Invoke("StartShooting", InitialDelay);
    }

    private void StartShooting()
    {
        if (_arrow != null && _spawnPoint != null)
            StartCoroutine(shootArrow());
        else
            Debug.LogWarning("Unassinged references");
    }
    private IEnumerator shootArrow()
    {
        Projectile arrow = Instantiate(_arrow, _spawnPoint, false);
        _arrow.setDuration(_duration);
        arrow.isInFlight(false);
        yield return _rest;
        _audioSource.Play();
        arrow.isInFlight(true);
        yield return _shot;
        StartCoroutine(shootArrow());
    }
}
