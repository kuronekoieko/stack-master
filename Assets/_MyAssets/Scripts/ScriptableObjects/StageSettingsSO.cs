using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

[CreateAssetMenu(menuName = "MyGame/Create StageSettingsSO", fileName = "StageSettingsSO")]
public class StageSettingsSO : ScriptableObject
{
    [ListDrawerSettings(ListElementLabelName = "stageNum")]
    public StageData[] stageDatas;

    [ListDrawerSettings(ListElementLabelName = "stageNum_ver2")]
    public StageData[] stageDatas_ver2;
    public StageData[] StageDatas => stageDatas_ver2;


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

/// <summary>
/// 【Unity】【Odin - Inspector and Serializer】クラスや構造体のパラメータを折りたたみ無しで表示する
/// https://baba-s.hatenablog.com/entry/2017/08/04/113000
/// </summary>
[Serializable, InlineProperty]
public class StageData
{
    string stageNum => "level " + (Array.IndexOf(StageSettingsSO.i.stageDatas, this) + 1);
    string stageNum_ver2 => "level " + (Array.IndexOf(StageSettingsSO.i.stageDatas_ver2, this) + 1);
    [HideLabel]
    public GameObject stagePrefab;
}

