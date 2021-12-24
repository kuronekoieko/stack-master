using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using DG.Tweening;

// Unityでモバイル端末のバイブレーションをコントロールする
// https://qiita.com/mrhdms/items/3201baf851a91875fa29
// Androidのバイブレーションの使い方
// https://qiita.com/k-ysd/items/cfc8fc59760f7694ea54
// 参考になりそうなソースコード
// https://gist.github.com/munkbusiness/9e0a7d41bb9c0eb229fd8f2313941564
// androidレファレンス
// https://developer.android.com/reference/android/os/Vibrator
// やはりUNITYでAndroidプラグインを作るにはAndroidJavaClassが便利だ
// https://qiita.com/YukiMiyatake/items/c8c2ef396fbf4457ba4f
// AndroidJavaObjectリファレンス
// https://docs.unity3d.com/ja/current/ScriptReference/AndroidJavaObject.html

public class VibrateManager
{

    /*
#if UNITY_EDITOR
    //何もしない
#elif UNITY_IOS
    [DllImport ("__Internal")]
    static extern void _playSystemSound(int n);
#elif UNITY_ANDROID
    // パーミッションの設定が必要なため
    // http://smartgames.hatenablog.com/entry/2019/02/21/232941
    static AndroidJavaClass uniVibration = new AndroidJavaClass("net.sanukin.vibration.UniVibration");
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
    static AndroidJavaClass vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
#endif

    public static float soundsSec = 0.045f;
    public static float limitSec = 0.095f;
    static bool isLimitVibration;

    public static void Play()
    {
        // if (SaveData.i.isOffVibration) return;
        int systemSoundID = 1520;
#if UNITY_EDITOR
        //何もしない
#elif UNITY_IOS
        _playSystemSound(systemSoundID);
#elif UNITY_ANDROID

 if (isLimitVibration) { return; }

        isLimitVibration = true;

        DOVirtual.DelayedCall(limitSec, () =>
        {
            isLimitVibration = false;
        });

int msec = (int)(soundsSec * 1000f);
//uniVibration.CallStatic("vibrate", msec);



if(getSDKInt()>=26)
        {
            AndroidJavaObject vibrationEffect = vibrationEffectClass.CallStatic<AndroidJavaObject>("createOneShot",msec, 5);
            uniVibration.CallStatic("vibrate",vibrationEffect);
        }
        else
        {
            uniVibration.CallStatic("vibrate", msec);
        }



        //vibrator.CallStatic("vibrate", msec);

       
#endif

    }

    private static int getSDKInt()
    {
        using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
        {
            return version.GetStatic<int>("SDK_INT");
        }
    }

    */
}
