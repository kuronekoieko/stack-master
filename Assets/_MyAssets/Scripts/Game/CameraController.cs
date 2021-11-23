using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;

public enum CameraState
{
    Following,
    ClimbingStairs,
    Rotate,
}

public class CameraController : MonoBehaviour
{
    [Inject] CharacterManager characterManager;
    Vector3 offset;
    Vector3 currentVelocity;
    public CameraState CameraState { get; set; } = CameraState.Following;

    void Start()
    {
        offset = transform.position - characterManager.BottomCharacterPos;
    }

    void LateUpdate()
    {

        switch (CameraState)
        {
            case CameraState.Following: Follow(); break;
            case CameraState.ClimbingStairs: ClimbingStairs(); break;
            case CameraState.Rotate: Rotation(); break;
            default: break;
        }
    }

    void Follow()
    {
        if (characterManager.ActiveCount == 0) return;

        float distance = Vector3.Distance(offset, Vector3.zero);
        if (6 < characterManager.ActiveCount)
        {
            float rate = (float)characterManager.ActiveCount / 6f;

            distance = distance * Mathf.Clamp(rate, 1f, 3.5f);
            RenderSettings.fogStartDistance = Mathf.Lerp(RenderSettings.fogStartDistance, 100f + (float)characterManager.ActiveCount * 1.5f, 0.5f * Time.deltaTime);
            RenderSettings.fogEndDistance = Mathf.Lerp(RenderSettings.fogEndDistance, 300f + RenderSettings.fogStartDistance, 0.5f * Time.deltaTime);
        }
        Vector3 targetPos = characterManager.BottomCharacterPos;
        targetPos.x = 0;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos + offset.normalized * distance, ref currentVelocity, 0.2f);
    }

    void ClimbingStairs()
    {
        float distance = Vector3.Distance(offset, Vector3.zero);
        Vector3 targetPos = characterManager.BottomCharacterPos;
        targetPos.x = 0;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos + offset.normalized * distance, ref currentVelocity, 0.2f);
    }

    void Rotation()
    {
        transform.RotateAround(characterManager.BottomCharacterPos, Vector3.up, Time.deltaTime * 5f);
    }
}
