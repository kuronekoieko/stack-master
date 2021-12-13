using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// ゲーム画面
/// ゲーム中に表示するUIです
/// あくまで例として実装してあります
/// ボタンなどは適宜編集してください
/// </summary>
public class GameCanvasManager : BaseCanvasManager
{
    [SerializeField] Text levelNumText;
    [SerializeField] Button retryButton;

    public override void OnStart()
    {
        base.SetScreenAction(thisScreen: ScreenState.Game);

        this.ObserveEveryValueChanged(currentSceneBuildIndex => StageTransManager.i.CurrentDisplayStageNum)
            .Subscribe(currentSceneBuildIndex => { ShowStageNumText(levelNum: currentSceneBuildIndex); })
            .AddTo(this.gameObject);
        retryButton.onClick.AddListener(OnClickRetryButton);
        gameObject.SetActive(true);
    }

    public override void OnSceneLoaded()
    {
        FirebaseAnalyticsManager.i.LogScreen("level" + StageTransManager.i.CurrentDisplayStageNum);
    }

    public override void OnUpdate()
    {

    }

    protected override void OnOpen()
    {
        gameObject.SetActive(true);
    }

    protected override void OnClose()
    {
    }

    void ShowStageNumText(int levelNum)
    {
        levelNumText.text = "LEVEL " + levelNum;
    }

    void OnClickRetryButton()
    {
        StageTransManager.i.ReLoadStage();
        SoundManager.i.PlayOneShot(0);
    }
}
