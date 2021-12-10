using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinCanvasManager : BaseCanvasManager
{
    [SerializeField] Button closeButton_arrow;
    [SerializeField] Button closeButton_x;
    [SerializeField] Button unlockButton;
    [SerializeField] Button rewardedButton;

    public override void OnStart()
    {
        base.SetScreenAction(thisScreen: ScreenState.Skin);
        gameObject.SetActive(false);
        closeButton_arrow.onClick.AddListener(OnClickCloseButton);
        closeButton_x.onClick.AddListener(OnClickCloseButton);
        unlockButton.onClick.AddListener(OnClickUnlockButton);
        rewardedButton.onClick.AddListener(OnClickRewardedButton);
    }

    protected override void OnOpen()
    {
        gameObject.SetActive(true);
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

    }

    void OnClickRewardedButton()
    {

    }
}
