using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Runtime.CompilerServices;

public static class ScreenShotGenerator
{
    private const string Path = "ScreenShot/";
    [MenuItem("Capture ScreenShot/Capture ScreenShot")]
    private static void Capture()
    {
        var assetPath = string.Format(System.IO.Path.Combine(Path, "ScreenShot_{0}.png"),
            DateTime.Now.ToString("yyyyMMddHHmmss"));
        SafeCreateDirectory(Path);
        ScreenCapture.CaptureScreenshot(string.Format(assetPath));
        Debug.Log($"スクリーンショットを保存しました： {(Application.dataPath).Replace("Assets", "") + assetPath}");
    }

    private static DirectoryInfo SafeCreateDirectory(string path)
    {
        var fullPath = (Application.dataPath).Replace("Assets", "") + path;
        return Directory.Exists(fullPath) ? null : Directory.CreateDirectory(fullPath);
    }
}