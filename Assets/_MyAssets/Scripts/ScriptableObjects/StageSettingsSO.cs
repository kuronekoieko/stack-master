using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

[CreateAssetMenu(menuName = "MyGame/Create StageSettingsSO", fileName = "StageSettingsSO")]
public class StageSettingsSO : ScriptableObject
{
    public StageData[] stageDatas;

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
    string stageNum => "level " + (Array.IndexOf(StageSettingsSO.i.stageDatas, this) + 1);
    [LabelText("$stageNum")]
    public GameObject stagePrefab;
}

