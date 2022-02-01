using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectManager : MonoBehaviour
{
    [SerializeField] SkinSettingSO skinSettingSO;
    [SerializeField] SoundResourceSO soundResourceSO;
    [SerializeField] StagePrefabPathSO stagePrefabPathSO;

    public void SetInstance()
    {
        if (skinSettingSO) SkinSettingSO.i = skinSettingSO;
        if (soundResourceSO) SoundResourceSO.i = soundResourceSO;
        if (stagePrefabPathSO) stagePrefabPathSO.SetInstance(stagePrefabPathSO);
    }
}
