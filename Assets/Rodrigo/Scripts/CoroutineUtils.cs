using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoroutineUtils
{
    public static IEnumerator DoAfterFrames(int _amount, System.Action _action)
    {
        for (int i = 0; i < _amount; i++)
            yield return null;

        _action.Invoke();
    }

    public static IEnumerator DoAfterDelay(float _time, System.Action _action)
    {
        yield return new WaitForSeconds(_time);

        _action.Invoke();
    }
}
