using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using System;

public class ClearCanvasManager : BaseCanvasManager
{
    [SerializeField] Button nextButton;
    [SerializeField] Button giftButton;
    [SerializeField] RectTransform gems;
    [SerializeField] Image titleImage;
    [SerializeField] Text currencyCountText;
    [SerializeField] SkinProgress skinProgress;
    [SerializeField] RectTransform gemImageRt;
    [SerializeField] GemCollectAnimManager gemCollectAnimManager;
    Sequence nextButtonSequence;
    Sequence giftButtonSequence;
    Tween emojiRotateTween;
    Tween emojiScaleTween;
    int currencyBaseCount = 15;


    public override void OnStart()
    {
        base.SetScreenAction(thisScreen: ScreenState.Clear);

        nextButton.onClick.AddListener(OnClickNextButton);
        giftButton.onClick.AddListener(OnClickGiftButton);
        gameObject.SetActive(false);
        skinProgress.OnStart();
        gemCollectAnimManager.OnStart(20);

    }

    public override void OnSceneLoaded()
    {
    }

    public override void OnUpdate()
    {


    }

    protected override void OnOpen()
    {
        skinProgress.OnOpen();
        nextButton.gameObject.SetActive(false);
        giftButton.gameObject.SetActive(false);
        titleImage.gameObject.SetActive(true);
        gems.gameObject.SetActive(true);

        SoundManager.i.PlayOneShot(1);
        SaveData.i.lastClearedDisplayStageNum = StageTransManager.i.CurrentDisplayStageNum;
        FirebaseAnalyticsManager.i.LogEvent_StageClear(StageTransManager.i.CurrentDisplayStageNum);

        int curencyCount = Mathf.RoundToInt(Variables.goalRate * currencyBaseCount);
        SaveData.i.currencyCount += curencyCount;
        currencyCountText.text = "+" + curencyCount.ToString();

        SaveDataManager.i.Save();

        DOVirtual.DelayedCall(1.0f, () =>
        {
            gameObject.SetActive(true);
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                skinProgress.Anim();
                gemCollectAnimManager.Anim(gemImageRt.position, 0.5f, () =>
                {
                    OnCompleteSkinProgress(skinProgress.IsMax);
                });
            });

            giftButton.transform.localScale = Vector3.one;
            giftButtonSequence = DOTween.Sequence()
            .Append(giftButton.transform.DOScale(Vector3.one * 1.1f, 0.5f))
            .Append(giftButton.transform.DOScale(Vector3.one, 0.5f));
            giftButtonSequence.SetLoops(-1);

            nextButton.transform.localScale = Vector3.one;
            nextButtonSequence = DOTween.Sequence()
            .Append(nextButton.transform.DOScale(Vector3.one * 1.1f, 0.5f))
            .Append(nextButton.transform.DOScale(Vector3.one, 0.5f));
            nextButtonSequence.SetLoops(-1);
        });
    }



    void OnCompleteSkinProgress(bool isMax)
    {
        if (isMax)
        {
            gems.gameObject.SetActive(false);
            titleImage.gameObject.SetActive(false);
            return;
        }

        bool isNextGiftScreen = StageTransManager.i.CurrentDisplayStageNum % 5 == 0;
        // isNextGiftScreen = true; //デバッグ用
        nextButton.gameObject.SetActive(!isNextGiftScreen);
        giftButton.gameObject.SetActive(isNextGiftScreen);
    }

    protected override void OnClose()
    {
        gameObject.SetActive(false);
        nextButtonSequence.Kill();
        giftButtonSequence.Kill();
        emojiRotateTween.Kill();
        emojiScaleTween.Kill();
        skinProgress.OnClose();
    }

    void OnClickNextButton()
    {
        SoundManager.i.PlayOneShot(0);
        StageTransManager.i.LoadNextStage();
    }



    void OnClickHomeButton()
    {
        // Variables.screenState = ScreenState.Home;
        SoundManager.i.PlayOneShot(0);
    }

    void OnClickGiftButton()
    {
        SoundManager.i.PlayOneShot(0);
        Variables.screenState = ScreenState.Gift;
    }
}