using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MaxSdkRewardedAds : MonoBehaviour
{
    // studio zzz
    string adUnitId = "ec802260c3aa2c72";
    int retryAttempt;
    Action onRewarded = () => { };
    Action onNotRewarded = () => { };
    public static MaxSdkRewardedAds i;

    void Awake()
    {
        i = this;
    }

    public void InitializeRewardedAds()
    {
        // Attach callback
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        // Load the first rewarded ad
        LoadRewardedAd();
    }

    private void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(adUnitId);
    }

    public void ShowRewardedAd(Action onRewarded, Action onNotRewarded)
    {
        this.onRewarded = onRewarded;
        this.onNotRewarded = onNotRewarded;

        if (Debug.isDebugBuild)
        {
            onRewarded();
            return;
        }

        if (!MaxSdk.IsRewardedAdReady(adUnitId))
        {
            onNotRewarded();
            return;
        }

        MaxSdk.ShowRewardedAd(adUnitId);
    }

    public bool IsRewardedAdReady => MaxSdk.IsRewardedAdReady(adUnitId);

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.
        // 報われた広告を表示する準備ができました。MaxSdk.IsRewardedAdReady(adUnitId)が'true'を返すようになりました。

        // Reset retry attempt
        retryAttempt = 0;
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).
        // リワード広告の読み込みに失敗しました 
        // AppLovinでは、最大の遅延（ここでは64秒）まで、指数関数的に高い遅延で再試行することを推奨しています。

        retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));

        Invoke(nameof(LoadRewardedAd), (float)retryDelay);
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        // リワード広告の表示に失敗しました。AppLovinは、次の広告を読み込むことを推奨します。
        LoadRewardedAd();
        onNotRewarded();
        Debug.Log("テスト MaxSdk OnRewardedAdFailedToDisplayEvent");
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        // 報酬を得た広告は非表示です。次の広告をプレロードする
        LoadRewardedAd();
        onNotRewarded();
        Debug.Log("テスト MaxSdk OnRewardedAdHiddenEvent");
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // The rewarded ad displayed and the user should receive the reward.
        // リワード広告が表示され、ユーザーがリワードを受け取ること。
        onRewarded();
        Debug.Log("テスト MaxSdk OnRewardedAdReceivedRewardEvent");
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Ad revenue paid. Use this callback to track user revenue.
        // 支払われた広告収入。このコールバックを使用して、ユーザーの収益を追跡します。
    }
}
