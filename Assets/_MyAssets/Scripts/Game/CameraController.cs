using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;

/// <summary>
/// Unityで解像度に合わせて画面のサイズを自動調整する
/// http://www.project-unknown.jp/entry/2017/01/05/212837
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] Transform startTarget;
    [Inject] CharacterManager characterManager;
    Vector3 offset;
    Vector3 currentVelocity;
    void Start()
    {
        offset = transform.position - characterManager.BottomCharacterPos;
    }

    void LateUpdate()
    {
        if (characterManager.ActiveCount == 0) return;
        float distance = Vector3.Distance(offset, Vector3.zero);
        if (characterManager.ActiveCount > 6)
        {
            distance = distance * (float)characterManager.ActiveCount / 6f;
            RenderSettings.fogStartDistance = (float)characterManager.ActiveCount * 1.5f;
            RenderSettings.fogEndDistance = 300f + RenderSettings.fogStartDistance;
        }
        Vector3 targetPos = characterManager.BottomCharacterPos;
        targetPos.x = 0;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos + offset.normalized * distance, ref currentVelocity, 0.2f);
    }
}
