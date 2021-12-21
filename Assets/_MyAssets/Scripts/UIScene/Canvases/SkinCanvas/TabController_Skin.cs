using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

public class TabController_Skin : MonoBehaviour
{
    [SerializeField] SkinSelectButtonManager skinSelectButtonManager;
    bool EnableUnlockRandom => SaveData.i.currencyCount >= Price && skinSelectButtonManager.NotOwnIndexes.Count > 0;
    int Price => CSVManager.i.CharacterSkinPrices.ClampIndex<SkinPrice>(PurchasedCount - 1).price;
    int PurchasedCount => SaveData.i.characterSkinSaveDatas.Count(_ => _.isOwn);

    public void OnStart()
    {
        skinSelectButtonManager.Generator<SkinSelectButtonController_Skin>(SkinSettingSO.i.characterSkinDatas.Length, true);
        skinSelectButtonManager.OnCompleteRewardedAds = () =>
        {
            SaveData.i.currencyCount += Price;
            SaveDataManager.i.Save();
        };
        skinSelectButtonManager.OnCompleteUnlock = (randomInt) =>
        {
            SaveData.i.currencyCount -= Price;
            SaveData.i.characterSkinSaveDatas[randomInt].isOwn = true;
            SaveData.i.selectedSkinIndex = randomInt;
            SaveDataManager.i.Save();
        };

        this.ObserveEveryValueChanged(_ => EnableUnlockRandom)
            .Subscribe(_ => skinSelectButtonManager.OnChangedUnlockButtonInteractable(_));

        this.ObserveEveryValueChanged(_ => Price)
            .Subscribe(_ => skinSelectButtonManager.OnChangedPrice(_));

        skinSelectButtonManager.OnStart();
    }

}
