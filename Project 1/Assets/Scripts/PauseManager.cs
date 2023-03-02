using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

    private bool paused;
    [SerializeField] private GameObject[] menus;
    

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }


    }
    public void Pause()
    {
        paused = !paused;

        if (paused)
        {
            menus[0].SetActive(true);
            Time.timeScale = 0.0f;
        }
        else
        {
            foreach (GameObject menu in menus)
            {
                menu.SetActive(false);
            }
            Time.timeScale = 1.0f;
        }
    }
}
