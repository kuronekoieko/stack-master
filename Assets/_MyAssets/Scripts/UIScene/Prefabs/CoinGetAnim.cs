using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

/*
DOTweenのコールバックをいくつか紹介
https://qiita.com/AzuQiita/items/822e382473e6c0db8237


*/
public class CoinGetAnim : MonoBehaviour
{
    [SerializeField] RectTransform targetRT;
    RectTransform rectTransform;
    Vector3 startPos;
    public void OnStart()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.zero;

        startPos = rectTransform.anchoredPosition;
    }


    public void Anim(Action OnComplete)
    {
        Sequence sequence = DOTween.Sequence()
        .OnStart(() =>
        {
            rectTransform.anchoredPosition = startPos;
        })
        .Append(rectTransform.DOScale(Vector3.one * 1.8f, 0.5f))
        .Append(rectTransform.DOLocalMove(targetRT.anchoredPosition, 0.5f))
        .AppendCallback(() =>
        {
            rectTransform.localScale = Vector3.zero;
            OnComplete();
        })
        .Append(DOVirtual.DelayedCall(1f, () =>
        {
            Variables.screenState = ScreenState.Game;
        }));
    }

}
