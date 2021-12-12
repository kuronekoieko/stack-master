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

    private static SkinSettingSO _i;
    public static SkinSettingSO i
    {
        get
        {
            string PATH = "ScriptableObjects/" + nameof(SkinSettingSO);
            //初アクセス時にロードする
            if (_i == null)
            {
                _i = Resources.Load<SkinSettingSO>(PATH);

                //ロード出来なかった場合はエラーログを表示
                if (_i == null)
                {
                    Debug.LogError(PATH + " not found");
                }
            }

            return _i;
        }
    }
}

[System.Serializable]
public class CharacterSkinData
{
    [OnValueChanged(nameof(OnPrefabSet))]
    public GameObject prefab;
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