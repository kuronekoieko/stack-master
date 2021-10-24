using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BackgroundManager : MonoBehaviour
{
    BackgroundController[] bgControllers;
    [SerializeField] int bgIndex;

    void OnValidate()
    {
        bgControllers = GetComponentsInChildren<BackgroundController>(true);
        if (bgIndex > bgControllers.Length - 1) return;
        foreach (var bg in bgControllers)
        {
            bg.gameObject.SetActive(false);
        }
        bgControllers[bgIndex].Activate();
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
        bgControllers[bgIndex].Activate();
    }
}
