using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Respawn : MonoBehaviour
{
    [Tooltip("How long it takes for the scene to restart when the player needs to respawn")]
    [SerializeField] private float _respawnTimer = 1.5f;
    private WaitForSeconds _respawn;

    private PlayerMovement _movement;
    private Collider2D _collider;

    private SpriteRenderer _sprite;

    private void Awake()
    {
        // assigns unassigned variables
        _movement = GetComponent<PlayerMovement>();
        _collider = GetComponent<Collider2D>();
        _sprite = GetComponent<SpriteRenderer>();

        // creates the WaitForSeconds used by the restart scene coroutine
        _respawn = new WaitForSeconds(_respawnTimer);
    }
    public void RespawnPlayer()
    {
        _movement.StopEverything();
        _movement.enabled = false;
        _collider.enabled = false;


        StartCoroutine(Fade(_sprite.color, new Color(1f, 1f, 1f, 0f), _respawnTimer - 0.25f));
        StartCoroutine(RestartScene());
    }

    private IEnumerator RestartScene()
    {
        yield return _respawn;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Fades sprite color within a time frame
    IEnumerator Fade(Color start, Color end, float time)
    {
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            _sprite.color = Color.Lerp(start, end, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _sprite.color = end;
    }
}
