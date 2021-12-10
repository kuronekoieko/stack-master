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

    public bool CanClickChest { get; set; }
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
            .Subscribe(_ => OnClickChestButton())
            .AddTo(this.gameObject);

        rewardedVideoButton.onClick.AddListener(OnClickRewardedVideoButton);
        closeButton.onClick.AddListener(OnClickCloseButton);
    }

    protected override void OnOpen()
    {
        gameObject.SetActive(true);
        foreach (var item in chestViews)
        {
            item.OnScreenOpen();
        }
        CanClickChest = true;
        ClickedChestCount = 0;
        rewardedVideoButton.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
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

    void OnClickChestButton()
    {
        if (ClickedChestCount == 0) return;
        CanClickChest = ClickedChestCount % 3 != 0;
        if (CanClickChest) return;
        DOVirtual.DelayedCall(3.0f, () =>
        {
            ShowRewardedVideoButtonAnim();
        });
    }

    void OnClickRewardedVideoButton()
    {
        CanClickChest = true;
        rewardedVideoButton.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
        showCloseButtonTween.Kill();
    }

    void ShowRewardedVideoButtonAnim()
    {
        if (ClickedChestCount == 9)
        {
            DOVirtual.DelayedCall(1.0f, () =>
            {
                StageTransManager.i.LoadNextStage();
            });
            return;
        }
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
}
