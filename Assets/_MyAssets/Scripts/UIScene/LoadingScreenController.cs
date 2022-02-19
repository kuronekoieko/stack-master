using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class LoadingScreenController : MonoBehaviour
{
    [SerializeField] Image bgImage;
    [SerializeField] Image loadingImage;
    Sequence sequence;
    public static LoadingScreenController i { get; private set; }
    float fadeDuration = 0.3f;

    public void OnAwake()
    {
        i = this;
        gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        loadingImage.transform.Rotate(Vector3.forward * 2f);
    }


    public void Show(Action OnComplete)
    {
        sequence = DOTween.Sequence()
        .AppendCallback(() =>
        {
            gameObject.SetActive(true);
            bgImage.SetAlpha(0);
        })
        .Append(DOTween.ToAlpha(() => bgImage.color, color => bgImage.color = color, 1f, fadeDuration).SetEase(Ease.InSine))
        .OnComplete(() =>
        {
            OnComplete();
        });
    }


    public void Hide()
    {
        sequence = DOTween.Sequence()
        .Append(DOTween.ToAlpha(() => bgImage.color, color => bgImage.color = color, 0f, fadeDuration).SetEase(Ease.OutSine))
        .OnComplete(() =>
        {
            gameObject.SetActive(false);
            MaxSdkBanner.i.Show();
        });
    }
}
