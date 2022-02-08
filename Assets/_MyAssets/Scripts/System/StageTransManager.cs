using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using System;

public class StageTransManager
{
    public static StageTransManager i => _i;
    private static StageTransManager _i = new StageTransManager();
    public int Level { get; private set; } = 1;
    List<string> loopStageDatas = new List<string>();
    public GameObject stagePrefab => resourceRequest.asset as GameObject;
    public AsyncOperation asyncOperation;
    public ResourceRequest resourceRequest;


    public void SetInitialLevel(int num)
    {
        Level = num;
    }

    string GetCurrentDisplayStagePath(int displayStageNum)
    {
        if (loopStageDatas.Count < StagePrefabPathSO.Instance.StagePrefabPaths.Length)
        {
            loopStageDatas.AddRange(StagePrefabPathSO.Instance.StagePrefabPaths);
        }

        while (loopStageDatas.IsIndexOutOfRange(displayStageNum - 1))
        {
            loopStageDatas.AddRange(StagePrefabPathSO.Instance.StagePrefabPaths.Skip(1));
        }

        return loopStageDatas[displayStageNum - 1];
    }


    /// <summary>
    /// 次のステージに遷移する
    /// </summary>
    public void TranslateNextStage()
    {
        if (Variables.isShowInterstitialBeforeRestartLevel)
        {
            MaxSdkInterstitial.i.ShowOnClear(StageTransManager.i.Level, onHidden: () =>
            {
                Level++;
                ActivateLoadedStage();
            });
        }
        else
        {
            Level++;
            ActivateLoadedStage();
        }
    }

    public void TranslateSameStage()
    {
        ActivateLoadedStage();
    }


    /// <summary>
    /// 起動時と、シーンのアクティベート終了時にシーンをロードしておく
    /// </summary>
    public void LoadSceneAsync()
    {
        int sceneBuildIndex = 1;
        asyncOperation = SceneManager.LoadSceneAsync(sceneBuildIndex);
        asyncOperation.allowSceneActivation = false;
    }

    /// <summary>
    /// 起動時と、ステージのクリア時にロードしておく
    /// </summary>
    /// <param name="displayStageNum"></param>
    public void LoadStagePrefabAsync(int displayStageNum)
    {
        resourceRequest = Resources.LoadAsync<GameObject>(GetCurrentDisplayStagePath(displayStageNum));
    }

    void ActivateLoadedStage()
    {
        LoadingScreenController.i.Show(() =>
        {
            asyncOperation.allowSceneActivation = true;
            LoadSceneAsync();
        });
    }


    /*    /// <summary>
        /// デバッグ画面用に、ステージ名を一括取得する
        /// </summary>
        /// <value></value>
        public List<string> GetStageNames
        {
            get
            {
                List<string> numStrings = new List<string>();
                for (int i = 1; i < StageSettingsSO.Instance.StageDatas.Length + 1; i++)
                {
                    string name = Path.GetFileName(SceneUtility.GetScenePathByBuildIndex(i));
                    numStrings.Add((i) + "  " + name);
                }
                return numStrings;
            }
        }*/

}