using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StageManager : MonoBehaviour
{

    [Inject] MeshPerformanceController meshPerformanceController;
    public void OnAwake()
    {
        if (!Variables.isLaunchUIScene) return;
        // var stagePrefab = StageTransManager.i.GetCurrentDisplayStageData().stagePrefab;
        var stagePrefab = StageTransManager.i.stagePrefab;
        var stageGO = Instantiate(stagePrefab);

        meshPerformanceController.OnStart(stageGO);
    }
}
