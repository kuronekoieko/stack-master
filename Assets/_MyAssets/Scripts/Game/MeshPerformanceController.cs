using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MeshPerformanceController : MonoBehaviour
{
    [Inject] CharacterManager characterManager;
    List<Renderer> renderers = new List<Renderer>();
    public void OnStart(GameObject stageGO)
    {
        Renderer[] rendererArray = stageGO.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < rendererArray.Length; i++)
        {
            if (rendererArray[i].gameObject.CompareTag("Floor")) continue;
            if (!rendererArray[i].enabled) continue;
            renderers.Add(rendererArray[i]);
            rendererArray[i].enabled = false;
        }
    }

    public void OnUpdate()
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            float dz = renderers[i].transform.position.z - characterManager.BottomCharacterPos.z;
            bool isInside = -20f < dz && dz < 80f;
            renderers[i].enabled = isInside;
        }
    }
}
