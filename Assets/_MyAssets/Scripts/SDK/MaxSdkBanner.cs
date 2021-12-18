using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxSdkBanner : MonoBehaviour
{
    // studio zzz
    string bannerAdUnitId = "643474e10e4d8d8c"; // Retrieve the ID from your account

    public static MaxSdkBanner i;

    void Awake()
    {
        i = this;
    }

    public void InitializeBannerAds()
    {
        if (Debug.isDebugBuild) return;
        // Banners are automatically sized to 320×50 on phones and 728×90 on tablets
        // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
        MaxSdk.CreateBanner(bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);

        // Set background or background color for banners to be fully functional
        MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, Color.white);
    }

    public void Show()
    {
        if (Debug.isDebugBuild) return;
        MaxSdk.ShowBanner(bannerAdUnitId);
    }

    public void Hide()
    {
        if (Debug.isDebugBuild) return;
        MaxSdk.HideBanner(bannerAdUnitId);
    }
}
