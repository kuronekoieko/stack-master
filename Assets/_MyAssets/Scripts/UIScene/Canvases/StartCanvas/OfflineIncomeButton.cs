using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class OfflineIncomeButton : MonoBehaviour
{
    enum State
    {
        Buy,
        RewardedAds,
        Max,
    }

    [SerializeField] MyButton button;
    [SerializeField] Text priceText;
    [SerializeField] Text freeText;
    [SerializeField] Image gemImage;
    [SerializeField] Image videoImage;
    [SerializeField] Text levelText;
    [SerializeField] NoticeImageController noticeImageController;

    bool Interactive
    {
        get
        {
            if (CSVManager.i.PlayerLevelPriceTable.IsLast(SaveData.i.offlineIncomeLevel - 1))
            {
                state = State.Max;
                return false;
            }

            if (isViewedRewardedAds)
            {
                return false;
            }

            if (SaveData.i.currencyCount >= Price)
            {
                state = State.Buy;
                return true;
            }

            state = State.RewardedAds;
            return MaxSdkRewardedAds.i.IsRewardedAdReady;
        }
    }
    int Price
    {
        get
        {
            PlayerLevelPrice playerLevelPrice = CSVManager.i.PlayerLevelPriceTable.ClampIndex(SaveData.i.offlineIncomeLevel - 1);
            if (playerLevelPrice == null) return 0;
            return playerLevelPrice.offlineIncomePrice;
        }
    }

    State state;
    bool isViewedRewardedAds;

    public void OnStart()
    {
        button.onClick.AddListener(OnClickLevelUpButton);
        this.ObserveEveryValueChanged(_ => Interactive)
            .Subscribe(_ => button.interactable = _);
        this.ObserveEveryValueChanged(_ => state)
            .Subscribe(_ => ChangeButtonView(_));
        this.ObserveEveryValueChanged(_ => SaveData.i.offlineIncomeLevel)
            .Subscribe(_ => levelText.text = _.ToString());
        this.ObserveEveryValueChanged(_ => Price)
            .Subscribe(_ => priceText.text = _.ToString());
        this.ObserveEveryValueChanged(_ => Interactive)
            .Subscribe(_ => noticeImageController.gameObject.SetActive(_));
    }

    public void OnOpen()
    {
        state = State.Buy;
        isViewedRewardedAds = false;

    }

    void OnClickLevelUpButton()
    {

        switch (state)
        {
            case State.Buy:
                SaveData.i.currencyCount -= Price;
                SaveData.i.offlineIncomeLevel++;
                SaveDataManager.i.Save();
                FirebaseAnalyticsManager.i.LogEvent("offline_income_button", "purchased");
                SoundManager.i?.PlayOneShot(4);
                break;
            case State.RewardedAds:
                Time.timeScale = 0;

                MaxSdkRewardedAds.i.ShowRewardedAd(
                    onRewarded: () =>
                    {
                        Time.timeScale = 1;
                        SaveData.i.offlineIncomeLevel++;
                        SaveDataManager.i.Save();
                        isViewedRewardedAds = true;
                        state = State.Buy;
                        FirebaseAnalyticsManager.i.LogEvent("offline_income_button", "reward_video");
                    },
                    onNotRewarded: () =>
                    {
                        Time.timeScale = 1;
                    }
                );
                break;
            default:
                break;
        }
    }

    void ChangeButtonView(State state)
    {
        priceText.gameObject.SetActive(state == State.Buy);
        gemImage.gameObject.SetActive(state == State.Buy);
        freeText.gameObject.SetActive(state == State.RewardedAds);
        videoImage.gameObject.SetActive(state == State.RewardedAds);

        if (state == State.Max)
        {
            freeText.gameObject.SetActive(true);
            freeText.text = "MAX";
        }
    }
}
