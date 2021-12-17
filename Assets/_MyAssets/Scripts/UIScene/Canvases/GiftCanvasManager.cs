using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using DG.Tweening;

public class GiftCanvasManager : BaseCanvasManager
{
    [SerializeField] GameObject chests;
    [SerializeField] MyButton rewardedVideoButton;
    [SerializeField] MyButton closeButton;
    ChestView[] chestViews;

    public bool CanClickChest => ClickedChestCount < 3;
    public int ClickedChestCount { get; set; }

    public override void OnStart()
    {
        base.SetScreenAction(thisScreen: ScreenState.Gift);
        chestViews = chests.GetComponentsInChildren<ChestView>();
        foreach (var item in chestViews)
        {
            item.OnStart(this);
        }
        gameObject.SetActive(false);

        this.ObserveEveryValueChanged(_ => ClickedChestCount)
            .Subscribe(_ => OnValueChanged(clickedChestCount: _));

        this.ObserveEveryValueChanged(_ => MaxSdkRewardedAds.i.IsRewardedAdReady)
            .Subscribe(_ => OnChangedRewardedAdReady(_));

        rewardedVideoButton.onClick.AddListener(OnClickRewardedVideoButton);
        closeButton.onClick.AddListener(OnClickCloseButton);
    }

    protected override void OnOpen()
    {
        gameObject.SetActive(true);
        ClickedChestCount = 0;
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

    void OnValueChanged(int clickedChestCount)
    {
        if (clickedChestCount == 0)
        {
            InitializeChests();
            return;
        }
        if (CanClickChest) return;
        // ダイヤアニメーションの待機分の遅延
        DOVirtual.DelayedCall(3.0f, () =>
        {
            ShowRewardedVideoButtonAnim();
        });
    }

    void InitializeChests()
    {
        foreach (var item in chestViews)
        {
            item.OnScreenOpen();
        }
        rewardedVideoButton.Hide();
        closeButton.Hide();
    }

    void OnClickRewardedVideoButton()
    {
        SoundManager.i.PlayOneShot(0);
        Time.timeScale = 0;

        MaxSdkRewardedAds.i.ShowRewardedAd(
            onRewarded: () =>
            {
                Time.timeScale = 1;
                ClickedChestCount = 0;
            },
            onNotRewarded: () =>
            {
                Time.timeScale = 1;
            }
        );
    }

    void ShowRewardedVideoButtonAnim()
    {
        rewardedVideoButton.Show_ScaleAnim();
        closeButton.Show_FadeAnim(1.5f);
    }

    void OnClickCloseButton()
    {
        StageTransManager.i.LoadNextStage();
    }

    void OnChangedRewardedAdReady(bool isRewardedAdReady)
    {
        rewardedVideoButton.interactable = isRewardedAdReady;
    }
}
