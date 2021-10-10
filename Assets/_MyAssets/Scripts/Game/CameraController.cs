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

    void Start()
    {
        offset = transform.position - characterManager.BottomCharacterPos;
    }

    void LateUpdate()
    {
        Vector3 targetPos = characterManager.BottomCharacterPos;
        targetPos.x = 0;
        // transform.position = Vector3.Lerp(transform.position, targetPos + offset, 0.5f);
        transform.position = targetPos + offset;
    }
}
