using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class SkinProgress : MonoBehaviour
{
    [SerializeField] Transform models;
    [SerializeField] Transform modelsCenter;
    [SerializeField] Material outlineMaterial;
    [SerializeField] Material maskMaterial;
    [SerializeField] Transform rateMaskTf;
    [SerializeField] Text rateText;
    [SerializeField] Button skinGetButton;
    [SerializeField] Button continueButton;
    [SerializeField] Text titleText;
    SkinController outlineSkin;
    SkinController maskSkin;
    SkinController defaultSkin;

    public void OnStart()
    {
        rateMaskTf.gameObject.SetActive(true);
        skinGetButton.onClick.AddListener(OnClickSkinGetButton);
        continueButton.onClick.AddListener(OnClickContinueButton);
    }

    public void OnOpen()
    {
        SaveData.i.unlockingSkin.percentage = 0;

        rateText.gameObject.SetActive(true);
        skinGetButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);
        titleText.gameObject.SetActive(false);
        modelsCenter.transform.localScale = Vector3.one;

        outlineSkin = InstantiateSkin();
        outlineSkin.ChangeMaterial(outlineMaterial, true);
        outlineSkin.ChangeLayersForAllChildren("Skin");
        outlineSkin.RectTransform.anchoredPosition3D = Vector3.forward * 0f;

        maskSkin = InstantiateSkin();
        maskSkin.ChangeMaterial(maskMaterial, true);
        maskSkin.ChangeLayersForAllChildren("Skin");
        maskSkin.RectTransform.anchoredPosition3D = Vector3.forward * -1f;

        defaultSkin = InstantiateSkin();
        defaultSkin.ChangeMaterial(SkinSettingSO.i.characterMaterialDatas[0].material);
        defaultSkin.ChangeLayersForAllChildren("SkinProgress");
        defaultSkin.RectTransform.anchoredPosition3D = Vector3.forward * -2f;

        rateMaskTf.transform.localScale = new Vector3(1, 1f - (float)SaveData.i.unlockingSkin.percentage / 100f, 1);
        rateText.text = SaveData.i.unlockingSkin.percentage.ToString();
    }

    public void Anim(int startVal, int endVal, Action<bool> OnCompleteSkinProgress)
    {
        float duration = 2f;
        int nowNumber = startVal;

        Sequence sequence = DOTween.Sequence()
        .Append(rateMaskTf.transform.DOScaleY(1f - (float)endVal / 100f, duration).SetEase(Ease.Linear))
        .Join(DOTween.To(() => nowNumber, (n) => nowNumber = n, endVal, duration).OnUpdate(() => rateText.text = nowNumber.ToString()));

        bool isMax = endVal == 100;
        sequence.AppendCallback(() =>
        {
            OnCompleteSkinProgress(isMax);
        });

        if (!isMax) return;
        sequence
        .AppendCallback(() =>
        {
            rateText.gameObject.SetActive(false);
            titleText.gameObject.SetActive(true);
        })
        .Append(modelsCenter.transform.DOScale(1.5f, 0.5f).SetEase(Ease.OutBack))
        .AppendCallback(() =>
        {
            outlineSkin.Animator.SetBool("IsDance", true);
            maskSkin.Animator.SetBool("IsDance", true);
            defaultSkin.Animator.SetBool("IsDance", true);
            skinGetButton.gameObject.SetActive(true);
        })
        .AppendInterval(1.5f)
        .AppendCallback(() =>
        {
            continueButton.gameObject.SetActive(true);
            Color color = continueButton.image.color;
            color.a = 0;
            continueButton.image.color = color;
            continueButton.image.DOFade(1, 1.5f);
        });
    }

    SkinController InstantiateSkin()
    {
        SkinController skin = Instantiate(SkinSettingSO.i.characterSkinDatas[1].prefab, Vector3.zero, Quaternion.identity, models);
        skin.RectTransform = skin.gameObject.AddComponent<RectTransform>();
        skin.OnInstantiate();
        skin.Animator.applyRootMotion = false;
        skin.IsSetMaterial_Manual = true;
        skin.RectTransform.eulerAngles = Vector3.up * -158f;
        return skin;
    }

    void OnClickSkinGetButton()
    {
        SoundManager.i.PlayOneShot(0);
        Time.timeScale = 0;

        MaxSdkRewardedAds.i.ShowRewardedAd(
            onRewarded: () =>
            {
                Time.timeScale = 1;
                outlineSkin.Animator.SetTrigger("Cheer");
                maskSkin.Animator.SetTrigger("Cheer");
                defaultSkin.Animator.SetTrigger("Cheer");
                DOVirtual.DelayedCall(2f, () => OnClickContinueButton());
            },
            onNotRewarded: () =>
            {
                Time.timeScale = 1;
            }
        );
    }

    void OnClickContinueButton()
    {
        bool isNextGiftScreen = StageTransManager.i.CurrentDisplayStageNum % 5 == 0;
        if (isNextGiftScreen)
        {
            Variables.screenState = ScreenState.Gift;
        }
        else
        {
            StageTransManager.i.LoadNextStage();
        }
    }

    public void OnClose()
    {
        DestroyImmediate(outlineSkin.gameObject);
        DestroyImmediate(maskSkin.gameObject);
        DestroyImmediate(defaultSkin.gameObject);
    }
}

