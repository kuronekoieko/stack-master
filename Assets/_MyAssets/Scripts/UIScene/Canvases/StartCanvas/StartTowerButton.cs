using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
public class StartTowerButton : MonoBehaviour
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
    [SerializeField] Sprite[] buttonSprites;

    bool Interactive
    {
        get
        {
            if (CSVManager.i.PlayerLevelPriceTable.IsLast(SaveData.i.startHumanCount - 1))
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

            return false;
            if (Variables.isHideRewardedAds)
            {
                return false;
            }

            state = State.RewardedAds;
            return MaxSdkRewardedAds.i.IsRewardedAdReady;
        }
    }

    int Price
    {
        get
        {
            PlayerLevelPrice playerLevelPrice = CSVManager.i.PlayerLevelPriceTable.ClampIndex(SaveData.i.startHumanCount - 1);
            if (playerLevelPrice == null) return 0;
            return playerLevelPrice.startTowerPrice;
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
        this.ObserveEveryValueChanged(_ => SaveData.i.startHumanCount)
            .Subscribe(_ => levelText.text = _.ToString());
        this.ObserveEveryValueChanged(_ => Price)
            .Subscribe(_ => priceText.text = _.ToString());
        this.ObserveEveryValueChanged(_ => (SaveData.i.currencyCount >= Price))
            .Subscribe(_ => noticeImageController.gameObject.SetActive(_));

        button.image.sprite = buttonSprites[Variables.isSkinReal ? 1 : 0];
    }

    public void OnOpen()
    {
        state = State.Buy;
        isViewedRewardedAds = false;

    }

    void OnClickLevelUpButton()
    {
        SoundManager.i?.PlayOneShot(4);
        switch (state)
        {
            case State.Buy:
                SaveData.i.currencyCount -= Price;
                SaveData.i.startHumanCount++;
                SaveDataManager.i.Save();
                FirebaseAnalyticsManager.i.LogEvent("start_tower_button", "purchased_" + SaveData.i.startHumanCount);
                break;
            case State.RewardedAds:
                Time.timeScale = 0;

                MaxSdkRewardedAds.i.ShowRewardedAd(
                    onRewarded: () =>
                    {
                        Time.timeScale = 1;
                        SaveData.i.startHumanCount++;
                        SaveDataManager.i.Save();
                        isViewedRewardedAds = true;
                        state = State.Buy;
                        FirebaseAnalyticsManager.i.LogEvent("start_tower_button", "reward_video_" + SaveData.i.startHumanCount);
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
