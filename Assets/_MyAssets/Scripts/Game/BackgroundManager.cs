using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BackgroundManager : MonoBehaviour
{
    BackgroundController[] bgControllers;

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
        bgControllers[1].Activate();
    }
}
