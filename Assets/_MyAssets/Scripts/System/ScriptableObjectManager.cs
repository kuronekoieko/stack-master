using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectManager : MonoBehaviour
{
    [SerializeField] StageSettingsSO stageSettingsSO;
    [SerializeField] ParameterSettingSO parameterSettingSO;
    [SerializeField] SkinSettingSO skinSettingSO;
    [SerializeField] SoundResourceSO soundResourceSO;
    void Awake()
    {
        StageSettingsSO.i = stageSettingsSO;
        ParameterSettingSO.i = parameterSettingSO;
        SkinSettingSO.i = skinSettingSO;
        SoundResourceSO.i = soundResourceSO;
    }

}
