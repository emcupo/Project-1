using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : StateMachineBehaviour
{
    public void LoadNewScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Intro E");
    }
}
