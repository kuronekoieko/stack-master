using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BackgroundManager : MonoBehaviour
{
    BackgroundController[] bgControllers;
    [SerializeField] int bgIndex;
    [SerializeField] Camera cam;
    [SerializeField] bool isDebug;

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

    void Awake()
    {
        bgControllers = GetComponentsInChildren<BackgroundController>(true);
    }
    void Start()
    {
        foreach (var bg in bgControllers)
        {
            bg.gameObject.SetActive(false);
        }
        var index = isDebug ? bgIndex : Variables.bgIndex;
        bgControllers[index].Activate(cam);
        Variables.bgIndex++;
        if (Variables.bgIndex == bgControllers.Length) Variables.bgIndex = 0;
    }
}
