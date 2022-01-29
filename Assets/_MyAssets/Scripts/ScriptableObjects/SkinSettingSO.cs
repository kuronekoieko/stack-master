using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "MyGame/Create SkinSettingSO", fileName = "SkinSettingSO")]
public class SkinSettingSO : ScriptableObject
{
    public RuntimeAnimatorController animatorController;
    [SerializeField] CharacterSkinData[] characterSkinDatas;
    public CharacterMaterialData[] characterMaterialDatas;
    [SerializeField] public CharacterSkinData[] characterSkinDatas_Real;

    public CharacterSkinData[] CharacterSkinDatas => Variables.isSkinReal ? characterSkinDatas_Real : characterSkinDatas;

    static SkinSettingSO _i;
    public static SkinSettingSO i
    {
        get
        {
            if (Variables.isLaunchUIScene) return _i;
            string PATH = "ScriptableObjects/" + nameof(SkinSettingSO);
            //初アクセス時にロードする
            if (_i == null)
            {
                _i = Resources.Load<SkinSettingSO>(PATH);
                Debug.Log("load");

                //ロード出来なかった場合はエラーログを表示
                if (_i == null)
                {
                    Debug.LogError(PATH + " not found");
                }
            }

            return _i;
        }
        set { _i = value; }
    }
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