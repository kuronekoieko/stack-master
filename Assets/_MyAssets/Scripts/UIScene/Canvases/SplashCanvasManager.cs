using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SplashCanvasManager : BaseCanvasManager
{
    [SerializeField] Image splashImage;
    public override void OnStart()
    {
        base.SetScreenAction(thisScreen: ScreenState.Splash);
        gameObject.SetActive(false);
    }

    public override void OnUpdate()
    {

    }

    protected override void OnOpen()
    {
        gameObject.SetActive(true);
        splashImage.SetAlpha(0);
        Sequence sequence = DOTween.Sequence()
        .Append(DOTween.ToAlpha(() => splashImage.color, color => splashImage.color = color, 1f, 0.5f).SetEase(Ease.InSine))
        .AppendInterval(2f)
        .Append(DOTween.ToAlpha(() => splashImage.color, color => splashImage.color = color, 0f, 0.5f).SetEase(Ease.OutSine))
        .OnComplete(() =>
        {
            StageTransManager.i.ReLoadStage();
            // StartCoroutine(WaitFirebaseInitialize());
        });
    }

    /*   private IEnumerator WaitFirebaseInitialize()
        {
            while (FirebaseScript.State == FirebaseScript.InitializedState.None)
            {
                yield return new WaitForEndOfFrame();
            }

            Sequence sequence = DOTween.Sequence()
            .Append(DOTween.ToAlpha(() => splashImage.color, color => splashImage.color = color, 0f, 0.5f).SetEase(Ease.OutSine))
            .OnComplete(() =>
            {
                StageTransManager.i.ReLoadStage();
            });
        }*/


    protected override void OnClose()
    {
        gameObject.SetActive(false);
        MaxSdkBanner.i.Show();
    }

    public override void OnSceneLoaded()
    {

    }
}
