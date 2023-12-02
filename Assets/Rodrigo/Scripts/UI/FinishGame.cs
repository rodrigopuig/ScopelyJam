using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGame : MonoBehaviour
{

    bool inputAllowed;

    private void Awake()
    {
        inputAllowed = true;
    }

    public void Replay()
    {
        if (inputAllowed)
        {
            inputAllowed = false;
            Fader.FadeIn(() => StartCoroutine(CoroutineUtils.DoAfterDelay(1, () => UnityEngine.SceneManagement.SceneManager.LoadScene("ActionPhase"))));
        }
    }

    public void GoToMainMenu()
    {
        if (inputAllowed)
        {
            inputAllowed = false;
            Fader.FadeIn(() => StartCoroutine(CoroutineUtils.DoAfterDelay(1, () => UnityEngine.SceneManagement.SceneManager.LoadScene("Menu"))));
        }
    }
}