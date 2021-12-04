using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    void Awake()
    {
        int stageIndex = StageTransManager.i.CurrentStageNum - 1;
        var stagePrefab = StageSettingsSO.i.stagePrefabs[stageIndex];
        Instantiate(stagePrefab);
    }
}
