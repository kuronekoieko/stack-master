using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Linq;
[CreateAssetMenu(menuName = "MyGame/Create StageSettingsSO", fileName = "StageSettingsSO")]
public class StageSettingsSO : SingletonScriptableObject<StageSettingsSO>
{
    [ListDrawerSettings(ListElementLabelName = "stageNum")]
    [OnValueChanged(nameof(SetPath_ver1), true)]
    public StageData[] stageDatas;

    [ListDrawerSettings(ListElementLabelName = "stageNum_ver2")]
    [OnValueChanged(nameof(SetPath_ver2), true)]
    public StageData[] stageDatas_ver2;

    void SetPath_ver1()
    {
        StagePrefabPathSO.Instance.stagePrefabPaths_ver1 = stageDatas
            .Where(_ => _.stagePrefab)
            .Select(_ => "mStageVer1/" + _.stagePrefab.name)
            .ToArray();
    }

    void SetPath_ver2()
    {
        StagePrefabPathSO.Instance.stagePrefabPaths_ver2 = stageDatas_ver2
            .Where(_ => _.stagePrefab)
            .Select(_ => "mStageVer2/" + _.stagePrefab.name)
            .ToArray();
    }
}

/// <summary>
/// 【Unity】【Odin - Inspector and Serializer】クラスや構造体のパラメータを折りたたみ無しで表示する
/// https://baba-s.hatenablog.com/entry/2017/08/04/113000
/// </summary>
[Serializable, InlineProperty]
public class StageData
{
    string stageNum
    {
        get
        {
            if (Application.isEditor)
            {
                Debug.Log("テスト aaaaaaaaaaaaaaaaaaaaaaaaaaa");
                return "level " + (Array.IndexOf(StageSettingsSO.Instance.stageDatas, this) + 1);
            }
            else
            {
                Debug.Log("テスト iiiiiiiiiiiiiiiiiiiiiiiiiii");
                return "";
            }
        }
    }
    string stageNum_ver2 => !Application.isEditor ? "" : "level " + (Array.IndexOf(StageSettingsSO.Instance.stageDatas_ver2, this) + 1);
    [HideLabel]
    public GameObject stagePrefab;
}

