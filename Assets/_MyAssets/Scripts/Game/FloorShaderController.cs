using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorShaderController : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;
    void Start()
    {
        // Material material = new Material(meshRenderer.material);
        // meshRenderer.material = material;
        // 何故かエディターではエラーが出ない
        if (meshRenderer.material.HasProperty("_MainTex"))
        {
            meshRenderer.material.SetTextureScale("_MainTex", new Vector2(1, transform.parent.localScale.z * 3f / 4f));
        }
    }
}
