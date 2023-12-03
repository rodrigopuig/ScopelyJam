using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class ButtonClick : MonoBehaviour, IPointerClickHandler
{
    Button myButton;

    private void Awake()
    {
        myButton = GetComponent<Button>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (myButton.interactable && AudioManager.Instance != null) 
        {
            AudioManager.Instance.PlayButtonSound();
        }
    }
}
