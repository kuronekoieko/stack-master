using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCanvasManager : BaseCanvasManager
{
    [SerializeField] TutrialController tutrialController;
    public override void OnStart()
    {
        base.SetScreenAction(thisScreen: ScreenState.Start);
    }

    protected override void OnOpen()
    {
        gameObject.SetActive(true);
        tutrialController.OnOpen();
    }

    public override void OnUpdate()
    {
        tutrialController.OnUpdate();
    }

    protected override void OnClose()
    {
        gameObject.SetActive(false);
    }

    public override void OnSceneLoaded()
    {

    }
}
