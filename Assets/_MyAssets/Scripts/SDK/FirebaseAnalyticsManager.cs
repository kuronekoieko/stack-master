using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase.Analytics;
using DG.Tweening;

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

    public void LogEvent(string title, string parameterName)
    {
        StartCoroutine(LogEventAsync(title, parameterName, parameterName));
    }

    public void LogEvent_level(string title)
    {
        string level = "level_" + StageTransManager.i.Level.ToString("000");
        StartCoroutine(LogEventAsync(title, "level", level));
    }

    IEnumerator LogEventAsync(string title, string parameterName, string parameterValue)
    {
        while (!isAvailable)
        {
            yield return 0;
        }
        FirebaseAnalytics.LogEvent(title, parameterName, parameterValue);
    }
}