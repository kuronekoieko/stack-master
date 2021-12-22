using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SplashController : MonoBehaviour
{
    [SerializeField] Image splashImage;
    Sequence sequence;
    public bool IsCompleteAnim { get; set; }
    bool isShowSplash = false;

    public void ShowSplash()
    {

        if (Application.isEditor && !isShowSplash)
        {
            IsCompleteAnim = true;
            return;
        }

        sequence = DOTween.Sequence()
        .AppendCallback(() =>
        {
            gameObject.SetActive(true);
            splashImage.SetAlpha(0);
            IsCompleteAnim = false;
        })
        .Append(DOTween.ToAlpha(() => splashImage.color, color => splashImage.color = color, 1f, 0.5f).SetEase(Ease.InSine))
        .OnComplete(() =>
        {
            IsCompleteAnim = true;
        });
    }


    public void HideSplash()
    {
        if (Application.isEditor && !isShowSplash)
        {
            return;
        }

        sequence = DOTween.Sequence()
        .Append(DOTween.ToAlpha(() => splashImage.color, color => splashImage.color = color, 0f, 0.5f).SetEase(Ease.OutSine))
        .OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
