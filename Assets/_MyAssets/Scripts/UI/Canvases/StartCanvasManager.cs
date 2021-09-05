using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCanvasManager : BaseCanvasManager
{
    //[SerializeField] TutrialHandController tutrialHandController;
    //[SerializeField] RectTransform leftPosRT;
    //[SerializeField] RectTransform rightPosRT;
    public override void OnStart()
    {
        base.SetScreenAction(ScreenState.Start);
        //tutrialHandController.OnStart();
    }

    public override void OnInitialize()
    {
        gameObject.SetActive(true);
    }

    public override void OnUpdate()
    {
        if (!base.IsThisScreen) { return; }

        /*    if (Input.GetMouseButtonDown(0))
                {
                    Variables.screenState = ScreenState.Game;
                }*/

    }

    protected override void OnOpen()
    {
        gameObject.SetActive(true);
        //tutrialHandController.DragHorizontalAnim(leftPosRT.anchoredPosition, rightPosRT.anchoredPosition);
    }

    protected override void OnClose()
    {
        gameObject.SetActive(false);
        // tutrialHandController.Kill();
    }
}
