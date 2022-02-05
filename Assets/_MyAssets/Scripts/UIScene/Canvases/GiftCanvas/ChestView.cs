using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public enum ChestState
{
    Clickable,
    NotClickable,
    Opening,
    Opened,
}

public class ChestView : MonoBehaviour
{
    [SerializeField] MyButton button;
    [SerializeField] Text gemCountText;
    [SerializeField] Image gemImage;
    [SerializeField] AnimationCurve chestOpeningScaleEasing;
    [SerializeField] GemCollectAnimManager gemCollectAnimManager;
    [SerializeField] RectTransform rectTransform;
    public ChestState ChestState { get; set; }
    Sequence chestSeq;
    GiftCanvasManager giftCanvasManager;
    int gemCount;

    public void OnStart(GiftCanvasManager giftCanvasManager)
    {
        this.giftCanvasManager = giftCanvasManager;

        button.onClick.AddListener(OnClickChestButton);
        this.ObserveEveryValueChanged(_ => ChestState)
            .Subscribe(_ => OnChangeState());

        gemCollectAnimManager.OnStart(7);
    }

    public void OnScreenOpen()
    {
        Initialize();
    }

    void OnClickChestButton()
    {
        if (!giftCanvasManager.CanClickChest) return;
        ChestState = ChestState.Opening;
        giftCanvasManager.ClickedChestCount++;
    }


    void Initialize()
    {
        ChestState = ChestState.Clickable;
        if (chestSeq != null) chestSeq.Kill();

        gemCountText.gameObject.SetActive(false);
        gemImage.gameObject.SetActive(false);
        button.gameObject.SetActive(true);
        button.interactable = true;

        button.transform.localScale = Vector3.one;
        button.transform.eulerAngles = Vector3.zero;

        chestSeq = DOTween.Sequence()
        .Append(button.transform.DORotate(Vector3.forward * -10f, 2f).SetEase(Ease.InOutFlash, 2))
        .Join(button.transform.DOScaleY(1.2f, 2f).SetEase(Ease.InOutFlash, 2));
        chestSeq.SetLoops(-1);

        gemCount = GetGemCount();
    }

    int GetGemCount()
    {
        LevelReward levelReward = CSVManager.i.LevelRewardTable.ClampIndex(StageTransManager.i.Level - 1);
        int baseReward = levelReward != null ? levelReward.chestsReward : 0;

        int randomInt = Random.Range(0, 100);
        if (randomInt <= 10) return baseReward * 4;
        if (randomInt <= 30) return baseReward * 3;
        if (randomInt <= 60) return baseReward * 2;
        return baseReward * 1;
    }

    void OnChangeState()
    {
        switch (ChestState)
        {
            case ChestState.Clickable:
                break;
            case ChestState.NotClickable:
                button.interactable = false;
                break;
            case ChestState.Opening:
                button.interactable = false;
                chestSeq.Kill();
                button.transform.DORotate(Vector3.zero, 0.5f);
                button.transform.DOScaleY(0.7f, 1.5f).SetEase(chestOpeningScaleEasing);
                // https://shibuya24.info/entry/2016/10/21/220000
                button.transform.DOShakePosition(1.5f, 5f, 20, 1, false, false)
                .OnComplete(() =>
                {
                    ChestState = ChestState.Opened;
                });
                break;
            case ChestState.Opened:
                gemCountText.gameObject.SetActive(true);
                gemImage.gameObject.SetActive(true);
                button.gameObject.SetActive(false);
                button.interactable = true;

                gemCollectAnimManager.Anim(rectTransform.position, 0.5f, () =>
                {
                    SaveData.i.currencyCount += gemCount;
                    SaveDataManager.i.Save();
                });

                gemCountText.text = gemCount.ToString();

                break;
            default:
                break;
        }
    }

}
