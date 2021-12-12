using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SkinController : MonoBehaviour
{
    Animator animator;
    public Animator Animator => animator;
    SkinnedMeshRenderer[] skinnedMeshRenderers;
    Material material;

    public void OnInstantiate(Material material)
    {
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = SkinSettingSO.i.animatorController;
        this.material = material;
        ChangeMaterial(material);
    }

    public void ChangeMaterial(Material material)
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
                if (texture)
                {
                    materials[j] = new Material(skinnedMeshRenderer.materials[j]);
                }
                else
                {
                    materials[j] = new Material(material);
                }

            }
            skinnedMeshRenderer.materials = materials;
        }

    }
}
