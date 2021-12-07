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
    [SerializeField] ScreenState launchScreen;//起動時の画面
    [SerializeField] ScreenState initializeScreen;//初期化後に開く画面
    BaseCanvasManager[] baseCanvasManagers;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
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
            if (!baseCanvasManager.gameObject.activeSelf) continue;
            baseCanvasManager.OnUpdate();
        }
    }

    void SceneLoaded(Scene nextScene, LoadSceneMode mode)
    {
        foreach (var baseCanvasManager in baseCanvasManagers)
        {
            baseCanvasManager.OnSceneLoaded();
        }
        Variables.screenState = initializeScreen;
    }
}