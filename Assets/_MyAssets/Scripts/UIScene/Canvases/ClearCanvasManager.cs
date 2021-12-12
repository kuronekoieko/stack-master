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
    [SerializeField] Text titleText;
    [SerializeField] Image emojiImage;
    [SerializeField] Text currencyCountText;
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
    }

    public override void OnSceneLoaded()
    {
    }

    public override void OnUpdate()
    {


    }

    protected override void OnOpen()
    {
        bool isNextGiftScreen = StageTransManager.i.CurrentDisplayStageNum % 5 == 0;
        // isNextGiftScreen = true; //デバッグ用
        nextButton.gameObject.SetActive(!isNextGiftScreen);
        giftButton.gameObject.SetActive(isNextGiftScreen); ;

        SoundManager.i.PlayOneShot(1);
        SaveData.i.lastClearedDisplayStageNum = StageTransManager.i.CurrentDisplayStageNum;

        int curencyCount = Mathf.RoundToInt(Variables.goalRate * currencyBaseCount);
        SaveData.i.currencyCount += curencyCount;
        currencyCountText.text = "+" + curencyCount.ToString();

        SaveDataManager.i.Save();

        DOVirtual.DelayedCall(1.0f, () =>
        {
            gameObject.SetActive(true);
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

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

            emojiImage.transform.eulerAngles = Vector3.forward * -40f;
            emojiRotateTween = emojiImage.transform.DORotate(Vector3.forward * 40f, 1.5f).SetEase(Ease.InOutFlash, 2).SetLoops(-1);
            emojiImage.transform.localScale = Vector3.one;
            emojiScaleTween = emojiImage.transform.DOScale(Vector3.one * 1.1f, 1.5f).SetEase(Ease.InOutFlash, 4).SetLoops(-1);
        });
    }

    protected override void OnClose()
    {
        gameObject.SetActive(false);
        nextButtonSequence.Kill();
        giftButtonSequence.Kill();
        emojiRotateTween.Kill();
        emojiScaleTween.Kill();
    }

    void OnClickNextButton()
    {

        SoundManager.i.PlayOneShot(0);
        Time.timeScale = 0;
        ShowInterstitial(() =>
        {
            StageTransManager.i.LoadNextStage();
            Time.timeScale = 1;
        });

    }

    void ShowInterstitial(Action onHidden)
    {
        if (StageTransManager.i.CurrentDisplayStageNum % 3 != 0)
        {
            onHidden();
            return;
        }

        MaxSdkInterstitial.i.Show(onHidden);
    }

    void OnClickHomeButton()
    {
        // Variables.screenState = ScreenState.Home;
        SoundManager.i.PlayOneShot(0);
    }

    void OnClickGiftButton()
    {
        SoundManager.i.PlayOneShot(0);
        Time.timeScale = 0;
        ShowInterstitial(() =>
        {
            Variables.screenState = ScreenState.Gift;
            Time.timeScale = 1;
        });
    }
}