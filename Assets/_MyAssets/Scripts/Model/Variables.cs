using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Variables : MonoBehaviour
{
    public static ScreenState screenState = ScreenState.Game;
    public static float speedX = 6f;
    public static float smoothTimeX = 0.01f;//0.05f
    public static float goalRate = 1.0f;
    public static bool isLaunchUIScene = false;
    public static bool isSkinReal = true;
    public static bool isStage30Sec = true;
    public static bool isZeroCameraPosX = true;
    public static bool isShowInterstitialOddLevel = true;
    public static bool isShowInterstitialBeforeRestartLevel = true;
    public static bool isHideRewardedAds = true;
}
