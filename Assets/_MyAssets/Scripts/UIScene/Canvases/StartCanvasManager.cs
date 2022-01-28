using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;
using System.Linq;

public class StartCanvasManager : BaseCanvasManager
{
    [SerializeField] TutrialController tutrialController;
    [SerializeField] LevelProgressionManager levelProgressionManager;
    [SerializeField] MyButton skinButton;
    [SerializeField] StartTowerButton startTowerButton;
    [SerializeField] OfflineIncomeButton offlineIncomeButton;
    [SerializeField] NoticeImageController skinButtonNotice;

    bool EnableUnlockRandom => SaveData.i.currencyCount >= Price;
    int Price
    {
        get
        {
            SkinPrice skinPrice = CSVManager.i.CharacterSkinPrices.ClampIndex<SkinPrice>(SaveData.i.characterSkinSaveDatas.Count(_ => _.isOwn) - 1);
            if (skinPrice == null) return 0;
            return skinPrice.price;
        }
    }

    public override void OnStart()
    {
        base.SetScreenAction(thisScreen: ScreenState.Start);
        skinButton.onClick.AddListener(() => Variables.screenState = ScreenState.Skin);
        offlineIncomeButton.OnStart();
        startTowerButton.OnStart();

        this.ObserveEveryValueChanged(_ => EnableUnlockRandom)
            .Subscribe(_ => skinButtonNotice.gameObject.SetActive(_));
    }

    protected override void OnOpen()
    {
        gameObject.SetActive(true);
        tutrialController.OnOpen();
        levelProgressionManager.OnOpen();
        startTowerButton.OnOpen();
        offlineIncomeButton.OnOpen();

        // チュートリアル（バッジが表示されるため）
        startTowerButton.gameObject.SetActive(StageTransManager.i.CurrentDisplayStageNum > 1);
        offlineIncomeButton.gameObject.SetActive(StageTransManager.i.CurrentDisplayStageNum > 1);
        skinButton.gameObject.SetActive(StageTransManager.i.CurrentDisplayStageNum > 1);
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
        if (tcount == 0) return false;
        for (int i = 0; i < tcount; i++)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
            {
                return true;
            }
        }
        return false;
    }
}
