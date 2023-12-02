using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeFrameManager : MonoBehaviour
{
    public static FreezeFrameManager Instance;
    [SerializeField] float freezeTimeDuration = 0.15f;
    [SerializeField] float bulletTimeSpeed = 0.15f;
    static bool frameFrozen;
    static bool bulletTime;
    static WaitForSecondsRealtime freezeWait;

    private void Awake()
    {
        Instance = this;
        freezeWait = new WaitForSecondsRealtime(freezeTimeDuration);
    }

    public static void FreezeFrame()
    {
        if (!frameFrozen)
        {
            Instance.StartCoroutine(DoFreezeFrame(0));
        }
    }
    public static void BulletTime()
    {
        if (!bulletTime)
        {
            Instance.StartCoroutine(DoFreezeFrame(Instance.bulletTimeSpeed));
        }
    }

    static IEnumerator DoFreezeFrame(float timescale)
    {
        if (timescale > 0) bulletTime = true;
        else frameFrozen = true;

        var original = Time.timeScale;
        Time.timeScale = timescale;
        yield return freezeWait;
        Time.timeScale = original;

        if (timescale > 0) bulletTime = false;
        else frameFrozen = false;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            if (freezeWait == null || freezeWait.waitTime != freezeTimeDuration)
            {
                freezeWait = new WaitForSecondsRealtime(freezeTimeDuration);
            }
        } 
    }
#endif
}
