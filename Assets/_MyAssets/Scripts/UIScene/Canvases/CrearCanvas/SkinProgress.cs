using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SkinProgress : MonoBehaviour
{
    [SerializeField] Transform models;
    [SerializeField] Material outlineMaterial;
    [SerializeField] Material maskMaterial;
    [SerializeField] Transform rateMaskTf;
    [SerializeField] Text rateText;
    public void OnStart()
    {
        rateMaskTf.gameObject.SetActive(true);
    }

    public void OnOpen()
    {
        SkinController outlineSkin = InstantiateSkin();
        outlineSkin.ChangeMaterial(outlineMaterial, true);
        outlineSkin.ChangeLayersForAllChildren("Skin");
        outlineSkin.RectTransform.anchoredPosition3D = Vector3.forward * 0f;

        SkinController maskSkin = InstantiateSkin();
        maskSkin.ChangeMaterial(maskMaterial, true);
        maskSkin.ChangeLayersForAllChildren("Skin");
        maskSkin.RectTransform.anchoredPosition3D = Vector3.forward * -1f;

        SkinController defaultSkin = InstantiateSkin();
        defaultSkin.ChangeMaterial(SkinSettingSO.i.characterMaterialDatas[0].material);
        defaultSkin.ChangeLayersForAllChildren("SkinProgress");
        defaultSkin.RectTransform.anchoredPosition3D = Vector3.forward * -2f;

        rateMaskTf.transform.localScale = new Vector3(1, 1f - (float)SaveData.i.unlockingSkin.percentage / 100f, 1);
        rateText.text = SaveData.i.unlockingSkin.percentage.ToString();
    }

    public void Anim(int startVal, int endVal)
    {
        float duration = 1f;
        rateMaskTf.transform.DOScaleY(1f - (float)endVal / 100f, duration).SetEase(Ease.Linear);
       
        int nowNumber = startVal;
        DOTween.To(() => nowNumber, (n) => nowNumber = n, endVal, duration)
            .OnUpdate(() => rateText.text = nowNumber.ToString());
    }

    SkinController InstantiateSkin()
    {
        SkinController skin = Instantiate(SkinSettingSO.i.characterSkinDatas[1].prefab, Vector3.zero, Quaternion.identity, models);
        skin.RectTransform = skin.gameObject.AddComponent<RectTransform>();
        skin.OnInstantiate();
        skin.Animator.applyRootMotion = false;
        skin.IsSetMaterial_Manual = true;
        skin.RectTransform.eulerAngles = Vector3.up * 158f;
        return skin;
    }
}

