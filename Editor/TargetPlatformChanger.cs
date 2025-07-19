using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace Framework.Toolkits.ResKit
{
    [InitializeOnLoad]
    public class TargetPlatformChanger
    {
        static TargetPlatformChanger()
        {
            PlayerSettings.SetApiCompatibilityLevel(NamedBuildTarget.Standalone, ApiCompatibilityLevel.NET_Unity_4_8);
            
            // .unitypackage 开始导入
            AssetDatabase.importPackageStarted += packageName =>
            {
                
            };
            // .unitypackage 导入成功
            AssetDatabase.importPackageCompleted += packageName =>
            {
                PlayerSettings.SetApiCompatibilityLevel(NamedBuildTarget.Standalone, ApiCompatibilityLevel.NET_Unity_4_8);
            };
            // .unitypackage 取消导入
            AssetDatabase.importPackageCancelled += packageName =>
            {
                Debug.Log(packageName);
            };
            // .unitypackage 导入失败
            AssetDatabase.importPackageFailed += (packageName, errorMessage) =>
            {
                Debug.Log(packageName + " import failed: " + errorMessage);
            
                PlayerSettings.SetApiCompatibilityLevel(NamedBuildTarget.Standalone, ApiCompatibilityLevel.NET_Unity_4_8);
            };
        }
    }
}