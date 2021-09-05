using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.SceneManagement;

public enum ScreenState
{
    Game = 0,
    Clear = 1,
    Failed = 2,
    Login = 3,
    Home = 4,
    Start = 5,
}

public abstract class BaseCanvasManager : MonoBehaviour
{
    ScreenState thisScreen;

    /// <summary>
    /// OnOpenとOnCloseのイベント発火タイミングを設定する
    /// </summary>
    /// <param name="thisScreen">セットする画面のenumを入れてください</param>
    protected void SetScreenAction(ScreenState thisScreen)
    {
        this.thisScreen = thisScreen;

        this.ObserveEveryValueChanged(screenState => Variables.screenState)
            .Where(screenState => screenState == thisScreen)
            .Subscribe(screenState => OnOpen())
            .AddTo(this.gameObject);

        this.ObserveEveryValueChanged(screenState => Variables.screenState)
            .Buffer(2, 1)
            .Where(screenState => screenState[0] == thisScreen)
            .Where(screenState => screenState[1] != thisScreen)
            .Subscribe(screenState => OnClose())
            .AddTo(this.gameObject);
    }

    public abstract void OnStart();

    public abstract void OnUpdate();

    protected abstract void OnOpen();

    protected abstract void OnClose();

    public abstract void OnInitialize();

    protected void ToNextScene()
    {
        if (!IsThisScreen) { return; }
        Variables.currentSceneBuildIndex++;
        SceneManager.LoadScene(Variables.currentSceneBuildIndex);
    }

    protected void ReLoadScene()
    {
        if (!IsThisScreen) { return; }
        SceneManager.LoadScene(Variables.currentSceneBuildIndex);
    }

    protected bool IsThisScreen => Variables.screenState == thisScreen;

    /*
        public readonly ScreenState thisScreen = ScreenState.
        
        public override void OnStart()
        {
            base.SetScreenAction(thisScreen: thisScreen);
        }

        public override void OnUpdate(ScreenState currentScreen)
        {
            if (currentScreen != thisScreen) { return; }

        }

        protected override void OnOpen()
        {
            gameObject.SetActive(true);
        }

        protected override void OnClose()
        {
        }
    */

}
