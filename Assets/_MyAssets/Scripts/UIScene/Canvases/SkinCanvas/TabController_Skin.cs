using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

public class TabController_Skin : MonoBehaviour
{
    [SerializeField] SkinSelectButtonManager skinSelectButtonManager;
    [SerializeField] NoticeImageController noticeImageController;
    bool EnableUnlockRandom => SaveData.i.currencyCount >= Price && skinSelectButtonManager.NotOwnIndexes.Count > 0;
    int Price
    {
        get
        {
            SkinPrice skinPrice = CSVManager.i.CharacterSkinPrices.ClampIndex<SkinPrice>(PurchasedCount - 1);
            if (skinPrice == null) return 0;
            return skinPrice.price;
        }
    }
    int PurchasedCount => SaveData.i.characterSkinSaveDatas.Count(_ => _.isOwn);

    public void OnStart()
    {
        skinSelectButtonManager.Generator<SkinSelectButtonController_Skin>(SkinSettingSO.i.CharacterSkinDatas.Length, true);
        skinSelectButtonManager.OnCompleteRewardedAds = () =>
        {
            SaveData.i.currencyCount += Price;
            SaveDataManager.i.Save();
            FirebaseAnalyticsManager.i.LogEvent("character_skin", "reward_video_button");
        };
        skinSelectButtonManager.OnCompleteUnlock = (randomInt) =>
        {
            SaveData.i.currencyCount -= Price;
            SaveData.i.characterSkinSaveDatas[randomInt].isOwn = true;
            SaveData.i.selectedSkinIndex = randomInt;
            SaveDataManager.i.Save();
            FirebaseAnalyticsManager.i.LogEvent("character_skin", "unlock_random_button");
        };

        this.ObserveEveryValueChanged(_ => EnableUnlockRandom)
            .Subscribe(_ =>
            {
                noticeImageController.gameObject.SetActive(_);
                skinSelectButtonManager.OnChangedUnlockButtonInteractable(_);
            });

        this.ObserveEveryValueChanged(_ => Price)
            .Subscribe(_ => skinSelectButtonManager.OnChangedPrice(_));

        skinSelectButtonManager.OnStart();
    }

}
