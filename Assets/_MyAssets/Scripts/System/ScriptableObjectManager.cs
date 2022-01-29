using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectManager : MonoBehaviour
{
    [SerializeField] StageSettingsSO stageSettingsSO;
    [SerializeField] ParameterSettingSO parameterSettingSO;
    [SerializeField] SkinSettingSO skinSettingSO;
    [SerializeField] SoundResourceSO soundResourceSO;

    void Start()
    {
        SetInstance();
    }
    public void SetInstance()
    {
        if (StageSettingsSO.i == null) StageSettingsSO.i = stageSettingsSO;
        if (ParameterSettingSO.i == null) ParameterSettingSO.i = parameterSettingSO;
        if (SkinSettingSO.i == null) SkinSettingSO.i = skinSettingSO;
        if (SoundResourceSO.i == null) SoundResourceSO.i = soundResourceSO;
    }
}
