using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeScene : MonoBehaviour
{
    [SerializeField] private bool isActive = true;
    void OnCollisionEnter2D(Collision2D collider)
    {
        if (isActive)
        {
            int index = SceneManager.GetActiveScene().buildIndex;

            if (index < SceneManager.sceneCountInBuildSettings - 1)
                SceneManager.LoadScene(++index);
            else
                Debug.LogWarning("No more levels exist");
        }

    }
}
