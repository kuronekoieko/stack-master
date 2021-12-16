using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(Animator))]
public class SkinController : MonoBehaviour
{
    Animator animator;
    public Animator Animator => animator;
    public RectTransform RectTransform { get; set; }

    SkinnedMeshRenderer[] skinnedMeshRenderers;
    public bool IsSetMaterial_Manual { get; set; }

    public void OnInstantiate()
    {
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = SkinSettingSO.i.animatorController;
        this.ObserveEveryValueChanged(_ => SaveData.i.selectedMaterialIndex)
            .Where(_ => !IsSetMaterial_Manual)
            .Subscribe(_ => ChangeMaterial(SkinSettingSO.i.characterMaterialDatas[SaveData.i.selectedMaterialIndex].material));
    }

    public void ChangeMaterial(Material material, bool isAll = false)
    {
        skinnedMeshRenderers = animator.GetComponentsInChildren<SkinnedMeshRenderer>();

        for (int i = 0; i < skinnedMeshRenderers.Length; i++)
        {
            var skinnedMeshRenderer = skinnedMeshRenderers[i];
            // https://ymgsapo.com/2021/04/27/change-material-script/
            var materials = new Material[skinnedMeshRenderer.sharedMaterials.Length];
            for (int j = 0; j < materials.Length; j++)
            {
                if (skinnedMeshRenderer.materials[j] == null) continue;
                Texture texture = skinnedMeshRenderer.materials[j].GetTexture("_MainTex");

                if (isAll)
                {
                    materials[j] = new Material(material);
                    continue;
                }

                if (texture)
                {
                    materials[j] = new Material(skinnedMeshRenderer.materials[j]);
                    continue;
                }

                materials[j] = new Material(material);
            }
            skinnedMeshRenderer.materials = materials;
        }

    }

    public void ChangeLayersForAllChildren(string layerName)
    {
        ChangeLayersForChildren(transform, layerName);
        gameObject.layer = LayerMask.NameToLayer(layerName);
    }


    void ChangeLayersForChildren(Transform transform, string layerName)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer(layerName);
            ChangeLayersForChildren(child, layerName);
        }
    }
}