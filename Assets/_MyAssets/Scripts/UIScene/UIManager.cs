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
    [SerializeField] SplashController splashController;
    [SerializeField] LoadingScreenController loadingScreenController;
    [SerializeField] CoinCountView coinCountView;
    [SerializeField] ScriptableObjectManager scriptableObjectManager;
    BaseCanvasManager[] baseCanvasManagers;
    // System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        Variables.isLaunchUIScene = true;
        baseCanvasManagers = canvasesParentTf.GetComponentsInChildren<BaseCanvasManager>(true);
        loadingScreenController.OnAwake();
        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
        StartCoroutine(Main());
    }


    private IEnumerator Main()
    {
        splashController.ShowSplash();

        while (!FirebaseRemoteConfigManager.i.IsFetchComplete)
        {
            yield return null;
        }

        SceneManager.sceneLoaded += SceneLoaded;
        CSVManager.i.ParseCSV();
        scriptableObjectManager.SetInstance();
        SaveDataManager.i.LoadSaveData();
        coinCountView.OnStart();
        SetPushNotification();
        FirebaseAnalyticsManager.i.Initialize();
        StageTransManager.i.SetInitialLevel(SaveData.i.lastClearedDisplayStageNum + 1);
        StageTransManager.i.LoadStagePrefabAsync(displayStageNum: StageTransManager.i.Level);
        StageTransManager.i.LoadSceneAsync();

        while (!splashController.IsCompleteAnim)
        {
            yield return null;
        }

        StartCanvases();// 重い(168msくらい)

        while (!StageTransManager.i.resourceRequest.isDone)
        {
            yield return null;
        }

        StageTransManager.i.asyncOperation.allowSceneActivation = true;

        while (!StageTransManager.i.asyncOperation.isDone)
        {

            yield return null;
        }

        yield return null;
        splashController.HideSplash();
        StageTransManager.i.LoadSceneAsync();
    }

    void SetPushNotification()
    {
        if (SaveData.i.isFirstLaunch) return;
        SaveData.i.isFirstLaunch = true;
        SaveDataManager.i.Save();
        // https://marumaro7.hatenablog.com/entry/localpush

        //　Androidチャンネルの登録
        //LocalPushNotification.RegisterChannel(引数1,引数２,引数３);
        //引数１ Androidで使用するチャンネルID なんでもいい LocalPushNotification.AddSchedule()で使用する
        //引数2　チャンネルの名前　なんでもいい　アプリ名でも入れておく
        //引数3　通知の説明 なんでもいい　自分がわかる用に書いておくもの

        // 説明を空文字にするとエラーで止まる
        LocalPushNotification.RegisterChannel("day_1", "LocalPush", "aaa");
        LocalPushNotification.RegisterChannel("day_3", "LocalPush", "aaa");
        LocalPushNotification.RegisterChannel("day_5", "LocalPush", "aaa");
        LocalPushNotification.RegisterChannel("day_7", "LocalPush", "aaa");

        //通知のクリア
        LocalPushNotification.AllClear();

        // プッシュ通知の登録
        //LocalPushNotification.AddSchedule(引数１,引数2,引数3,引数4,引数5);
        //引数１ プッシュ通知のタイトル
        //引数2　通知メッセージ
        //引数3　表示するバッジの数(バッジ数はiOSのみ適用の様子 Androidで数値を入れても問題無い)
        //引数4　何秒後に表示させるか？
        //引数5　Androidで使用するチャンネルID　「Androidチャンネルの登録」で登録したチャンネルIDと合わせておく
        //注意　iOSは45秒経過後からしかプッシュ通知が表示されない  
        int day = 60 * 60 * 24;
        LocalPushNotification.AddSchedule("Are you playing today😜?", "Play the game and build a tall tower now😂!", 1, day * 1, "day_1");
        LocalPushNotification.AddSchedule("Are you playing today😜?", "Play the game and build a tall tower now😂!", 1, day * 3, "day_3");
        LocalPushNotification.AddSchedule("Are you playing today😜?", "Play the game and build a tall tower now😂!", 1, day * 5, "day_5");
        LocalPushNotification.AddSchedule("Are you playing today😜?", "Play the game and build a tall tower now😂!", 1, day * 7, "day_7");
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
        //// Debug.Log(nextScene.name);
        foreach (var baseCanvasManager in baseCanvasManagers)
        {
            baseCanvasManager.OnSceneLoaded();
        }
        Variables.screenState = ScreenState.Start;

        // 同じフレームだと、シーン生成でカクつくため
        Observable.TimerFrame(1)
            .Subscribe(_ => LoadingScreenController.i.Hide());
    }
}