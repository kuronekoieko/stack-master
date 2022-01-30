
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PressMachineController : MonoBehaviour
{
    [SerializeField] Transform body_transform;
    [SerializeField] Transform arm_transform;
    [SerializeField] Transform bevel_transform;
    [Space(10)]
    [SerializeField] float height;
    [SerializeField, Range(0f, 7.9f)] float pressDistance;
    [SerializeField] float startOffsetTime_sec;
    [SerializeField] float pressCompleteTime_sec;
    [SerializeField] float intervalTime_press2reset_sec;
    [SerializeField] float resetCompleteTime_sec;
    [SerializeField] float rechargeTime_sec;

    Sequence bevel_sequence;
    Sequence arm_sequence;
    float armLength_max;

    //<Start & Update>ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayMethod(startOffsetTime_sec, () =>
        {
            bevel_sequence = DOTween.Sequence()
                                     .AppendInterval(rechargeTime_sec)
                                     .Append(bevel_transform.DOLocalMoveX(pressDistance + 1.1f, pressCompleteTime_sec))
                                     .AppendInterval(intervalTime_press2reset_sec)
                                     .Append(bevel_transform.DOLocalMoveX(1.1f, resetCompleteTime_sec).SetEase(Ease.Linear))
                                     .SetLoops(-1);
            arm_sequence = DOTween.Sequence()
                           .AppendInterval(rechargeTime_sec)
                           .Append(arm_transform.DOScaleX(pressDistance + 0.55f, pressCompleteTime_sec))
                           .AppendInterval(intervalTime_press2reset_sec)
                           .Append(arm_transform.DOScaleX(0.55f, resetCompleteTime_sec).SetEase(Ease.Linear))
                           .SetLoops(-1);
        }));
    }

    void OnValidate()
    {
        body_transform.localScale = new Vector3(1.75f, height * 2 + 1, 2);
        arm_transform.localPosition = new Vector3(0.5f, height + 0.25f, 0);
        bevel_transform.localPosition = new Vector3(1.1f, height + 0.25f, 0);
        bevel_transform.localScale = new Vector3(0.25f, height, 1.5f);
    }

    //ーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーーー
    IEnumerator DelayMethod(float delayTime_sec, Action action) { yield return new WaitForSeconds(delayTime_sec); action(); }
}
