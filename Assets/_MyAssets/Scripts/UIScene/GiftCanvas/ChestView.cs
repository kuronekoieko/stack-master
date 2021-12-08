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

    public ChestState ChestState { get; set; }
    Sequence clickableSeq;

    public void OnStart()
    {
        button.onClick.AddListener(OnClickChestButton);
        this.ObserveEveryValueChanged(_ => ChestState)
            // .Where(_ => _ == thisScreen)
            .Subscribe(_ => OnChangeState())
            .AddTo(this.gameObject);
    }

    public void OnScreenOpen()
    {
        ChestState = ChestState.Clickable;
    }

    void OnClickChestButton()
    {
        ChestState = ChestState.Opening;
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
                clickableSeq = DOTween.Sequence()
                .Append(button.transform.DORotate(Vector3.forward * -10f, 2f).SetEase(Ease.InOutFlash, 2))
                .Join(button.transform.DOScaleY(1.2f, 2f).SetEase(Ease.InOutFlash, 2));
                clickableSeq.SetLoops(-1);



                break;
            case ChestState.NotClickable:
                button.interactable = false;
                break;
            case ChestState.Opening:
                button.interactable = false;
                clickableSeq.Kill();
                break;
            case ChestState.Opened:
                gemCountText.gameObject.SetActive(true);
                gemImage.gameObject.SetActive(true);
                button.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

}
