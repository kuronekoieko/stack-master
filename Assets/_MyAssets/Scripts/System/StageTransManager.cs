using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;

public class StageTransManager
{
    public static StageTransManager i => _i;
    private static StageTransManager _i = new StageTransManager();
    public int CurrentDisplayStageNum { get; private set; } = 1;
    List<StageData> loopStageDatas = new List<StageData>();

    /// <summary>
    /// ステージ番号の初期化と、最初のロード
    /// </summary>
    /// <param name="isMultiScene"></param>
    /// <param name="startStageNum">アプリ起動時にどのステージから始めるか</param>
    /// <param name="lastStageNum">最後のステージ番号</param>
    public void LoadStageOnAppLaunch(int startDisplayStageNum)
    {
        CurrentDisplayStageNum = startDisplayStageNum;
    }

    public StageData GetCurrentDisplayStageData()
    {
        int displaysStageNum = CurrentDisplayStageNum;
        if (loopStageDatas.Count < StageSettingsSO.i.stageDatas.Length)
        {
            loopStageDatas.AddRange(StageSettingsSO.i.stageDatas);
        }

        while (loopStageDatas.IsIndexOutOfRange(displaysStageNum - 1))
        {
            loopStageDatas.AddRange(StageSettingsSO.i.stageDatas.Skip(1));
        }

        return loopStageDatas[displaysStageNum - 1];
    }

    /// <summary>
    /// 次のステージに遷移する
    /// </summary>
    public void LoadNextStage()
    {
        CurrentDisplayStageNum++;
        ReLoadStage();
    }

    /// <summary>
    /// 現在のステージを再読み込みする
    /// </summary>
    public AsyncOperation ReLoadStage(bool isSplash = false)
    {
        int sceneBuildIndex = 1;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneBuildIndex);
        asyncOperation.allowSceneActivation = false;
        if (!isSplash) LoadingScreenController.i.Show(() => asyncOperation.allowSceneActivation = true);
        return asyncOperation;
    }

    /// <summary>
    /// デバッグ画面用に、ステージ名を一括取得する
    /// </summary>
    /// <value></value>
    public List<string> GetStageNames
    {
        get
        {
            List<string> numStrings = new List<string>();
            for (int i = 1; i < StageSettingsSO.i.stageDatas.Length + 1; i++)
            {
                string name = Path.GetFileName(SceneUtility.GetScenePathByBuildIndex(i));
                numStrings.Add((i) + "  " + name);
            }
            return numStrings;
        }
    }
}