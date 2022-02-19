using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxSdkManager : MonoBehaviour
{
    public static MaxSdkManager i;
    // studio zzz
    static readonly string sdkKey = "nirDFR5Ia8FAlCZc9rlX8wP_kDNEiuHc4HGKHSQ4fFDhScOd5lW-T31uZdVJ9vqy35mpZCJVS7URsASrlXu7iF";


    void Awake()
    {
        i = this;
    }

    void Start()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
        {
            // AppLovin SDK is initialized, start loading ads
            MaxSdkInterstitial.i.InitializeInterstitialAds();
            MaxSdkBanner.i.InitializeBannerAds();
            MaxSdkRewardedAds.i.InitializeRewardedAds();
        };
        MaxSdk.SetSdkKey(sdkKey);
        // MaxSdk.SetUserId("USER_ID");
        MaxSdk.InitializeSdk();
        MaxSdk.SetMuted(true);  // オーディオのミュート
    }
}
