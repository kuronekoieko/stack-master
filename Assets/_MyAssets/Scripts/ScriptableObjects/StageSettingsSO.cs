using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Linq;
[CreateAssetMenu(menuName = "MyGame/Create StageSettingsSO", fileName = "StageSettingsSO")]
public class StageSettingsSO : SingletonScriptableObject<StageSettingsSO>
{
    [OnValueChanged(nameof(SetPath_ver1), true)]
    public StageData[] stageDatas;

    [OnValueChanged(nameof(SetPath_ver2), true)]
    public StageData[] stageDatas_ver2;

    public void SetPath_ver1()
    {
        for (int i = 0; i < stageDatas.Length; i++)
        {
            stageDatas[i].level = "level " + (i + 1);
        }

        StagePrefabPathSO.Instance.stagePrefabPaths_ver1 = stageDatas
            .Where(_ => _.stagePrefab)
            .Select(_ => "mStageVer1/" + _.stagePrefab.name)
            .ToArray();
    }

    public void SetPath_ver2()
    {
        for (int i = 0; i < stageDatas_ver2.Length; i++)
        {
            stageDatas_ver2[i].level = "level " + (i + 1);
        }

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
    [HideInInspector]
    public string level;
    [LabelText("$level")]
    public GameObject stagePrefab;

}

