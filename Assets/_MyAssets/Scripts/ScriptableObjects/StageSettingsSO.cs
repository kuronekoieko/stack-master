using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Sirenix.OdinInspector;
using System;

[CreateAssetMenu(menuName = "MyGame/Create StageSettingsSO", fileName = "StageSettingsSO")]
public class StageSettingsSO : ScriptableObject
{
    public GameObject[] stagePrefabs;

    public static StageSettingsSO i;
}

[Serializable]
public class StageData
{
    string stageNum => "stage " + (Array.IndexOf(StageSettingsSO.i.stagePrefabs, this) + 1);
    // [LabelText("$stageNum")] public string data = "ここはゲームの設計によって任意に変更(csvなど)";
    public GameObject stagePrefab;
}

