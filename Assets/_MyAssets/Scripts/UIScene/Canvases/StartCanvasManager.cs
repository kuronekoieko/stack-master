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
    [SerializeField] MyButton levelUpButton;
    [SerializeField] MyButton offlineIncomeButton;

    public override void OnStart()
    {
        base.SetScreenAction(thisScreen: ScreenState.Start);
        skinButton.onClick.AddListener(() => Variables.screenState = ScreenState.Skin);
    }

    protected override void OnOpen()
    {
        gameObject.SetActive(true);
        tutrialController.OnOpen();
        levelProgressionManager.OnOpen();
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
#if UNITY_EDITOR
        return EventSystem.current.IsPointerOverGameObject();
#else
        return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
#endif
    }
}
