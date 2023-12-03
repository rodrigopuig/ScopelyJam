using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;

public class RoundNotificator : MonoBehaviour
{
    public Transform trOrigin;
    public Transform trEnd;

    public Transform trPanel;

    public LineRenderer lr_link;

    public TextMeshPro txtRound;

    Vector3[] positions;
    Vector3 offset;

    Vector3 origin, end;

    // Start is called before the first frame update
    void Start()
    {
        positions = new Vector3[2];

        origin = Vector3.zero;
        end =  trEnd.position - trOrigin.position - Vector3.up * 1;

        trPanel.position = origin;
    }

    // Update is called once per frame
    void Update()
    {
        positions[0] = trOrigin.position;
        positions[1] = trEnd.position;
        lr_link.SetPositions(positions);

        offset.z = 1;
        offset.x = 0.5f * Mathf.Sin(Time.time * 120 * Mathf.Deg2Rad);
        trPanel.position = trOrigin.position + offset;
    }

    public void PlayAnimation(int _round, System.Action  _onComplete = null)
    {
        txtRound.text = $"Round {_round}";
        StartCoroutine(AnimationCoroutine(_onComplete));
    }

    IEnumerator AnimationCoroutine(System.Action _onComplete = null)
    {
        bool _stepComplted = false;

        float _value = 0;
        DOTween.To(() => _value, x => _value = x, 1, 1).SetUpdate(true)
            .OnUpdate(()=>
            {
                offset.y = Mathf.Lerp(origin.y, end.y, _value);
            })
            .OnComplete(() => _stepComplted = true);

        yield return new WaitUntil(() => _stepComplted);
        yield return new WaitForSecondsRealtime(2);

        _stepComplted = false;
        DOTween.To(() => _value, x => _value = x, 0, 1).SetUpdate(true)
             .OnUpdate(() =>
             {
                 offset.y = Mathf.Lerp(origin.y, end.y, _value);
             })
            .OnComplete(() => _stepComplted = true);

        yield return new WaitUntil(() => _stepComplted);

        _onComplete?.Invoke();
    }
}
