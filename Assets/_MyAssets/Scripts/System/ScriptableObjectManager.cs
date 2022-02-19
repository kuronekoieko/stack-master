using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectManager : SingletonMonoBehaviour<ScriptableObjectManager>
{
    [SerializeField] SkinSettingSO skinSettingSO_Stickman;
    [SerializeField] SkinSettingSO skinSettingSO_Real;
    [SerializeField] SoundResourceSO soundResourceSO;
    [SerializeField] StagePrefabPathSO stagePrefabPathSO;
    public SkinSettingSO SkinSettingSO { get; private set; }

    public void SetInstance()
    {
        if (Variables.isSkinReal)
        {
            SkinSettingSO = skinSettingSO_Real;
        }
        else
        {
            SkinSettingSO = skinSettingSO_Stickman;
        }
        if (soundResourceSO) SoundResourceSO.i = soundResourceSO;
        if (stagePrefabPathSO) stagePrefabPathSO.SetInstance(stagePrefabPathSO);
    }
}
