using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabController_Material : MonoBehaviour
{
    [SerializeField] SkinSelectButtonManager skinSelectButtonManager;
    public void OnStart()
    {
        skinSelectButtonManager.Generator<SkinSelectButtonController_Material>(SkinSettingSO.i.characterMaterialDatas.Length, false);

        skinSelectButtonManager.OnCompleteRewardedAds = () =>
        {
            SaveData.i.currencyCount += ParameterSettingSO.i.SkinRewardedCurrency;
            SaveDataManager.i.Save();
        };
        skinSelectButtonManager.OnCompleteUnlock = (randomInt) =>
        {
            SaveData.i.currencyCount -= ParameterSettingSO.i.SkinUnlockRandomCurrency;
            SaveData.i.materialSkinSaveDatas[randomInt].isOwn = true;
            SaveData.i.selectedMaterialIndex = randomInt;
            SaveDataManager.i.Save();
        };
        skinSelectButtonManager.unlockRandomCurrency = ParameterSettingSO.i.SkinUnlockRandomCurrency;
        skinSelectButtonManager.rewardedCurrency = ParameterSettingSO.i.SkinRewardedCurrency;
        skinSelectButtonManager.OnStart();
    }
}
