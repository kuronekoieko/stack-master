using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyGame/Create ParameterSettingSO", fileName = "ParameterSettingSO")]
public class ParameterSettingSO : ScriptableObject
{
    [Header("アンロックボタンの値段")]
    [Header("===スキン変更画面===")]
    public int SkinUnlockRandomCurrency;
    [Header("リワードボタンの報酬額")]
    public int SkinRewardedCurrency;

    [Header("===ギフト画面===")]
    public GiftRewardData[] giftRewardDatas;
    [Header("===スタート画面===")]
    public int addStartTowerPrice;

    public int offlineIncomePrice;

    private static ParameterSettingSO _i;
    public static ParameterSettingSO i
    {
        get
        {
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
    }
}

[System.Serializable]
public class GiftRewardData
{
    public int probability;
    public int rewardCurrency;
}