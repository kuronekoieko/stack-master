using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyGame/Create ParameterSettingSO", fileName = "ParameterSettingSO")]
public class ParameterSettingSO : ScriptableObject
{
    static ParameterSettingSO _i;
    public static ParameterSettingSO i
    {
        get
        {
            if (Variables.isLaunchUIScene) return _i;
            string PATH = "ScriptableObjects/" + nameof(ParameterSettingSO);
            //初アクセス時にロードする
            if (_i == null)
            {
                _i = Resources.Load<ParameterSettingSO>(PATH);

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
public class GiftRewardData
{
    public int probability;
    public int rewardCurrency;
}