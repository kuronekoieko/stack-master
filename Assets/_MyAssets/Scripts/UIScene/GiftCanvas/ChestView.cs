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
    [SerializeField] Button button;
    [SerializeField] Text gemCountText;
    [SerializeField] Image gemImage;
    [SerializeField] AnimationCurve chestOpeningScaleEasing;
    [SerializeField] GemImageAnim gemImageAnimPrefab;
    [SerializeField] RectTransform rectTransform;
    GemImageAnim[] gemImageAnims;

    public ChestState ChestState { get; set; }
    Sequence chestSeq;
    GiftCanvasManager giftCanvasManager;

    public void OnStart(GiftCanvasManager giftCanvasManager)
    {
        this.giftCanvasManager = giftCanvasManager;
        button.onClick.AddListener(OnClickChestButton);
        this.ObserveEveryValueChanged(_ => ChestState)
            .Subscribe(_ => OnChangeState())
            .AddTo(this.gameObject);

        gemImageAnims = new GemImageAnim[7];
        for (int i = 0; i < gemImageAnims.Length; i++)
        {
            gemImageAnims[i] = Instantiate(gemImageAnimPrefab, transform);
            gemImageAnims[i].OnInstansiate();
        }
    }

    public void OnScreenOpen()
    {
        ChestState = ChestState.Clickable;
    }

    void OnClickChestButton()
    {
        if (!giftCanvasManager.CanClickChest) return;
        ChestState = ChestState.Opening;
        giftCanvasManager.ClickedChestCount++;
    }

    void OnChangeState()
    {
        switch (ChestState)
        {
            case ChestState.Clickable:
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
                Vector3 startOffset = Vector3.zero;
                for (int i = 0; i < gemImageAnims.Length; i++)
                {
                    startOffset.x = Random.Range(-5f, 5f);
                    startOffset.y = Random.Range(-5f, 5f);
                    gemImageAnims[i].Anim(rectTransform.position + startOffset, CoinCountView.i.GemImagePos * 2f, 0);
                }
                break;
            default:
                break;
        }
    }

}
