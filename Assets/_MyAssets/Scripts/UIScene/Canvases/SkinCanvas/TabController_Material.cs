using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

public class TabController_Material : MonoBehaviour
{
    [SerializeField] SkinSelectButtonManager skinSelectButtonManager;
    bool EnableUnlockRandom => SaveData.i.currencyCount >= Price && skinSelectButtonManager.NotOwnIndexes.Count > 0;
    int Price
    {
        get
        {
            SkinPrice skinPrice = CSVManager.i.MaterialSkinPrices.ClampIndex<SkinPrice>(PurchasedCount - 1);
            if (skinPrice == null) return 0;
            return skinPrice.price;
        }
    }
    int PurchasedCount => SaveData.i.materialSkinSaveDatas.Count(_ => _.isOwn);

    public void OnStart()
    {
        skinSelectButtonManager.Generator<SkinSelectButtonController_Material>(SkinSettingSO.i.characterMaterialDatas.Length, false);

        skinSelectButtonManager.OnCompleteRewardedAds = () =>
        {
            SaveData.i.currencyCount += Price;
            SaveDataManager.i.Save();
        };
        skinSelectButtonManager.OnCompleteUnlock = (randomInt) =>
        {
            SaveData.i.currencyCount -= Price;
            SaveData.i.materialSkinSaveDatas[randomInt].isOwn = true;
            SaveData.i.selectedMaterialIndex = randomInt;
            SaveDataManager.i.Save();
        };

        this.ObserveEveryValueChanged(_ => EnableUnlockRandom)
            .Subscribe(_ => skinSelectButtonManager.OnChangedUnlockButtonInteractable(_));

        this.ObserveEveryValueChanged(_ => Price)
            .Subscribe(_ => skinSelectButtonManager.OnChangedPrice(_));

        skinSelectButtonManager.OnStart();
    }
}
