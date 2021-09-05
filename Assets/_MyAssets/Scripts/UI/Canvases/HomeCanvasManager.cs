using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeCanvasManager : BaseCanvasManager
{
    [SerializeField] Button startButton;


    public override void OnStart()
    {
        base.SetScreenAction(thisScreen: ScreenState.Home);

        startButton.onClick.AddListener(OnClickStartButton);
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
        gameObject.SetActive(true);
    }

    protected override void OnClose()
    {
        gameObject.SetActive(false);
    }

    void OnClickStartButton()
    {
        Variables.screenState = ScreenState.Game;
    }
}
