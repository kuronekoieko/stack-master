using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

/// <summary>
/// 画面UIの一括管理
/// GameDirectorと各画面を中継する役割
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] Transform canvasesParentTf;
    [SerializeField] bool isSkipSplash;
    BaseCanvasManager[] baseCanvasManagers;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        DontDestroyOnLoad(gameObject);
        baseCanvasManagers = canvasesParentTf.GetComponentsInChildren<BaseCanvasManager>(true);
        FirebaseAnalyticsManager.i.Initialize();
        Variables.isLaunchUIScene = true;
    }

    void Start()
    {
        SaveDataManager.i.LoadSaveData();
        StageTransManager.i.LoadStageOnAppLaunch(startDisplayStageNum: SaveData.i.lastClearedDisplayStageNum + 1);
        SetCanvases();
        // イベントにイベントハンドラーを追加
        SceneManager.sceneLoaded += SceneLoaded;

        if (!Application.isEditor)
        {
            Variables.screenState = ScreenState.Splash;
            return;
        }

        if (!isSkipSplash)
        {
            Variables.screenState = ScreenState.Splash;
            return;
        }
        StageTransManager.i.ReLoadStage();

    }

    void SetCanvases()
    {
        foreach (var baseCanvasManager in baseCanvasManagers)
        {
            baseCanvasManager.OnStart();
        }
    }

    void Update()
    {
        foreach (var baseCanvasManager in baseCanvasManagers)
        {
            if (!baseCanvasManager.IsThisScreen) continue;
            baseCanvasManager.OnUpdate();
        }
    }

    void SceneLoaded(Scene nextScene, LoadSceneMode mode)
    {
        foreach (var baseCanvasManager in baseCanvasManagers)
        {
            baseCanvasManager.OnSceneLoaded();
        }
        Variables.screenState = ScreenState.Start;
    }
}