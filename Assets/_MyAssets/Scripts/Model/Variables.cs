using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Variables : MonoBehaviour
{
    public static ScreenState screenState = ScreenState.Game;
    public static int currentSceneBuildIndex
    {
        set { _currentSceneBuildIndex = Mathf.Clamp(value, 1, lastSceneBuildIndex); }
        get { return _currentSceneBuildIndex; }
    }
    private static int _currentSceneBuildIndex;
    public static int lastSceneBuildIndex => SceneManager.sceneCountInBuildSettings - 1;
}
