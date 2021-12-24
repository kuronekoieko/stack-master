using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HammerController : MonoBehaviour
{
    [SerializeField] float cycleTime_sec = 2;
    [SerializeField] float startOffset_sec = 0;
    Sequence sequence;
    Vector3 defaultRotation;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    // Start is called before the first frame update
    void Start()
    {
        defaultRotation = transform.localEulerAngles;
        StartCoroutine(DelayMethod(startOffset_sec, () =>
        {
            sequence = DOTween.Sequence()
                          .Append(transform.DORotate(defaultRotation + new Vector3(0, 179.9f, 0), cycleTime_sec / 2).SetEase(Ease.OutBounce))
                          .Append(transform.DORotate(defaultRotation, cycleTime_sec / 2).SetEase(Ease.OutBounce))
                          .SetLoops(-1);
        }));
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    IEnumerator DelayMethod(float delayTime_sec, Action action) { yield return new WaitForSeconds(delayTime_sec); action(); }
}
