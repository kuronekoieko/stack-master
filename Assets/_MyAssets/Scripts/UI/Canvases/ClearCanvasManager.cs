using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public class ClearCanvasManager : BaseCanvasManager
{
    [SerializeField] Button nextButton;
    [SerializeField] Button retryButton;
    [SerializeField] Text titleText;
    [SerializeField] Image emojiImage;
    Sequence nextButtonSequence;
    Sequence retryButtonSequence;

    public override void OnStart()
    {
        base.SetScreenAction(thisScreen: ScreenState.Clear);

        nextButton.onClick.AddListener(OnClickNextButton);
        retryButton.onClick.AddListener(OnClickRetryButton);
        gameObject.SetActive(false);
    }

    public override void OnInitialize()
    {
    }

    public override void OnUpdate()
    {
        if (!base.IsThisScreen) { return; }

    }

    protected override void OnOpen()
    {
        UICameraController.i.PlayConfetti();
        bool isLastStage = Variables.currentSceneBuildIndex == Variables.lastSceneBuildIndex;
        nextButton.gameObject.SetActive(!isLastStage);
        retryButton.gameObject.SetActive(isLastStage);
        if (isLastStage) titleText.text = "COMPLETE!";

        DOVirtual.DelayedCall(1.0f, () =>
        {
            gameObject.SetActive(true);
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

            retryButton.transform.localScale = Vector3.one;
            retryButtonSequence = DOTween.Sequence()
            .Append(retryButton.transform.DOScale(Vector3.one * 1.1f, 0.5f))
            .Append(retryButton.transform.DOScale(Vector3.one, 0.5f));
            retryButtonSequence.SetLoops(-1);

            nextButton.transform.localScale = Vector3.one;
            nextButtonSequence = DOTween.Sequence()
            .Append(nextButton.transform.DOScale(Vector3.one * 1.1f, 0.5f))
            .Append(nextButton.transform.DOScale(Vector3.one, 0.5f));
            nextButtonSequence.SetLoops(-1);

            emojiImage.transform.eulerAngles = Vector3.forward * -40f;
            emojiImage.transform.DORotate(Vector3.forward * 40f, 1.5f).SetEase(Ease.InOutFlash, 2).SetLoops(-1);
            emojiImage.transform.localScale = Vector3.one;
            emojiImage.transform.DOScale(Vector3.one * 1.1f, 1.5f).SetEase(Ease.InOutFlash, 4).SetLoops(-1);
        });
    }

    protected override void OnClose()
    {
        gameObject.SetActive(false);
        nextButtonSequence.Kill();
        retryButtonSequence.Kill();
    }

    void OnClickNextButton()
    {
        base.ToNextScene();
        SoundManager.i.PlayOneShot(0);
    }

    void OnClickRetryButton()
    {
        base.ReLoadScene();
        SoundManager.i.PlayOneShot(0);
    }
    void OnClickHomeButton()
    {
        Variables.screenState = ScreenState.Home;
        SoundManager.i.PlayOneShot(0);
    }
}