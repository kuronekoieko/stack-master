using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public void OnAwake()
    {
        if (!Variables.isLaunchUIScene) return;
        int stageIndex = StageTransManager.i.CurrentStageNum - 1;
        var stagePrefab = StageSettingsSO.i.stagePrefabs[stageIndex];
        Instantiate(stagePrefab);
    }
}
