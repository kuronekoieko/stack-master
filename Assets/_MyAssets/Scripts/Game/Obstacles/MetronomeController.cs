using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MetronomeController : MonoBehaviour
{
    [SerializeField] Transform body_transform;
    [SerializeField] float startOffsetTime_sec;
    [SerializeField] float cycleTime_sec;
    [SerializeField] float waitTime_sec;
    Sequence sequence;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayMethod(startOffsetTime_sec, () =>
        {
            sequence = DOTween.Sequence()
                               .Append(body_transform.DOLocalRotate(new Vector3(0, 0, 179.9f), cycleTime_sec / 2 - waitTime_sec).SetEase(Ease.OutBounce))
                               .AppendInterval(waitTime_sec)
                               .Append(body_transform.DOLocalRotate(new Vector3(0, 0, 0.1f), cycleTime_sec / 2 - waitTime_sec).SetEase(Ease.OutBounce))
                               .AppendInterval(waitTime_sec)
                               .SetLoops(-1);
        }));
    }

    // Update is called once per frame
    void Update()
    {

    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    IEnumerator DelayMethod(float delayTime_sec, Action action) { yield return new WaitForSeconds(delayTime_sec); action(); }
}
