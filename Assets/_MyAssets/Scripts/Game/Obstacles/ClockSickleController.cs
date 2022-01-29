using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClockSickleController : MonoBehaviour
{
    [SerializeField] bool isClockwise = true;
    [SerializeField] float startOffsetTime_sec;
    [SerializeField] float rotateCompleteTime_sec;
    [SerializeField] float rechargeTime_sec;

    Sequence sequence;

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayMethod(startOffsetTime_sec, () =>
        {
            if (isClockwise)
            {
                sequence = DOTween.Sequence()
                                   .Append(transform.DORotate(new Vector3(0, 360, 0), rotateCompleteTime_sec, RotateMode.FastBeyond360).SetEase(Ease.InOutSine))
                                   .AppendInterval(rechargeTime_sec)
                                   .SetLoops(-1);
            }
            else
            {
                sequence = DOTween.Sequence()
                                   .Append(transform.DORotate(new Vector3(0, -360, 0), rotateCompleteTime_sec, RotateMode.FastBeyond360).SetEase(Ease.InOutSine))
                                   .AppendInterval(rechargeTime_sec)
                                   .SetLoops(-1);
            }
        }));
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    IEnumerator DelayMethod(float delayTime_sec, Action action) { yield return new WaitForSeconds(delayTime_sec); action(); }
}