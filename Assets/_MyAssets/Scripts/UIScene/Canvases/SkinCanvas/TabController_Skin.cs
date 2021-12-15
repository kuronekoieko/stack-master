using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class TabController_Skin : MonoBehaviour
{

    [SerializeField] SkinSelectButtonManager skinSelectButtonManager;
    public void OnStart()
    {
        skinSelectButtonManager.Generator<SkinSelectButtonController_Skin>(SkinSettingSO.i.characterSkinDatas.Length, true);
        skinSelectButtonManager.OnCompleteRewardedAds = () =>
        {
            SaveData.i.currencyCount += ParameterSettingSO.i.SkinRewardedCurrency;
            SaveDataManager.i.Save();
        };
        skinSelectButtonManager.OnCompleteUnlock = (randomInt) =>
        {
            SaveData.i.currencyCount -= ParameterSettingSO.i.SkinUnlockRandomCurrency;
            SaveData.i.characterSkinSaveDatas[randomInt].isOwn = true;
            SaveData.i.selectedSkinIndex = randomInt;
            SaveDataManager.i.Save();
        };
        skinSelectButtonManager.unlockRandomCurrency = ParameterSettingSO.i.SkinUnlockRandomCurrency;
        skinSelectButtonManager.rewardedCurrency = ParameterSettingSO.i.SkinRewardedCurrency;
        skinSelectButtonManager.OnStart();
    }

}
