using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[InitializeOnLoad]
public class StagePrefabPathsSetter
{
    static StagePrefabPathsSetter()
    {
        StageSettingsSO.Instance.SetPath_ver1();
        StageSettingsSO.Instance.SetPath_ver2();
    }
}
