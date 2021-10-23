using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class StageTransManager
{
    public static StageTransManager i { get { return _i; } }
    private static StageTransManager _i = new StageTransManager();

    public int CurrentStageNum
    {
        set { currentStageNum = Mathf.Clamp(value, 1, lastStageNum); }
        get { return currentStageNum; }
    }
    private int currentStageNum = 1;
    private int lastStageNum;
    private bool isMultiScene;//シーン毎にステージを管理している場合true

    /// <summary>
    /// ステージ番号の初期化と、最初のロード
    /// </summary>
    /// <param name="isMultiScene"></param>
    /// <param name="startStageNum">アプリ起動時にどのステージから始めるか</param>
    /// <param name="lastStageNum">最後のステージ番号</param>
    public void LoadStageOnAppLaunch(bool isMultiScene, int startStageNum, int lastStageNum = 1)
    {
        this.isMultiScene = isMultiScene;
        this.lastStageNum = isMultiScene ? SceneManager.sceneCountInBuildSettings - 1 : lastStageNum;
        CurrentStageNum = startStageNum;
    }

    /// <summary>
    /// 次のステージに遷移する
    /// </summary>
    public void LoadNextStage()
    {
        CurrentStageNum++;
        ReLoadStage();
    }

    /// <summary>
    /// 現在のステージを再読み込みする
    /// </summary>
    public void ReLoadStage()
    {
        int sceneBuildIndex = isMultiScene ? CurrentStageNum : 1;
        SceneManager.LoadScene(sceneBuildIndex);
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
            for (int i = 1; i < lastStageNum + 1; i++)
            {
                string name = isMultiScene ? Path.GetFileName(SceneUtility.GetScenePathByBuildIndex(i)) : "";
                numStrings.Add((i) + "  " + name);
            }
            return numStrings;
        }
    }
}