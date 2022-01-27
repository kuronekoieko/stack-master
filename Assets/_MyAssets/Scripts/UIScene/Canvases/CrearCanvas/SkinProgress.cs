using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Linq;
using UniRx;

public class SkinProgress : MonoBehaviour
{
    [SerializeField] Transform models;
    [SerializeField] Transform modelsCenter;
    [SerializeField] Material outlineMaterial;
    [SerializeField] Material maskMaterial;
    [SerializeField] Transform rateMaskTf;
    [SerializeField] Text rateText;
    [SerializeField] MyButton skinGetButton;
    [SerializeField] MyButton closeButton;
    [SerializeField] MyButton continueButton;
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
        closeButton.onClick.AddListener(OnClickCloseButton);
        continueButton.onClick.AddListener(OnClickContinueButton);
        this.ObserveEveryValueChanged(_ => MaxSdkRewardedAds.i.IsRewardedAdReady)
            .Subscribe(_ => OnChangedRewardedAdReady(_));
    }

    void OnChangedRewardedAdReady(bool isRewardedAdReady)
    {
        skinGetButton.interactable = isRewardedAdReady;
    }

    public void OnOpen()
    {
        gameObject.SetActive(false);
        SetPercentage();

        rateText.gameObject.SetActive(true);
        skinGetButton.Hide();
        closeButton.Hide();
        continueButton.Hide();
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
        rateText.text = SaveData.i.unlockingSkin.percentage + " %";
    }


    void SetPercentage()
    {
        // 初期状態 or no thanks
        if (SaveData.i.unlockingSkin.percentage == 0)
        {
            SetRomdomSkin();
            return;
        }

        // スキンゲットボタン or スキン購入画面で取得済みの場合
        if (SaveData.i.characterSkinSaveDatas[SaveData.i.unlockingSkin.index].isOwn)
        {
            SetRomdomSkin();
            return;
        }
    }

    void SetRomdomSkin()
    {
        SkinSaveData[] notOwns = SaveData.i.characterSkinSaveDatas.Where(_ => !_.isOwn).ToArray();

        if (notOwns.Length == 0)
        {
            isNotingSkin = true;
            return;
        }

        notOwns.GetRandom<SkinSaveData>(out int index);
        SaveData.i.unlockingSkin.index = index;
        SaveData.i.unlockingSkin.percentage = 0;
    }

    public void ProgressAnim()
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

        float duration = 1.5f;
        int nowNumber = startVal;

        Sequence sequence = DOTween.Sequence()
        .Append(rateMaskTf.transform.DOScaleY(1f - (float)endVal / 100f, duration).SetEase(Ease.Linear))
        .Join(
            DOTween.To(() => nowNumber, (n) => nowNumber = n, endVal, duration)
            .OnUpdate(() => rateText.text = nowNumber + " %")
            .SetEase(Ease.Linear)
            ).OnComplete(() =>
            {
                MaxAnim();
            });

    }

    public void MaxAnim()
    {
        if (!IsMax)
        {
            continueButton.Show_ScaleAnim();
            return;
        }
        Sequence sequence = DOTween.Sequence()
        .AppendCallback(() =>
        {
            rateText.gameObject.SetActive(false);
        })
        .Append(modelsCenter.transform.DOScale(1.5f, 0.5f).SetEase(Ease.OutBack))
        .AppendCallback(() =>
        {
            outlineSkin.Animator.SetTrigger("Dance");
            maskSkin.Animator.SetTrigger("Dance");
            defaultSkin.Animator.SetTrigger("Dance");
            skinGetButton.Show_ScaleAnim();
            closeButton.Show_FadeAnim(1.5f);
        });
    }

    SkinController InstantiateSkin()
    {
        SkinController skin = Instantiate(SkinSettingSO.i.CharacterSkinDatas[SaveData.i.unlockingSkin.index].prefab, Vector3.zero, Quaternion.identity, models);
        skin.RectTransform = skin.gameObject.AddComponent<RectTransform>();
        skin.OnInstantiate();
        skin.Animator.applyRootMotion = false;
        skin.IsSetMaterial_Manual = true;
        skin.RectTransform.eulerAngles = Vector3.up * -158f;
        return skin;
    }

    void OnClickSkinGetButton()
    {
        // SoundManager.i.PlayOneShot(0);
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
                skinGetButton.Hide();
                closeButton.Hide();
                FirebaseAnalyticsManager.i.LogEvent("skin_get_button", "skin_index_" + SaveData.i.unlockingSkin.index + "_skin_id_" + SkinSettingSO.i.CharacterSkinDatas[SaveData.i.unlockingSkin.index].id);
            },
            onNotRewarded: () =>
            {
                Time.timeScale = 1;
            }
        );
    }

    void OnClickContinueButton()
    {
        ToNext();
    }

    void OnClickCloseButton()
    {
        ToNext();
        SaveData.i.unlockingSkin.percentage = 0;
        FirebaseAnalyticsManager.i.LogEvent("skin_get_button_no_thanks", "skin_index_" + SaveData.i.unlockingSkin.index + "_skin_id_" + SkinSettingSO.i.CharacterSkinDatas[SaveData.i.unlockingSkin.index].id);
    }

    void ToNext()
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

