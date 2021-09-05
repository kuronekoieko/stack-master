using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;
using System;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class MyBuildPostprocessor : IPreprocessBuildWithReport
{
    // 実行順
    public int callbackOrder { get { return 0; } }

    static string releaseBundleIdentifier;
    static string releaseBundleDisplayName;

    // ビルド前処理
    public void OnPreprocessBuild(BuildReport report)
    {
        Debug.Log("OnPreprocessBuild");
        releaseBundleIdentifier = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android);

        // ビルド時に一時的にPlayerSettingを変更し、ビルド後に戻す
        // androidでのポストプロセスが実装できなかったため、この方法ならOSに関わらず実装できる
        if (EditorUserBuildSettings.development)
        {
            releaseBundleDisplayName = PlayerSettings.productName;

            string dateName = DateTime.Today.Month.ToString("D2") + DateTime.Today.Day.ToString("D2");

            string debugBundleDisplayName = $"{dateName}_{releaseBundleDisplayName}";
            string debugBundleIdentifier = releaseBundleIdentifier + ".dev";

            PlayerSettings.productName = debugBundleDisplayName;
            // PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, debugBundleIdentifier);
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, debugBundleIdentifier);
        }
    }

    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
    {
        Debug.Log("OnPostProcessBuild buildTarget : " + buildTarget);
        if (EditorUserBuildSettings.development)
        {
            PlayerSettings.productName = releaseBundleDisplayName;
            // PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, releaseBundleIdentifier);
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, releaseBundleIdentifier);
        }
        // PostProcessBuild後の保存が自動でされず、gitの変更にPreprocessBuildの変更が出てしまうため
        AssetDatabase.SaveAssets();
    }

    static void IOS(BuildTarget buildTarget, string path)
    {
        if (buildTarget != BuildTarget.iOS) return;

        string projectPath = PBXProject.GetPBXProjectPath(path);

        PBXProject pbxProject = new PBXProject();
        pbxProject.ReadFromFile(projectPath);

        //Exception: Calling TargetGuidByName with name='Unity-iPhone' is deprecated.【解決策】
        //https://koujiro.hatenablog.com/entry/2020/03/16/050848
        string target = pbxProject.GetUnityMainTargetGuid();


        //pbxProject.AddCapability(target, PBXCapabilityType.InAppPurchase);

        // Plistの設定のための初期化
        var plistPath = Path.Combine(path, "Info.plist");
        var plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        plist.WriteToFile(plistPath);
        pbxProject.WriteToFile(projectPath);
    }

    static void Android(BuildTarget buildTarget, string path)
    {
        if (buildTarget != BuildTarget.Android) return;
        // https://qiita.com/ckazu/items/07dff39449e9f544b038
    }
}