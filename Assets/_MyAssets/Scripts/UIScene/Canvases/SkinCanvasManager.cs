using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class SkinCanvasManager : BaseCanvasManager
{
    [SerializeField] Button closeButton_arrow;
    [SerializeField] Button closeButton_x;
    [SerializeField] Button unlockButton;
    [SerializeField] Button rewardedButton;
    [SerializeField] SkinSelectButtonManager skinSelectButtonManager;
    int rewardedCurrencyCount = 500;


    public override void OnStart()
    {
        base.SetScreenAction(thisScreen: ScreenState.Skin);
        gameObject.SetActive(false);
        closeButton_arrow.onClick.AddListener(OnClickCloseButton);
        closeButton_x.onClick.AddListener(OnClickCloseButton);
        unlockButton.onClick.AddListener(OnClickUnlockButton);
        rewardedButton.onClick.AddListener(OnClickRewardedButton);
        this.ObserveEveryValueChanged(_ => MaxSdkRewardedAds.i.IsRewardedAdReady)
            .Subscribe(_ => OnChangedRewardedAdReady(_));

        this.ObserveEveryValueChanged(_ => skinSelectButtonManager.EnableUnlockRandom)
            .Subscribe(_ => unlockButton.interactable = _);

        skinSelectButtonManager.OnStart();
    }

    protected override void OnOpen()
    {
        gameObject.SetActive(true);
        skinSelectButtonManager.OnOpen();
    }

    public override void OnUpdate()
    {

    }

    protected override void OnClose()
    {
        gameObject.SetActive(false);
    }

    public override void OnSceneLoaded()
    {

    }

    void OnClickCloseButton()
    {
        //StageTransManager.i.ReLoadStage();
        Variables.screenState = ScreenState.Start;
    }

    void OnClickUnlockButton()
    {
        skinSelectButtonManager.UnlockRandom();
    }

    void OnClickRewardedButton()
    {
        SoundManager.i.PlayOneShot(0);
        Time.timeScale = 0;

        MaxSdkRewardedAds.i.ShowRewardedAd(
            onRewarded: () =>
            {
                Time.timeScale = 1;
                SaveData.i.currencyCount += rewardedCurrencyCount;
                SaveDataManager.i.Save();
            },
            onNotRewarded: () =>
            {
                Time.timeScale = 1;
            }
        );
    }

    void OnChangedRewardedAdReady(bool isRewardedAdReady)
    {
        rewardedButton.interactable = isRewardedAdReady;
    }
}
