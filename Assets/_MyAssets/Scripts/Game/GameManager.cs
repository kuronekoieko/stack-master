using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Inject] CharacterManager characterManager;
    [Inject] CameraController cameraController;
    [Inject] BackgroundManager backgroundManager;
    [Inject] StageManager stageManager;
    [Inject] AddCountTextEffectManager addCountTextEffectManager;
    [Inject] MeshPerformanceController meshPerformanceController;

    void Awake()
    {
        
    }

    void Start()
    {
        if (!Variables.isLaunchUIScene)
        {
            ScriptableObjectManager.i.SetInstance();
        }

        // StartCoroutine(LoadAsync());
        characterManager.OnAwake();
        // yield return null;
        backgroundManager.OnAwake();
        // yield return null;
        stageManager.OnAwake();
        // yield return null;
        characterManager.OnStart();
        // yield return null;
        // backgroundManager.OnStart();
        // yield return null;
        addCountTextEffectManager.OnStart();
    }

    void Update()
    {
        characterManager.OnUpdate();
        meshPerformanceController.OnUpdate();
    }

    void LateUpdate()
    {
        cameraController.OnLateUpdate();
    }
}
