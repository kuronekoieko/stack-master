using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectManager : MonoBehaviour
{
    [SerializeField] StageSettingsSO stageSettingsSO;
    [SerializeField] SkinSettingSO skinSettingSO;
    [SerializeField] SoundResourceSO soundResourceSO;

    public void SetInstance()
    {
        if (stageSettingsSO) StageSettingsSO.i = stageSettingsSO;
        if (skinSettingSO) SkinSettingSO.i = skinSettingSO;
        if (soundResourceSO) SoundResourceSO.i = soundResourceSO;
    }
}
