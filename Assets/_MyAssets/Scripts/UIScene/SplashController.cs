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

    public void ShowSplash()
    {
        sequence = DOTween.Sequence()
        .AppendCallback(() =>
        {
            Debug.Log("テスト スプラッシュ開始");
            gameObject.SetActive(true);
            splashImage.SetAlpha(0);
            IsCompleteAnim = false;
        })
        .Append(DOTween.ToAlpha(() => splashImage.color, color => splashImage.color = color, 1f, 0.5f).SetEase(Ease.InSine))
        .OnComplete(() =>
        {
            Debug.Log("テスト スプラッシュフェードイン終了");
            IsCompleteAnim = true;
        });

    }


    public void HideSplash()
    {
        sequence = DOTween.Sequence()
        .Append(DOTween.ToAlpha(() => splashImage.color, color => splashImage.color = color, 0f, 0.5f).SetEase(Ease.OutSine))
        .OnComplete(() =>
        {
            // StageTransManager.i.ReLoadStage();
            // StartCoroutine(WaitFirebaseInitialize());
            gameObject.SetActive(false);
            Debug.Log("テスト スプラッシュおわり");
        });
    }
}
