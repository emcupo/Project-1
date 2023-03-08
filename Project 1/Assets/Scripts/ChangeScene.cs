using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeScene : MonoBehaviour
{
    [SerializeField] private bool isActive = true;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
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
}
