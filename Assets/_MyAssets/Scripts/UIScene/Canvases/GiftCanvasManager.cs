using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using DG.Tweening;

public class GiftCanvasManager : BaseCanvasManager
{
    [SerializeField] GameObject chests;
    [SerializeField] Button rewardedVideoButton;
    [SerializeField] Button closeButton;
    [SerializeField] Image closeButtonImage;
    ChestView[] chestViews;
    Tween rewardedVideoButtonTween;

    public bool CanClickChest => ClickedChestCount < 3;
    public int ClickedChestCount { get; set; }
    Tween showCloseButtonTween;

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
        rewardedVideoButton.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
        if (showCloseButtonTween != null) showCloseButtonTween.Kill();
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
        if (rewardedVideoButtonTween != null) rewardedVideoButtonTween.Kill();
        rewardedVideoButton.gameObject.SetActive(true);
        rewardedVideoButton.transform.localScale = Vector3.zero;
        rewardedVideoButton.transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack)
        .OnComplete(() =>
        {
            rewardedVideoButtonTween = rewardedVideoButton.transform.DOScale(Vector3.one * 1.1f, 1f).SetEase(Ease.Flash, 2).SetLoops(-1);
        });


        showCloseButtonTween = DOVirtual.DelayedCall(1.0f, () =>
        {
            closeButton.gameObject.SetActive(true);
            Color color = closeButtonImage.color;
            color.a = 0;
            closeButtonImage.color = color;
            closeButtonImage.DOFade(1, 1.5f);
        });
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
