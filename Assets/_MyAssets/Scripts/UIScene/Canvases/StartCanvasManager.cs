using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartCanvasManager : BaseCanvasManager
{
    [SerializeField] TutrialController tutrialController;
    [SerializeField] LevelProgressionManager levelProgressionManager;
    [SerializeField] MyButton skinButton;
    [SerializeField] StartTowerButton startTowerButton;
    [SerializeField] OfflineIncomeButton offlineIncomeButton;

    public override void OnStart()
    {
        base.SetScreenAction(thisScreen: ScreenState.Start);
        skinButton.onClick.AddListener(() => Variables.screenState = ScreenState.Skin);
        offlineIncomeButton.OnStart();
        startTowerButton.OnStart();
    }

    protected override void OnOpen()
    {
        gameObject.SetActive(true);
        tutrialController.OnOpen();
        levelProgressionManager.OnOpen();
        startTowerButton.OnOpen();
        offlineIncomeButton.OnOpen();
    }

    public override void OnUpdate()
    {
        if (IsTapUI()) return;
        if (Input.GetMouseButtonDown(0))
        {
            Variables.screenState = ScreenState.Game;
        }
    }

    protected override void OnClose()
    {
        tutrialController.OnClose();
        gameObject.SetActive(false);
    }

    public override void OnSceneLoaded()
    {

    }

    /// <summary>
    /// 【Unity】ボタンを押したときに画面クリックは無視する
    /// https://nn-hokuson.hatenablog.com/entry/2017/07/12/220302
    /// </summary>
    /// <returns></returns>
    bool IsTapUI()
    {
        if (Application.isEditor)
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        int tcount = Input.touchCount;
        if (tcount > 0)
        {
            for (int i = 0; i < tcount; i++)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
