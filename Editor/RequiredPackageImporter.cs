using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Framework.Toolkits.ResKit
{
    // [InitializeOnLoad]
    // public class RequiredPackageImporter
    // {
    //     static AddRequest Request;

    //     [MenuItem("Tools/ResKit/Download Required Packages")]
    //     static void AddRequiredPackage()
    //     {
    //         Debug.Log("Downloading required packages...");
    //         // Add a package to the project
    //         Request                  =  Client.Add("com.unity.addressables");
    //         EditorApplication.update += Progress;
    //         Debug.Log(Request.Status);
    //     }

    //     static void Progress()
    //     {
    //         if (Request.IsCompleted)
    //         {
    //             if (Request.Status == StatusCode.Success)
    //                 Debug.Log("Installed: " + Request.Result.packageId);
    //             else if (Request.Status >= StatusCode.Failure)
    //                 Debug.Log(Request.Error.message);

    //             EditorApplication.update -= Progress;
    //             AssetDatabase.Refresh();
    //         }
    //     }

    //     static RequiredPackageImporter()
    //     {
    //         AssetDatabase.ActiveRefreshImportMode = AssetDatabase.RefreshImportMode.InProcess;
         
    //         AddRequiredPackage();
            
    //         File.WriteAllText(Application.dataPath + "/RequiredPackageImporter.txt",
    //             "Required packages are being downloaded. Please wait until the process is complete.");
    //         // .unitypackage 开始导入
    //         AssetDatabase.importPackageStarted += packageName =>
    //         {
    //             AddRequiredPackage();
    //             File.WriteAllText(Application.dataPath + "/importPackageStarted.txt",
    //                 "Required packages are being downloaded. Please wait until the process is complete.");
    //         };
    //         // .unitypackage 导入成功
    //         AssetDatabase.importPackageCompleted += packageName =>
    //         {
    //             AddRequiredPackage();
    //             File.WriteAllText(Application.dataPath + "/importPackageCompleted.txt",
    //                 "Required packages are being downloaded. Please wait until the process is complete.");
    //         };
    //         // .unitypackage 取消导入
    //         AssetDatabase.importPackageCancelled += packageName =>
    //         {
    //             Debug.Log(packageName);
    //             File.WriteAllText(Application.dataPath + "/importPackageCancelled.txt",
    //                 "Required packages are being downloaded. Please wait until the process is complete.");
    //         };
    //         // .unitypackage 导入失败
    //         AssetDatabase.importPackageFailed += (packageName, errorMessage) =>
    //         {
    //             Debug.Log(packageName + " import failed: " + errorMessage);
    //             File.WriteAllText(Application.dataPath + "/importPackageFailed.txt",
    //                 "Required packages are being downloaded. Please wait until the process is complete.");
            
    //             AddRequiredPackage();
    //         };
    //     }
    // }
}