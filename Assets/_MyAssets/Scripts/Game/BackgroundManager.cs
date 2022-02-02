using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] int bgIndex;
    [SerializeField] Camera cam;
    [SerializeField] MeshRenderer planeMr;
    [SerializeField] Transform cubesParentTf;
    [SerializeField] BackgroundData[] backgroundDatas;
    int CurrentBGIndex => (StageTransManager.i.CurrentDisplayStageNum - 1) % backgroundDatas.Length;
    MeshRenderer[] cubeMrs;

    void OnValidate()
    {
        cubeMrs = cubesParentTf.GetComponentsInChildren<MeshRenderer>();
        Activate();
    }

    public void OnAwake()
    {
        cubeMrs = cubesParentTf.GetComponentsInChildren<MeshRenderer>();
        Activate();
    }

    void Activate()
    {
        var index = Variables.isLaunchUIScene ? CurrentBGIndex : bgIndex;
        BackgroundData backgroundData = backgroundDatas[index];
        cam.backgroundColor = backgroundData.FogColor;
        RenderSettings.fogColor = backgroundData.FogColor;
        foreach (var item in cubeMrs)
        {
            item.material = backgroundData.cubeMaterial;
        }
        planeMr.material = backgroundData.planeMaterial;
    }
}

[System.Serializable]
public class BackgroundData
{
    public Color FogColor;
    public Material planeMaterial;
    public Material cubeMaterial;
}
