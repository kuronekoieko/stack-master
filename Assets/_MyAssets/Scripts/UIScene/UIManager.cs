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
    ResourceRequest stageLoadingRR;
    AsyncOperation stageSceneAO;
    bool isCompleteOnStart;

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
        // Debug.Log("テスト main start() a");
        stageLoadingRR = Resources.LoadAsync<GameObject>("mStageVer2/m001_ver2");
        // Debug.Log("テスト main start() b");
        stageSceneAO = StageTransManager.i.ReLoadStage(true); //重い
                                                              // Debug.Log("テスト main start() c");
        StartCoroutine(Main());
        // StartCoroutine(UIInit());
    }


    private IEnumerator Main()
    {
        splashController.ShowSplash();
        // Debug.Log("テスト main");
        while (!splashController.IsCompleteAnim)
        {
            yield return null;
        }

        //  Debug.Log("テスト UIInit start");
        // yield return new WaitForSeconds(3f);
        scriptableObjectManager.SetInstance();
        SceneManager.sceneLoaded += SceneLoaded;
        CSVManager.i.ParseCSV();
        SaveDataManager.i.LoadSaveData();
        coinCountView.OnStart();
        SetPushNotification();
        StartCanvases();// 重い(1sくらい)

        FirebaseAnalyticsManager.i.Initialize();
        StageTransManager.i.LoadStageOnAppLaunch(startDisplayStageNum: SaveData.i.lastClearedDisplayStageNum + 1);
        // Debug.Log("テスト UIInit end");

        // Debug.Log("テスト プレハブロード 開始 " + Time.time);
        while (!stageLoadingRR.isDone)
        {
            yield return null;
        }
        var stagePrefab = stageLoadingRR.asset as GameObject;
        StageTransManager.i.stagePrefab = stagePrefab;
        // Debug.Log("テスト プレハブロード 開始 " + Time.time);
        // yield return new WaitForSeconds(3f);
        stageSceneAO.allowSceneActivation = true;
        // Debug.Log("テスト シーンロード 開始 " + Time.time);
        while (!stageSceneAO.isDone)
        {
            // Debug.Log("テスト シーンロード 中 " + stageSceneAO.progress);
            yield return null;
        }

        // Debug.Log("テスト シーンロード 終了 " + Time.time);

        yield return null;
        splashController.HideSplash();
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
        // Debug.Log(nextScene.name);
        foreach (var baseCanvasManager in baseCanvasManagers)
        {
            baseCanvasManager.OnSceneLoaded();
        }
        Variables.screenState = ScreenState.Start;

        // 同じフレームだと、シーン生成でカクつくため
        Observable.TimerFrame(1)
            .Subscribe(_ => LoadingScreenController.i.Hide());

        MaxSdkBanner.i.Show();
    }
}