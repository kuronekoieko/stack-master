using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "MyGame/Create StagePrefabPathSO", fileName = "StagePrefabPathSO")]
public class StagePrefabPathSO : SingletonScriptableObject<StagePrefabPathSO>
{

    [ReadOnly]
    public string[] stagePrefabPaths_ver1;
    [ReadOnly]
    public string[] stagePrefabPaths_ver2;

    public string[] StagePrefabPaths => stagePrefabPaths_ver2;
}
