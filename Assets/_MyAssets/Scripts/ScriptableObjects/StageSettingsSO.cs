using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Sirenix.OdinInspector;
using System;

[CreateAssetMenu(menuName = "MyGame/Create StageSettingsSO", fileName = "StageSettingsSO")]
public class StageSettingsSO : ScriptableObject
{
    public GameObject[] stagePrefabs;

    static StageSettingsSO _i;
    public static StageSettingsSO i
    {
        get
        {
            if (Variables.isLaunchUIScene) return _i;
            string PATH = "ScriptableObjects/" + nameof(StageSettingsSO);
            //初アクセス時にロードする
            if (_i == null)
            {
                _i = Resources.Load<StageSettingsSO>(PATH);

                //ロード出来なかった場合はエラーログを表示
                if (_i == null)
                {
                    Debug.LogError(PATH + " not found");
                }
            }

            return _i;
        }
        set { _i = value; }
    }

}

[Serializable]
public class StageData
{
    string stageNum => "stage " + (Array.IndexOf(StageSettingsSO.i.stagePrefabs, this) + 1);
    // [LabelText("$stageNum")] public string data = "ここはゲームの設計によって任意に変更(csvなど)";
    public GameObject stagePrefab;
}

