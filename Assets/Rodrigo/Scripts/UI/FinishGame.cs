using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGame : MonoBehaviour
{

    public GameObject player1_win, player1_lose, player2_win, player2_lose;

    bool inputAllowed;

    private void Awake()
    {
        inputAllowed = true;

        if(PlayerPrefs.GetInt(DataUtils.playerScore1) == 1)
        {
            player1_win.SetActive(true);
            player2_win.SetActive(false);
            player1_lose.SetActive(false);
            player2_lose.SetActive(true);
        }
        else
        {
            player1_win.SetActive(false);
            player2_win.SetActive(true);
            player1_lose.SetActive(true);
            player2_lose.SetActive(true);
        }
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
