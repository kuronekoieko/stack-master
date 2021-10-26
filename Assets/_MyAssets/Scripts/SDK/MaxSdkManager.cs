using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxSdkManager : MonoBehaviour
{

    void Start()
    {
        Debug.Log("テスト " + 0);
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
        {
            Debug.Log("テスト " + 1);
            // AppLovin SDK is initialized, start loading ads
            MaxSdkInterstitial.i.InitializeInterstitialAds();
            Debug.Log("テスト " + 2);
            MaxSdkBanner.i.InitializeBannerAds();

            Debug.Log("テスト " + 3);
        };
        Debug.Log("テスト " + 4);
        MaxSdk.SetSdkKey("nirDFR5Ia8FAlCZc9rlX8wP_kDNEiuHc4HGKHSQ4fFDhScOd5lW-T31uZdVJ9vqy35mpZCJVS7URsASrlXu7iF");
        Debug.Log("テスト " + 5);
        // MaxSdk.SetUserId("USER_ID");
        MaxSdk.InitializeSdk();
        Debug.Log("テスト " + 6);
        MaxSdk.SetMuted(true);  // オーディオのミュート
        Debug.Log("テスト " + 7);
    }
}
