using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MaxSdkInterstitial : MonoBehaviour
{
    // studio zzz
    string adUnitId = "328618b8e949358c";
    int retryAttempt;
    public static MaxSdkInterstitial i;
    Action onHidden = () => { };
    public bool IsDebug => Debug.isDebugBuild && false;

    void Awake()
    {
        i = this;
    }

    public void Show(Action onHidden)
    {
        this.onHidden = onHidden;
        if (IsDebug)
        {
            onHidden();
            return;
        }

        if (!MaxSdk.IsInterstitialReady(adUnitId))
        {
            onHidden();
            return;
        }

        Time.timeScale = 0;
        MaxSdk.ShowInterstitial(adUnitId);
    }

    public void ShowOnClear(int level, Action onHidden)
    {
        if (IsShowAdLevel(level))
        {
            Show(onHidden);
        }
        else
        {
            onHidden();
        }
    }

    bool IsShowAdLevel(int level)
    {
        if (Variables.isShowInterstitialOddLevel)
        {
            // ・広告は最初3ステージは非表示。4ステージ目からは、2回プレイごとに1回でる
            if (level <= 3) return false;
            if (level % 2 == 1) return false;
            return true;
        }
        else
        {
            // 1以外全部
            if (level == 1) return false;
            return true;
        }
    }

    public void InitializeInterstitialAds()
    {
        // Attach callback
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;

        // Load the first interstitial
        LoadInterstitial();
    }

    private void LoadInterstitial()
    {
        MaxSdk.LoadInterstitial(adUnitId);
    }

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'

        // Reset retry attempt
        retryAttempt = 0;
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)

        retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));

        Invoke("LoadInterstitial", (float)retryDelay);
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        onHidden();
        Time.timeScale = 1;
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
        LoadInterstitial();
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        onHidden();
        Time.timeScale = 1;
        // Interstitial ad is hidden. Pre-load the next ad.
        LoadInterstitial();
    }
}
