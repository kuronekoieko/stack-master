using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GoalController : MonoBehaviour
{
    [SerializeField] SpriteRenderer goalSr;


    void Awake()
    {
        Material material = new Material(goalSr.material);
        goalSr.material = material;
    }
    public void OnGoaled()
    {
        DOTween.ToAlpha(
            () => goalSr.material.color,
            color => goalSr.material.color = color,
            0f, // 目標値
            1f // 所要時間
        );
    }
}
