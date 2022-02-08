using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "MyGame/Create SkinSettingSO", fileName = "SkinSettingSO")]
public class SkinSettingSO : ScriptableObject
{
    public RuntimeAnimatorController animatorController;
    public CharacterSkinData[] characterSkinDatas;
    public CharacterMaterialData[] characterMaterialDatas;
}

[System.Serializable]
public class CharacterSkinData
{
    [OnValueChanged(nameof(OnPrefabSet))]
    public SkinController prefab;
    [ReadOnly]
    public string id;//TODO:初期値を自動で決めるようにする

    void OnPrefabSet()
    {
        id = prefab.name;
    }
}

[System.Serializable]
public class CharacterMaterialData
{
    [OnValueChanged(nameof(OnPrefabSet))]
    public Material material;
    [ReadOnly]
    public string id;//TODO:初期値を自動で決めるようにする

    void OnPrefabSet()
    {
        id = material.name;
    }
}