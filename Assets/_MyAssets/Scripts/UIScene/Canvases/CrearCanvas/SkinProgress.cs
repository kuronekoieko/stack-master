using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Linq;

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
    [SerializeField] GameObject radial;
    SkinController outlineSkin;
    SkinController maskSkin;
    SkinController defaultSkin;
    bool isNotingSkin;
    public bool IsMax { get; private set; }

    public void OnStart()
    {
        rateMaskTf.gameObject.SetActive(true);
        skinGetButton.onClick.AddListener(OnClickSkinGetButton);
        continueButton.onClick.AddListener(OnClickContinueButton);
    }

    public void OnOpen()
    {
        SetPercentage();

        skinGetButton.enabled = true;
        continueButton.enabled = true;
        rateText.gameObject.SetActive(true);
        skinGetButton.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);
        titleText.gameObject.SetActive(false);
        modelsCenter.transform.localScale = Vector3.one;

        if (isNotingSkin)
        {
            rateText.gameObject.SetActive(false);
            radial.gameObject.SetActive(false);
            return;
        }

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


    void SetPercentage()
    {

        if (!SaveData.i.characterSkinSaveDatas[SaveData.i.unlockingSkin.index].isOwn) return;

        SkinSaveData skinSaveData = SaveData.i.characterSkinSaveDatas.Where(_ => !_.isOwn).FirstOrDefault();
        if (skinSaveData == null)
        {
            isNotingSkin = true;
            return;
        }

        SaveData.i.unlockingSkin.index = SaveData.i.characterSkinSaveDatas.IndexOf(skinSaveData);
        SaveData.i.unlockingSkin.percentage = 0;

    }

    public void Anim()
    {
        int randomInt = UnityEngine.Random.Range(25, 35);
        int startVal = SaveData.i.unlockingSkin.percentage;
        int endVal = Mathf.Clamp(SaveData.i.unlockingSkin.percentage + randomInt, 0, 100);
        SaveData.i.unlockingSkin.percentage = endVal < 100 ? endVal : 0;
        SaveDataManager.i.Save();
        IsMax = endVal == 100;

        if (isNotingSkin)
        {
            return;
        }

        float duration = 2f;
        int nowNumber = startVal;

        Sequence sequence = DOTween.Sequence()
        .Append(rateMaskTf.transform.DOScaleY(1f - (float)endVal / 100f, duration).SetEase(Ease.Linear))
        .Join(DOTween.To(() => nowNumber, (n) => nowNumber = n, endVal, duration).OnUpdate(() => rateText.text = nowNumber.ToString()));


        if (!IsMax) return;
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
        SkinController skin = Instantiate(SkinSettingSO.i.characterSkinDatas[SaveData.i.unlockingSkin.index].prefab, Vector3.zero, Quaternion.identity, models);
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
                SaveData.i.characterSkinSaveDatas[SaveData.i.unlockingSkin.index].isOwn = true;
                SaveData.i.selectedSkinIndex = SaveData.i.unlockingSkin.index;
                SaveDataManager.i.Save();
                skinGetButton.enabled = false;
                continueButton.enabled = false;
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
        if (outlineSkin == null) return;
        DestroyImmediate(outlineSkin.gameObject);
        DestroyImmediate(maskSkin.gameObject);
        DestroyImmediate(defaultSkin.gameObject);
    }
}

