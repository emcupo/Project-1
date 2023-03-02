using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private EventSystem _system;
    private void Awake()
    {
        if (PauseManager.instance != null && SceneManager.GetActiveScene().buildIndex == 0)
            Destroy(PauseManager.instance.gameObject);
    }

    // called first
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EventSystem system = EventSystem.current;
        if (system == null)
        {
            Instantiate(_system, Vector3.zero, Quaternion.identity);
        }
    }

    public void PlayGame()
    {
        int index = SceneManager.GetActiveScene().buildIndex;

        if (index < SceneManager.sceneCountInBuildSettings - 1)
            SceneManager.LoadScene(++index);
        else
            Debug.LogWarning("No more levels exist");
    }

    public void goToScene(int index)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(index);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
