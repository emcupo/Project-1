using System.Collections;
using UnityEngine;

public class Crossbow : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Projectile _arrow;

    private WaitForSeconds _rest;
    private WaitForSeconds _shot;

    [Header("Projectile")]
    [SerializeField] private float _duration = 5f;
    [SerializeField] private float _restTime = 1f;
    [SerializeField] private float _shotCooldown = 1f;

    private void Awake()
    {
        _rest = new WaitForSeconds(_restTime);
        _shot = new WaitForSeconds(_shotCooldown);
    }

    private void Start()
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
        arrow.enabled = false;
        yield return _rest;
        arrow.enabled = true;
        yield return _shot;
        StartCoroutine(shootArrow());
    }
}
