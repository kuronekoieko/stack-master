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
        bloodPsChildren = bloodPs.GetComponentsInChildren<ParticleSystem>();
        inkScale = inkSr.transform.lossyScale;
        inkSr.gameObject.SetActive(false);
        bloodPs.gameObject.SetActive(false);
        this.ObserveEveryValueChanged(_ => SaveData.i.selectedMaterialIndex)
            .Subscribe(_ => OnChangedMaterial(_));
    }

    void OnChangedMaterial(int selectedIndex)
    {
        inkSr.material = new Material(SkinSettingSO.i.characterMaterialDatas[selectedIndex].material);
        for (int i = 0; i < bloodPsChildren.Length; i++)
        {
            ParticleSystem.MainModule main = bloodPsChildren[i].main;
            main.startColor = inkSr.material.color;
        }
    }

    public void PlayBloodParticle(float characterHeight)
    {
        if (bloodPsTween != null) bloodPsTween.Kill();
        bloodPs.gameObject.SetActive(true);
        transform.parent = null;
        var pos = transform.position;
        pos.y += characterHeight / 2f;
        bloodPs.transform.position = pos;
        bloodPsTween = DOVirtual.DelayedCall(3, () =>
        {
            bloodPs.gameObject.SetActive(false);
        });
    }

    public void ShowInkSprite(Vector3 hitPos, float characterHeight)
    {
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
