using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase.Analytics;

/// <summary>
/// Unity 用 Google アナリティクスを使ってみる
/// https://firebase.google.com/docs/analytics/unity/start?hl=ja
/// </summary>
public class FirebaseAnalyticsManager : MonoBehaviour
{
    public static FirebaseAnalyticsManager i => _i;
    private static FirebaseAnalyticsManager _i;

    bool isAvailable;

    void Awake()
    {
        _i = this;
    }


    public void Initialize()
    {
#if UNITY_IOS
        isAvailable = true;
#elif UNITY_ANDROID
        isAvailable = false;
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                //   app = Firebase.FirebaseApp.DefaultInstance;
                isAvailable = true;
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
#endif
    }

    public void LogEvent(
        string eventCategory,
        string eventAction,
        string eventLabel,
        long value)
    {
        if (!isAvailable) { return; }
        FirebaseAnalytics.LogEvent(
            name: eventCategory + ":" + eventAction,
            parameterName: eventAction,
            parameterValue: value);
    }

    public void LogTest(string title)
    {
        if (!isAvailable) { return; }
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventScreenView, "test_a", title);
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventScreenView, title, "test_b");
        FirebaseAnalytics.LogEvent("test_c", title, title);
        FirebaseAnalytics.LogEvent("test_name", "test_parameterName", "test_parameterValue");
    }

    public void LogEvent_StageStart(int stageNum)
    {
        if (!isAvailable) { return; }
        FirebaseAnalytics.LogEvent("stage_start", "stage_" + stageNum, stageNum);
    }

    public void LogEvent_StageClear(int stageNum)
    {
        if (!isAvailable) { return; }
        FirebaseAnalytics.LogEvent("stage_clear", "stage_" + stageNum, stageNum);
    }

    public void LogEvent_StageFailed(int stageNum)
    {
        if (!isAvailable) { return; }
        FirebaseAnalytics.LogEvent("stage_failed", "stage_" + stageNum, stageNum);
    }
}