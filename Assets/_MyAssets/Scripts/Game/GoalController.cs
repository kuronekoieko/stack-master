using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GoalController : MonoBehaviour
{
    [SerializeField] SpriteRenderer goalSr;

    public void OnGoaled()
    {
        DOTween.ToAlpha(
            () => goalSr.color,
            color => goalSr.color = color,
            0f, // 目標値
            1f // 所要時間
        );
    }
}
