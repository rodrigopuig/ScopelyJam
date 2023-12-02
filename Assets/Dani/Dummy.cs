using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MANUELITO;
using Rodrigo;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    public AnimationPair[] animationPairs;

    public void Update()
    {
        foreach (var _pair in animationPairs)
        {
            if (Input.GetKey(_pair.keyCode) || Input.GetKeyDown(_pair.keyCode))
                if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(_pair.animationName))
                {
                    if(_pair.action == AnimationPair.Action.PressDown && Input.GetKeyDown(_pair.keyCode))
                    GetComponent<Animator>().Play(_pair.animationName, 0);
                    else if(_pair.action == AnimationPair.Action.PressHold && Input.GetKeyDown(_pair.keyCode))
                        GetComponent<Animator>().Play(_pair.animationName, 0);
                } 
        }

        if(!Input.anyKey)
        {
            //idle
        }

    }

    [System.Serializable]
    public class AnimationPair
    {
        public enum Layer { Head, Torso, Legs}
        public enum Action { PressDown, PressHold}
        public Action action;
        public KeyCode keyCode;
        public string animationName;
    }
}
