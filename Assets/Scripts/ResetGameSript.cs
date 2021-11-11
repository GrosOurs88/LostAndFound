using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGameSript : MonoBehaviour
{
    public KeyCode inputReset;

    void Update()
    {
        if(Input.GetKeyDown(inputReset))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
}
