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
    [SerializeField] MyButton rewardVideoButton;
    [SerializeField] RectTransform gems;
    [SerializeField] Text currencyCountText;
    [SerializeField] Text rewardVideoButtonCountText;
    [SerializeField] SkinProgress skinProgress;
    [SerializeField] RectTransform gemImageRt;
    [SerializeField] GemCollectAnimManager gemCollectAnimManager;
    [SerializeField] LevelProgressionManager levelProgressionManager;
    [SerializeField] GameObject clearGroup;
    int curencyCount;

    public override void OnStart()
    {
        base.SetScreenAction(thisScreen: ScreenState.Clear);

        nextButton.onClick.AddListener(() =>
        {
            FirebaseAnalyticsManager.i.LogEvent("clear_screen", "not_thanks_button");
            OnClickNextButton();
        });
        rewardVideoButton.onClick.AddListener(OnClickRewardVideoButton);
        gameObject.SetActive(false);
        skinProgress.OnStart();
        gemCollectAnimManager.OnStart(20);
        rewardVideoButton.Text = rewardVideoButtonCountText;
    }

    public override void OnSceneLoaded()
    {

    }

    public override void OnUpdate()
    {
    }

    protected override void OnOpen()
    {
        SoundManager.i.PlayOneShot(1);
        Open();

        /*        DOVirtual.DelayedCall(1.5f, () =>
        {
            Time.timeScale = 0;
            ShowInterstitial(() =>
            {
                Open();
                Time.timeScale = 1;
            });
        });*/


    }

    void ShowInterstitial(Action onHidden)
    {

        if (StageTransManager.i.CurrentDisplayStageNum == 1)
        {
            onHidden();
            return;
        }
        MaxSdkInterstitial.i.Show(onHidden);

    }

    void Open()
    {
        skinProgress.OnOpen();
        clearGroup.SetActive(true);
        levelProgressionManager.OnOpen();
        nextButton.Hide();
        rewardVideoButton.Hide();
        gems.gameObject.SetActive(true);


        SaveData.i.lastClearedDisplayStageNum = StageTransManager.i.CurrentDisplayStageNum;
        FirebaseAnalyticsManager.i.LogEvent_level("level_cleared");

        int baseClearReward = CSVManager.i.LevelRewardTable.ClampIndex(StageTransManager.i.CurrentDisplayStageNum - 1).clearReward;
        curencyCount = Mathf.RoundToInt(Variables.goalRate * baseClearReward);
        currencyCountText.text = "+" + curencyCount.ToString();
        rewardVideoButton.Text.text = "+" + (curencyCount * 2);

        SaveDataManager.i.Save();

        DOVirtual.DelayedCall(0.5f, () =>
        {
            gameObject.SetActive(true);
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                levelProgressionManager.Anim();
                gemCollectAnimManager.Anim(gemImageRt.position, 0.5f, () =>
                {
                    nextButton.Show_FadeTextAnim(1.5f);
                    rewardVideoButton.Show_ScaleAnim();
                    SaveData.i.currencyCount += curencyCount;
                    SaveDataManager.i.Save();
                });
            });
        });
    }

    void OpenSkinProgress()
    {
        clearGroup.SetActive(false);
        skinProgress.gameObject.SetActive(true);
        skinProgress.ProgressAnim();
    }

    protected override void OnClose()
    {
        gameObject.SetActive(false);
        skinProgress.OnClose();
    }

    void OnClickNextButton()
    {
        // SoundManager.i.PlayOneShot(0);
        OpenSkinProgress();
    }

    void OnClickRewardVideoButton()
    {
        Time.timeScale = 0;

        MaxSdkRewardedAds.i.ShowRewardedAd(
            onRewarded: () =>
            {
                Time.timeScale = 1;
                gemCollectAnimManager.Anim(gemImageRt.position, 0.5f, () =>
                {
                    SaveData.i.currencyCount += curencyCount * 2;
                    SaveDataManager.i.Save();
                    OnClickNextButton();
                });
                nextButton.Hide();
                rewardVideoButton.Hide();
                FirebaseAnalyticsManager.i.LogEvent("clear_screen", "reward_video_button");
            },
            onNotRewarded: () =>
            {
                Time.timeScale = 1;
            }
        );
    }
}