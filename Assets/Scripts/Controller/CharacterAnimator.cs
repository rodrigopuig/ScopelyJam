using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    Animator myAnim;
    const string animParamID = "walking";

    private void Awake()
    {
        myAnim = GetComponent<Animator>();
    }

    public void UpdateAnimParams(float movSpeed)
    {
        myAnim.SetBool(animParamID, movSpeed > 0);
    }

}
