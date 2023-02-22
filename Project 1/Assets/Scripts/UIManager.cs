using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
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
        SceneManager.LoadScene(index);
    }

    public void Quit() {
        Application.Quit();
    }
}
