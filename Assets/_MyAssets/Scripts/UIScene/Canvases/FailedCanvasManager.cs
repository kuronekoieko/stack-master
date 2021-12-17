using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public class FailedCanvasManager : BaseCanvasManager
{
    [SerializeField] MyButton restartButton;
    [SerializeField] Image emojiImage;

    public override void OnStart()
    {
        base.SetScreenAction(thisScreen: ScreenState.Failed);
        restartButton.onClick.AddListener(OnClickRestartButton);
        gameObject.SetActive(false);
    }

    public override void OnSceneLoaded()
    {
    }

    public override void OnUpdate()
    {

    }

    protected override void OnOpen()
    {
        FirebaseAnalyticsManager.i.LogEvent_StageFailed(StageTransManager.i.CurrentDisplayStageNum);

        DOVirtual.DelayedCall(1.3f, () =>
        {
            gameObject.SetActive(true);
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

            restartButton.Show_ScaleAnim();

            emojiImage.transform.localScale = Vector3.one;
            emojiImage.transform.DOScale(Vector3.one * 1.1f, 1.5f).SetEase(Ease.InOutFlash, 4).SetLoops(-1);
        });
    }

    protected override void OnClose()
    {
        gameObject.SetActive(false);
    }

    void OnClickRestartButton()
    {
        StageTransManager.i.ReLoadStage();
        SoundManager.i.PlayOneShot(0);
    }

    void OnClickHomeButton()
    {
        // Variables.screenState = ScreenState.Home;
        SoundManager.i.PlayOneShot(0);
    }
}