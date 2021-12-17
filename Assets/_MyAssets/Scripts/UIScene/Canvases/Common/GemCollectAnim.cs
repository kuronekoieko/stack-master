using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class GemCollectAnim : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    public void OnInstansiate()
    {
        gameObject.SetActive(false);
    }

    public void Anim(Vector3 startPos, Vector3 offset, Vector3 endPos, float delay, Action OnMoveEnd = null)
    {
        gameObject.SetActive(true);
        rectTransform.position = startPos;
        rectTransform.localScale = Vector3.zero;
        Sequence sequence = DOTween.Sequence()
        .Append(rectTransform.DOScale(Vector3.one * 1.3f, 0.5f).SetEase(Ease.OutBack))
        .Join(rectTransform.DOMove(startPos + offset, 0.5f))
        .AppendInterval(0.5f + delay)
        .Append(rectTransform.DOMove(endPos, 1.0f))
        .Join((rectTransform.DOScale(Vector3.zero, 1.0f)).SetEase(Ease.InCubic))
        .OnComplete(() =>
        {
            gameObject.SetActive(false);
            if (OnMoveEnd != null) OnMoveEnd();
        });
    }
}
