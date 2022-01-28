using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class IncEffectController : MonoBehaviour
{
    [SerializeField] ParticleSystem bloodPs;
    [SerializeField] SpriteRenderer inkSr;
    ParticleSystem[] bloodPsChildren;
    Vector3 inkScale;
    Tween bloodPsTween;
    Tween inkSrTween;

    public void OnInstantiate()
    {
        inkSr.gameObject.SetActive(false);
        bloodPs.gameObject.SetActive(false);
        if (Variables.isSkinReal) return;
        bloodPsChildren = bloodPs.GetComponentsInChildren<ParticleSystem>();
        inkScale = inkSr.transform.lossyScale;
        this.ObserveEveryValueChanged(_ => SaveData.i.selectedMaterialIndex)
            .Subscribe(_ => OnChangedMaterial(_));
    }

    void OnChangedMaterial(int selectedIndex)
    {
        if (Variables.isSkinReal) return;
        inkSr.material.color = SkinSettingSO.i.characterMaterialDatas[selectedIndex].material.color;
        for (int i = 0; i < bloodPsChildren.Length; i++)
        {
            ParticleSystem.MainModule main = bloodPsChildren[i].main;
            main.startColor = inkSr.material.color;
        }
    }

    public void PlayBloodParticle(Vector3 hitPos, float characterHeight)
    {
        if (Variables.isSkinReal) return;
        if (bloodPsTween != null) bloodPsTween.Kill();
        bloodPs.gameObject.SetActive(true);
        transform.parent = null;
        var pos = hitPos;
        pos.y += characterHeight / 2f;
        bloodPs.transform.position = pos;
        bloodPs.Play();
        bloodPsTween = DOVirtual.DelayedCall(3, () =>
        {
            bloodPs.gameObject.SetActive(false);
        });
    }

    public void ShowInkSprite(Vector3 hitPos, float characterHeight)
    {
        if (Variables.isSkinReal) return;
        if (inkSrTween != null) inkSrTween.Kill();

        inkSr.gameObject.SetActive(true);
        transform.parent = null;
        inkSr.transform.localScale = Vector3.zero;
        hitPos.z -= 0.1f;
        hitPos.y += characterHeight / 2f;
        inkSr.transform.position = hitPos;
        inkSr.transform.DOScale(inkScale, 0.5f);

        inkSrTween = DOVirtual.DelayedCall(3, () =>
        {
            inkSr.gameObject.SetActive(false);
        });
    }

}
