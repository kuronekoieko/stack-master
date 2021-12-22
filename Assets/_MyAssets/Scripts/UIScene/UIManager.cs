using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 画面UIの一括管理
/// GameDirectorと各画面を中継する役割
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] Transform canvasesParentTf;
    [SerializeField] SplashController splashController;
    [SerializeField] StageSettingsSO stageSettingsSO;
    BaseCanvasManager[] baseCanvasManagers;
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        Variables.isLaunchUIScene = true;
        baseCanvasManagers = canvasesParentTf.GetComponentsInChildren<BaseCanvasManager>(true);
        SceneManager.sceneLoaded += SceneLoaded;
    }


    void Start()
    {
        StartCoroutine(LoadAsync());
    }


    private IEnumerator LoadAsync()
    {
        splashController.ShowSplash();

        while (!splashController.IsCompleteAnim)
        {
            yield return 0;
        }

        DontDestroyOnLoad(gameObject);
       
        CSVManager.i.ParseCSV();
        SaveDataManager.i.LoadSaveData();
        StartCanvases();

        FirebaseAnalyticsManager.i.Initialize();

        StageSettingsSO.i = stageSettingsSO;
        StageTransManager.i.LoadStageOnAppLaunch(startDisplayStageNum: SaveData.i.lastClearedDisplayStageNum + 1);
        AsyncOperation asyncOperation = StageTransManager.i.ReLoadStage();

        while (!asyncOperation.isDone)
        {
            yield return 0;
        }

        splashController.HideSplash();
    }

    void StartCanvases()
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