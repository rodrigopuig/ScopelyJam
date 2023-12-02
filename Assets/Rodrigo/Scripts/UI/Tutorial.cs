using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;


public class Tutorial : MonoBehaviour
{
    public static System.Action onCloseTutorial;

    public Canvas canvas;
    public PlayerUIInput[] inputs;
    public GameObject[] objectsToDisableOnTutorialClose;

    // Update is called once per frame
    void Update()
    {
        foreach(var input in inputs)
        {
            if (Input.GetKey(input.keyCode))
                input.imgKey.color = Color.green;
            else
                input.imgKey.color = Color.white;
        }
    }

    public void Continue()
    {
        canvas.enabled = false;

        foreach (var go in objectsToDisableOnTutorialClose)
            go.SetActive(false);

        onCloseTutorial?.Invoke();
    }

    private void OnDestroy()
    {
        onCloseTutorial = null;
    }

    [System.Serializable]
    public class PlayerUIInput
    {
        public KeyCode keyCode;
        public Image imgKey;
    }
}
