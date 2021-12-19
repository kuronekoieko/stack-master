using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject] CharacterManager characterManager;
    [Inject] CameraController cameraController;
    [Inject] BackgroundManager backgroundManager;
    [Inject] StageManager stageManager;

    void Awake()
    {
        characterManager.OnAwake();
        backgroundManager.OnAwake();
        stageManager.OnAwake();
    }

    void Start()
    {
        characterManager.OnStart();
        backgroundManager.OnStart();
    }


    void Update()
    {
        characterManager.OnUpdate();
    }

    void LateUpdate()
    {
        cameraController.OnLateUpdate();
    }

    void FixedUpdate()
    {

    }
}
