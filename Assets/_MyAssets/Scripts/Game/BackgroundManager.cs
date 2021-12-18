using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    BackgroundController[] bgControllers;
    [SerializeField] int bgIndex;
    [SerializeField] Camera cam;
    [SerializeField] bool isDebug;
    int CurrentBGIndex => (StageTransManager.i.CurrentDisplayStageNum - 1) % bgControllers.Length;

    void OnValidate()
    {
        bgControllers = GetComponentsInChildren<BackgroundController>(true);
        if (bgIndex > bgControllers.Length - 1) return;
        foreach (var bg in bgControllers)
        {
            bg.gameObject.SetActive(false);
        }
        bgControllers[bgIndex].Activate(cam);
    }

    public void OnAwake()
    {
        bgControllers = GetComponentsInChildren<BackgroundController>(true);
    }

    public void OnStart()
    {
        foreach (var bg in bgControllers)
        {
            bg.gameObject.SetActive(false);
        }
        var index = isDebug ? bgIndex : CurrentBGIndex;
        bgControllers[index].Activate(cam);
    }
}
