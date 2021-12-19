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
    }

    [SerializeField] MyButton button;
    [SerializeField] Text priceText;
    [SerializeField] Text freeText;
    [SerializeField] Image gemImage;
    [SerializeField] Image videoImage;
    [SerializeField] Text levelText;

    bool Interactive
    {
        get
        {
            if (isViewedRewardedAds)
            {
                return false;
            }

            if (SaveData.i.currencyCount >= ParameterSettingSO.i.offlineIncomePrice)
            {
                state = State.Buy;
                return true;
            }

            state = State.RewardedAds;
            return MaxSdkRewardedAds.i.IsRewardedAdReady;
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
    }

    public void OnOpen()
    {
        state = State.Buy;
        isViewedRewardedAds = false;

    }

    void OnClickLevelUpButton()
    {
        SoundManager.i.PlayOneShot(0);

        switch (state)
        {
            case State.Buy:
                SaveData.i.currencyCount -= ParameterSettingSO.i.offlineIncomePrice;
                SaveData.i.offlineIncomeLevel++;
                SaveDataManager.i.Save();
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

        priceText.text = ParameterSettingSO.i.offlineIncomePrice.ToString();
    }
}
