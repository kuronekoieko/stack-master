using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftCanvasManager : BaseCanvasManager
{
    [SerializeField] GameObject chests;
    ChestView[] chestViews;

    public override void OnStart()
    {
        base.SetScreenAction(thisScreen: ScreenState.Gift);
        chestViews = chests.GetComponentsInChildren<ChestView>();
        foreach (var item in chestViews)
        {
            item.OnStart();
        }
        // gameObject.SetActive(false);
    }

    protected override void OnOpen()
    {
        gameObject.SetActive(true);
        foreach (var item in chestViews)
        {
            item.OnScreenOpen();
        }
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
}
