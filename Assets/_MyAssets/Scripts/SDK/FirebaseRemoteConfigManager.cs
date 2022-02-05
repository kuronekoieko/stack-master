using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using System.Threading.Tasks;


public class FirebaseRemoteConfigManager : SingletonMonoBehaviour<FirebaseRemoteConfigManager>
{
    readonly string isSkinReal = "is_skin_real";
    readonly string isStage30Sec = "is_stage_30_sec";
    readonly string isZeroCameraPosX = "is_zero_camera_pos_x";
    readonly string isShowInterstitialOddLevel = "is_show_interstitial_odd_level";
    readonly string isShowInterstitialBeforeRestartLevel = "is_show_interstitial_before_restart_level";
    public bool IsFetchComplete { get; private set; }

    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        Dictionary<string, object> defaults = new Dictionary<string, object>();

        // 今回ABテストで使うパラメータのデフォルト値
        defaults.Add(isSkinReal, true);
        defaults.Add(isStage30Sec, true);
        defaults.Add(isZeroCameraPosX, true);
        defaults.Add(isShowInterstitialOddLevel, true);
        defaults.Add(isShowInterstitialBeforeRestartLevel, true);
        IsFetchComplete = false;

        FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
        .ContinueWithOnMainThread((task) =>
        {

        });

        FetchDataAsync();
    }

    ConfigValue GetConfigValue(string key)
    {
        return FirebaseRemoteConfig.DefaultInstance.GetValue(key);
    }

    // Start a fetch request.
    // FetchAsync only fetches new data if the current data is older than the provided
    // timespan.  Otherwise it assumes the data is "recent enough", and does nothing.
    // By default the timespan is 12 hours, and for production apps, this is a good
    // number. For this example though, it's set to a timespan of zero, so that
    // changes in the console will always show up immediately.
    Task FetchDataAsync()
    {
        Debug.Log("Fetching data...");
        Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        Action<Task> FetchComplete = (task) =>
        {
            // フェッチ後にアクティベートしないと、値が取得できない
            FirebaseRemoteConfig.DefaultInstance.ActivateAsync();
            Debug.Log("FetchComplete");
            Variables.isSkinReal = GetConfigValue(isSkinReal).BooleanValue;
            Variables.isStage30Sec = GetConfigValue(isStage30Sec).BooleanValue;
            Variables.isZeroCameraPosX = GetConfigValue(isZeroCameraPosX).BooleanValue;
            Variables.isShowInterstitialOddLevel = GetConfigValue(isShowInterstitialOddLevel).BooleanValue;
            Variables.isShowInterstitialBeforeRestartLevel = GetConfigValue(isShowInterstitialBeforeRestartLevel).BooleanValue;
            IsFetchComplete = true;
        };
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }
}
