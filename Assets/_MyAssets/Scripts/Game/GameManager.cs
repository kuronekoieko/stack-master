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
    [Inject] AddCountTextEffectManager addCountTextEffectManager;


    void Awake()
    {

    }

    void Start()
    {
        StartCoroutine(LoadAsync());
    }

    private IEnumerator LoadAsync()
    {
        characterManager.OnAwake();
        yield return null;
        backgroundManager.OnAwake();
        yield return null;
        stageManager.OnAwake();
        yield return null;
        characterManager.OnStart();
        yield return null;
        backgroundManager.OnStart();
        yield return null;
        addCountTextEffectManager.OnStart();
    }

    void Update()
    {
        characterManager.OnUpdate();
    }

    void LateUpdate()
    {
        cameraController.OnLateUpdate();
    }
}
