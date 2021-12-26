using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public void OnAwake()
    {
        if (!Variables.isLaunchUIScene) return;
        var stagePrefab = StageTransManager.i.GetCurrentDisplayStageData().stagePrefab;
        Instantiate(stagePrefab);
    }
}
