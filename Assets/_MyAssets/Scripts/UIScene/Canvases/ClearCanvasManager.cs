using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using System;

public class ClearCanvasManager : BaseCanvasManager
{
    [SerializeField] MyButton nextButton;
    [SerializeField] MyButton giftButton;
    [SerializeField] RectTransform gems;
    [SerializeField] Image titleImage;
    [SerializeField] Text currencyCountText;
    [SerializeField] SkinProgress skinProgress;
    [SerializeField] RectTransform gemImageRt;
    [SerializeField] GemCollectAnimManager gemCollectAnimManager;
    int curencyCount;

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
        giftButton.Hide();
        nextButton.Hide();
        titleImage.gameObject.SetActive(true);
        gems.gameObject.SetActive(true);

        SoundManager.i.PlayOneShot(1);
        SaveData.i.lastClearedDisplayStageNum = StageTransManager.i.CurrentDisplayStageNum;
        FirebaseAnalyticsManager.i.LogEvent_StageClear(StageTransManager.i.CurrentDisplayStageNum);

        int baseClearReward = CSVManager.i.LevelRewardTable.ClampIndex(StageTransManager.i.CurrentDisplayStageNum - 1).clearReward;
        curencyCount = Mathf.RoundToInt(Variables.goalRate * baseClearReward);
        currencyCountText.text = "+" + curencyCount.ToString();

        SaveDataManager.i.Save();

        DOVirtual.DelayedCall(2.5f, () =>
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
                    SaveData.i.currencyCount += curencyCount;
                    SaveDataManager.i.Save();
                });
            });

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

        if (isNextGiftScreen)
        {
            giftButton.Show_ScaleAnim();
        }
        else
        {
            nextButton.Show_ScaleAnim();
        }
    }

    protected override void OnClose()
    {
        gameObject.SetActive(false);
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