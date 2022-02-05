using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;
using System;

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
    Vector3 angleCurrentVelocity;
    Vector3 camTarget;
    Vector3 startLookTargetOffset;
    public CameraState CameraState { get; set; } = CameraState.Following;


    public void SetOffset(Vector3 startPos)
    {
        offset = transform.position - startPos;


        startLookTargetOffset = GetStartLookTarget() - characterManager.BottomCharacterPos;
        transform.position = GetFollowPos();
        if (Variables.isZeroCameraXPos)
        {
            transform.SetPosX(0);
        }
        camTarget = GetLookTarget();
        transform.LookAt(camTarget);
    }


    Vector3 GetStartLookTarget()
    {
        // 平面を定義
        var plane = new Plane(Vector3.right, Vector3.zero);
        // レイを定義
        var ray = new Ray(transform.position, transform.forward);

        // レイと平面との当たり判定
        // ヒットした場合はenterに平面までの距離が格納される
        var isHit = plane.Raycast(ray, out var enter);

        if (isHit)
        {
            // ヒットした場合は平面の位置に点を移動
            return ray.GetPoint(enter);
        }
        return Vector3.zero;
    }

    public void OnLateUpdate()
    {
        switch (CameraState)
        {
            case CameraState.Following:
                if (Variables.isZeroCameraXPos)
                {
                    FollowFromBehind();
                }
                else
                {
                    Follow2();
                }
                break;
            case CameraState.ClimbingStairs: ClimbingStairs(); break;
            case CameraState.Rotate: ClimbingStairs(); break;
            default: break;
        }
    }

    void Follow()
    {
        if (characterManager.ActiveCount == 0) return;

        // float distance = Vector3.Distance(offset, Vector3.zero);
        Vector3 followOffset = offset;

        // if (6 < characterManager.ActiveCount)
        // {

        // float rate = Mathf.Clamp(1 / (float)characterManager.ActiveCount * 2f, 1f, 3.5f);
        //distance = distance * Mathf.Clamp(rate, 1f, 3.5f);
        followOffset.x *= Mathf.Clamp((float)(characterManager.ActiveCount ^ 2) / 10f, 1f, 3f);
        followOffset.y *= Mathf.Clamp((float)(characterManager.ActiveCount ^ 2) / 5f, 1f, 3f * 2f);
        followOffset.z *= Mathf.Clamp((float)(characterManager.ActiveCount ^ 2) / 10f, 1f, 3.5f);
        //followOffset += followOffset * rate;

        RenderSettings.fogStartDistance = Mathf.Lerp(RenderSettings.fogStartDistance, 100f + (float)characterManager.ActiveCount * 1.5f, 0.5f * Time.deltaTime);
        RenderSettings.fogEndDistance = Mathf.Lerp(RenderSettings.fogEndDistance, 300f + RenderSettings.fogStartDistance, 0.5f * Time.deltaTime);
        // }

        Vector3 bottomPos = characterManager.BottomCharacterPos;
        bottomPos.x = 0;
        transform.position = Vector3.SmoothDamp(transform.position, bottomPos + followOffset, ref currentVelocity, 0.2f);

        float maxCount = 5 ^ 2;
        float currentCount = (float)(characterManager.ActiveCount ^ 2);
        float towerCenterHeight = Mathf.Clamp(currentCount, 0f, maxCount) * characterManager.characterHeight / 2f;


        camTarget = Vector3.SmoothDamp(camTarget, bottomPos + Vector3.up * towerCenterHeight, ref angleCurrentVelocity, 0.2f);
        camTarget.z = bottomPos.z;
        transform.LookAt(camTarget);
    }


    void Follow1()
    {
        if (characterManager.ActiveCount == 0) return;

        // float distance = Vector3.Distance(offset, Vector3.zero);
        Vector3 followOffset = offset;

        // if (6 < characterManager.ActiveCount)
        // {

        // float rate = Mathf.Clamp(1 / (float)characterManager.ActiveCount * 2f, 1f, 3.5f);
        //distance = distance * Mathf.Clamp(rate, 1f, 3.5f);
        followOffset.x *= Mathf.Clamp(Mathf.Pow(characterManager.ActiveCount, 1.8f) / 100f, 1f, 3f);
        followOffset.y *= Mathf.Clamp(Mathf.Pow(characterManager.ActiveCount, 2) / 100f, 1f, 3f * 1.5f);
        followOffset.z *= Mathf.Clamp(Mathf.Pow(characterManager.ActiveCount, 1.8f) / 100f, 1f, 3.5f);
        //followOffset += followOffset * rate;

        RenderSettings.fogStartDistance = Mathf.Lerp(RenderSettings.fogStartDistance, 100f + (float)characterManager.ActiveCount * 1.5f, 0.5f * Time.deltaTime);
        RenderSettings.fogEndDistance = Mathf.Lerp(RenderSettings.fogEndDistance, 300f + RenderSettings.fogStartDistance, 0.5f * Time.deltaTime);
        // }

        Vector3 bottomPos = characterManager.BottomCharacterPos;
        bottomPos.x = 0;
        transform.position = Vector3.SmoothDamp(transform.position, bottomPos + followOffset, ref currentVelocity, 0.2f);


        float maxCount = Mathf.Pow(5, 2);
        float currentCount = Mathf.Pow(characterManager.ActiveCount, 2);
        float towerCenterHeight = Mathf.Clamp(currentCount, 0f, maxCount) * characterManager.characterHeight / 2f / 5f;


        camTarget = Vector3.SmoothDamp(camTarget, bottomPos + Vector3.up * towerCenterHeight, ref angleCurrentVelocity, 0.2f);
        camTarget.z = bottomPos.z;
        transform.LookAt(camTarget);
    }

    void Follow2()
    {
        if (characterManager.ActiveCount == 0) return;
        transform.position = Vector3.SmoothDamp(transform.position, GetFollowPos(), ref currentVelocity, 0.4f);
        camTarget = Vector3.SmoothDamp(camTarget, GetLookTarget(), ref angleCurrentVelocity, 0.4f);
        transform.LookAt(camTarget);
    }

    void FollowFromBehind()
    {
        if (characterManager.ActiveCount == 0) return;
        transform.position = Vector3.SmoothDamp(transform.position, GetFollowPos(), ref currentVelocity, 0.4f);
        transform.SetPosX(0);
        camTarget = Vector3.SmoothDamp(camTarget, GetLookTarget(), ref angleCurrentVelocity, 0.4f);
        transform.LookAt(camTarget);
    }

    Vector3 GetFollowPos()
    {
        Vector3 followOffset = offset;

        float max = 10;
        followOffset.x *= Mathf.Clamp(characterManager.ActiveCount * 0.15f, 1f, max * 0.15f);
        followOffset.y *= Mathf.Clamp(characterManager.ActiveCount * 0.3f, 1f, (max + 5) * 0.3f);
        followOffset.z *= Mathf.Clamp(characterManager.ActiveCount * 0.15f, 1f, max * 0.15f);

        // RenderSettings.fogStartDistance = Mathf.Lerp(RenderSettings.fogStartDistance, 100f + (float)characterManager.ActiveCount * 1.5f, 0.5f * Time.deltaTime);
        // RenderSettings.fogEndDistance = Mathf.Lerp(RenderSettings.fogEndDistance, 300f + RenderSettings.fogStartDistance, 0.5f * Time.deltaTime);

        Vector3 bottomPos = characterManager.BottomCharacterPos;
        bottomPos.x = 0;

        return bottomPos + followOffset;
    }


    Vector3 GetLookTarget()
    {
        float maxCount = 15;
        float currentCount = characterManager.ActiveCount;
        float towerCenterHeight = Mathf.Clamp(currentCount, 1f, maxCount) * characterManager.characterHeight / 2f;
        var target = characterManager.BottomCharacterPos + startLookTargetOffset.normalized * towerCenterHeight;
        target.x = 0;
        return target;
    }



    void ClimbingStairs()
    {
        Vector3 targetPos = characterManager.BottomCharacterPos;
        targetPos.x = 0;
        Vector3 stairOffset = offset;
        //stairOffset.y *= 1.5f;
        // stairOffset.z *= 1.5f;
        // transform.position = targetPos + stairOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos + stairOffset, ref currentVelocity, 0.4f);
        camTarget = Vector3.SmoothDamp(camTarget, targetPos, ref angleCurrentVelocity, 0.4f);
        transform.LookAt(camTarget);
    }

    void Rotation()
    {
        //  transform.RotateAround(characterManager.BottomCharacterPos, Vector3.up, Time.deltaTime * 5f);
    }
}
