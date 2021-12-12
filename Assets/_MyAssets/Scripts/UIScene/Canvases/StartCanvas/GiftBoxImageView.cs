using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GiftBoxImageView : MonoBehaviour
{

    void Start()
    {
        float angle = 20f;
        Sequence sequence = DOTween.Sequence()
        .Append(transform.DORotate(Vector3.forward * angle, 0.3f))
        .Append(transform.DORotate(Vector3.forward * -angle, 0.3f))
        .Append(transform.DORotate(Vector3.forward * angle, 0.3f))
        .Append(transform.DORotate(Vector3.forward * -angle, 0.3f))
        .Append(transform.DORotate(Vector3.forward * 0f, 0.5f))
        .AppendInterval(0.5f);
        sequence.SetLoops(-1);
    }


}
