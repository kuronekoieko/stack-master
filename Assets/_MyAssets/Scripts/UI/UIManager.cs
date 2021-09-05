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
    }

    void Start()
    {
        SetCanvases();
        Variables.currentSceneBuildIndex++;
        SceneManager.LoadScene(Variables.currentSceneBuildIndex);
        // イベントにイベントハンドラーを追加
        SceneManager.sceneLoaded += SceneLoaded;
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
            baseCanvasManager.OnUpdate();
        }
    }

    // イベントハンドラー（イベント発生時に動かしたい処理）
    void SceneLoaded(Scene nextScene, LoadSceneMode mode)
    {
        foreach (var baseCanvasManager in baseCanvasManagers)
        {
            baseCanvasManager.OnInitialize();
        }
        Variables.screenState = initializeScreen;
    }
}