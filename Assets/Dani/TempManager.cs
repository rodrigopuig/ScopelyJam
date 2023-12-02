using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempManager : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentSceneIndex);
            Time.timeScale = 1;
        }
    }
}
