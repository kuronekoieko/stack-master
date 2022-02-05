using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class IncEffectController : MonoBehaviour
{
    [SerializeField] ParticleSystem bloodPs;
    [SerializeField] ParticleSystem hitPs;
    [SerializeField] SpriteRenderer inkSr;
    ParticleSystem[] bloodPsChildren;
    Vector3 inkScale;
    Tween bloodPsTween;
    Tween inkSrTween;
    ParticleSystem DeadPs => Variables.isSkinReal ? hitPs : bloodPs;

    public void OnInstantiate()
    {
        inkSr.gameObject.SetActive(false);
        DeadPs.gameObject.SetActive(false);
        if (Variables.isSkinReal) return;
        bloodPsChildren = DeadPs.GetComponentsInChildren<ParticleSystem>();
        inkScale = inkSr.transform.lossyScale;
        this.ObserveEveryValueChanged(_ => SaveData.i.selectedMaterialIndex)
            .Subscribe(_ => OnChangedMaterial(_));
    }

    void OnChangedMaterial(int selectedIndex)
    {
        if (Variables.isSkinReal) return;
        inkSr.material.color = ScriptableObjectManager.i.SkinSettingSO.characterMaterialDatas[selectedIndex].material.color;
        for (int i = 0; i < bloodPsChildren.Length; i++)
        {
            ParticleSystem.MainModule main = bloodPsChildren[i].main;
            main.startColor = inkSr.material.color;
        }
    }

    public void PlayBloodParticle(Vector3 characterPos, float characterHeight)
    {
        //if (Variables.isSkinReal) return;
        if (bloodPsTween != null) bloodPsTween.Kill();
        DeadPs.gameObject.SetActive(true);
        transform.parent = null;
        characterPos.y += characterHeight / 2f;
        DeadPs.transform.position = characterPos;
        DeadPs.Play();
        bloodPsTween = DOVirtual.DelayedCall(3, () =>
        {
            DeadPs.gameObject.SetActive(false);
        });
    }

    public void ShowInkSprite(Vector3 characterPos, float characterHeight, float characterRadius)
    {
        if (Variables.isSkinReal) return;
        if (inkSrTween != null) inkSrTween.Kill();

        inkSr.gameObject.SetActive(true);
        transform.parent = null;
        inkSr.transform.localScale = Vector3.zero;
        characterPos.y += characterHeight / 2f;
        characterPos.z += (characterRadius - 0.1f);
        inkSr.transform.position = characterPos;
        inkSr.transform.DOScale(inkScale, 0.5f);

        inkSrTween = DOVirtual.DelayedCall(3, () =>
        {
            inkSr.gameObject.SetActive(false);
        });
    }

}
