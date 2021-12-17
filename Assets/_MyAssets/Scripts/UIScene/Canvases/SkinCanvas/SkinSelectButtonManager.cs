using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SnapScroll;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;
using UniRx;
using System;

public class SkinSelectButtonManager : MonoBehaviour
{
    [SerializeField] SkinSelectButtonController skinSelectPrefab;
    [SerializeField] RectTransform scrollViewContent;
    [SerializeField] SnapScrollView scrollView;
    [SerializeField] Image originIndicator;
    [SerializeField] Transform indicatorParent;
    [SerializeField] Sprite activeIndicatorSprite;
    [SerializeField] Sprite inActiveIndicatorSprite;
    [SerializeField] MyButton scrollButton_Right;
    [SerializeField] MyButton scrollButton_Left;
    [SerializeField] GameObject skinMasks;
    [SerializeField] MyButton unlockButton;
    [SerializeField] MyButton rewardedButton;
    Image[] indicators;
    SkinSelectButtonController[] skinSelectControllers = new SkinSelectButtonController[0];//初期化時nullのため
    int contentsCountPerPage = 9;
    List<int> notOwnIndexes = new List<int>();
    [System.NonSerialized] public Action OnCompleteRewardedAds = () => { };
    [System.NonSerialized] public Action<int> OnCompleteUnlock = (randomInt) => { };
    [System.NonSerialized] public int unlockRandomCurrency;
    [System.NonSerialized] public int rewardedCurrency;
    bool EnableUnlockRandom => SaveData.i.currencyCount >= unlockRandomCurrency && NotOwnIndexes.Count > 0;

    public virtual void OnStart()
    {
        scrollButton_Right.onClick.AddListener(() => OnClickScrollButton(true));
        scrollButton_Left.onClick.AddListener(() => OnClickScrollButton(false));
        skinMasks.SetActive(true);

        unlockButton.onClick.AddListener(OnClickUnlockButton);
        rewardedButton.onClick.AddListener(OnClickRewardedButton);
        this.ObserveEveryValueChanged(_ => MaxSdkRewardedAds.i.IsRewardedAdReady)
            .Subscribe(_ => OnChangedRewardedAdReady(_));

        this.ObserveEveryValueChanged(_ => EnableUnlockRandom)
            .Subscribe(_ => unlockButton.interactable = _);

        unlockButton.Text.text = unlockRandomCurrency.ToString();
        rewardedButton.Text.text = "+" + rewardedCurrency;
    }

    public void Generator<T>(int buttonCount, bool isCharacterSkin) where T : MonoBehaviour
    {
        if (skinSelectControllers.Length > 0) { return; }


        skinSelectControllers = new SkinSelectButtonController[buttonCount];
        for (int i = 0; i < skinSelectControllers.Length; i++)
        {
            skinSelectControllers[i] = Instantiate(skinSelectPrefab, scrollViewContent);
            skinSelectControllers[i].gameObject.AddComponent<T>();
            skinSelectControllers[i].OnInstantiate(i, false);

        }

        int dummyButtonCount = skinSelectControllers.Length - skinSelectControllers.Length % contentsCountPerPage;
        for (int i = 0; i < dummyButtonCount; i++)
        {
            Instantiate(skinSelectPrefab, scrollViewContent).OnInstantiate(i, true);
        }

        /// 【UnityAsset】SnapScroll – iPhoneのホーム画面のようなスナップスクロールを作る
        /// https://tempura-kingdom.jp/snapscroll/

        scrollView.MaxPage = Mathf.CeilToInt((float)skinSelectControllers.Length / (float)contentsCountPerPage) - 1;
        // scrollView.PageSize = scrollView.GetComponent<RectTransform>().sizeDelta.x;
        scrollView.PageSize = 800;
        scrollView.ScrollableDistance = 0.01f;//スワイプ感度
        scrollView.OnPageChanged += OnPageChanged;
        GenerateIndicators();
        scrollView.RefreshPage();
    }


    void GenerateIndicators()
    {
        indicators = new Image[scrollView.MaxPage + 1];

        float offset = 60f;
        Vector3 pos = -Vector3.right * offset * (indicators.Length - 1) / 2f;
        for (int i = 0; i < indicators.Length; i++)
        {
            indicators[i] = Instantiate(originIndicator, indicatorParent);
            indicators[i].rectTransform.anchoredPosition3D = pos;
            pos.x += offset;
        }
        originIndicator.gameObject.SetActive(false);
    }

    void OnPageChanged()
    {
        for (var i = 0; i < indicators.Length; i++)
        {
            indicators[i].sprite = (i == scrollView.Page) ? activeIndicatorSprite : inActiveIndicatorSprite;
        }

        scrollButton_Left.gameObject.SetActive(scrollView.Page != 0);
        scrollButton_Right.gameObject.SetActive(scrollView.Page != scrollView.MaxPage);
    }


    void OnClickScrollButton(bool isRight)
    {
        int page = scrollView.Page;
        if (isRight)
        {
            page++;
        }
        else
        {
            page--;
        }
        scrollView.Page = Mathf.Clamp(page, 0, scrollView.MaxPage);
        scrollView.RefreshPage();
    }

    List<int> NotOwnIndexes
    {
        get
        {
            int upperLeftIndex = scrollView.Page * contentsCountPerPage;
            int nextPageUpperLeftIndex = Mathf.Clamp(upperLeftIndex + contentsCountPerPage, 0, skinSelectControllers.Length);
            notOwnIndexes.Clear();
            for (int i = upperLeftIndex; i < nextPageUpperLeftIndex; i++)
            {
                if (skinSelectControllers[i].SelectState != SkinSelectState.Lock) continue;
                notOwnIndexes.Add(i);
            }
            return notOwnIndexes;
        }
    }

    public void UnlockRandom()
    {
        int randomInt = notOwnIndexes[UnityEngine.Random.Range(0, notOwnIndexes.Count)];
        OnCompleteUnlock(randomInt);
    }

    void OnClickUnlockButton()
    {
        SoundManager.i.PlayOneShot(0);
        UnlockRandom();
    }

    void OnClickRewardedButton()
    {
        SoundManager.i.PlayOneShot(0);
        Time.timeScale = 0;

        MaxSdkRewardedAds.i.ShowRewardedAd(
            onRewarded: () =>
            {
                Time.timeScale = 1;
                OnCompleteRewardedAds();
            },
            onNotRewarded: () =>
            {
                Time.timeScale = 1;
            }
        );
    }

    void OnChangedRewardedAdReady(bool isRewardedAdReady)
    {
        rewardedButton.interactable = isRewardedAdReady;
    }

}
