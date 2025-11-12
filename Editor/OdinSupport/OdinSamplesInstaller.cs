// ------------------------------------------------------------
// @file       OdinSamplesInstaller.cs
// @brief
// @author     zheliku
// @Modified   2025-11-13 03:49:19
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------

namespace Framework3.Editor
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    ///     负责：
    ///     - 检测是否已安装 Odin；
    ///     - 解析当前 UPM 包根、package.json 的 name/displayName；
    ///     - 在 Samples~/ 下扫描任意版本的 Odin .unitypackage（挑最高版本/最新）；
    ///     - 一键“模拟 UPM Import Sample”（复制到 Assets/Samples/&lt;displayName&gt;/...）并导入 unitypackage；
    ///     - 打开 Package Manager 定位到当前包。
    /// </summary>
    public static class OdinSamplesInstaller
    {
        private static bool   initialized;
        private static string pkgRoot;
        private static string pkgName;
        private static string pkgDisplayName;

        private static string bestUnityPackageFromSamples; // Packages/…/Samples~/…/*.unitypackage
        private static string installedSamplesRoot;        // Assets/Samples/<displayName>

        public static void InitPackageInfoIfNeeded(string customPkgName = null, string customDisplay = null)
        {
            if (initialized) return;
            initialized = true;

            pkgRoot                    = ResolveThisPackageDiskPath();
            (pkgName, pkgDisplayName) = ReadPackageNameAndDisplayName(pkgRoot);

            if (!string.IsNullOrEmpty(customPkgName)) pkgName        = customPkgName;
            if (!string.IsNullOrEmpty(customDisplay)) pkgDisplayName = customDisplay;

            bestUnityPackageFromSamples = FindBestOdinUnitypackageUnderSamples(pkgRoot);
            installedSamplesRoot        = GuessInstalledSampleRoot(pkgDisplayName);
        }

        public static bool IsOdinPresent()
        {
            try
            {
                var t = Type.GetType("Sirenix.OdinInspector.InlineEditorAttribute, Sirenix.OdinInspector.Attributes");
                if (t != null) return true;
                return AppDomain.CurrentDomain.GetAssemblies()
                   .Any(a => a.GetName().Name.StartsWith("Sirenix.OdinInspector", StringComparison.OrdinalIgnoreCase));
            } catch { return false; }
        }

        public static void DrawInstallGuideUI(string windowTitle)
        {
            GUILayout.Space(6);
            EditorGUILayout.HelpBox(
                "未检测到 Odin Inspector。\n\n" +
                $"此窗口（{windowTitle}）的完整可视化功能依赖 Odin。\n" +
                "你可以：\n" +
                "1) 打开 Package Manager 并在本包的 Samples 面板点击 Import；\n" +
                "2) 或用“一键从 Samples 安装”，自动复制 Sample 到 Assets/Samples 并导入 .unitypackage；\n" +
                "3) 若你已在 Package Manager 导入过 Sample，可点“扫描并导入已安装 Sample 中的 Odin”。",
                MessageType.Info);

            GUILayout.Space(6);

            using(new EditorGUILayout.VerticalScope("box"))
            {
                GUILayout.Label("选项 A：打开 Package Manager（推荐）", EditorStyles.boldLabel);
                using(new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("打开 Package Manager", GUILayout.Height(24)))
                        OpenPackageManagerToThisPackage(pkgName, pkgDisplayName);
                    if (GUILayout.Button("复制包名", GUILayout.Height(24)))
                    {
                        EditorGUIUtility.systemCopyBuffer = pkgName ?? pkgDisplayName ?? "Framework3";
                        ShowTempNotification("已复制包名到剪贴板");
                    }
                }
                EditorGUILayout.LabelField("包名（用以搜索定位）", pkgName ?? "(unknown)");
            }

            GUILayout.Space(6);

            using(new EditorGUILayout.VerticalScope("box"))
            {
                GUILayout.Label("选项 B：一键从 Samples~ 安装 Odin（自动）", EditorStyles.boldLabel);

                if (!string.IsNullOrEmpty(bestUnityPackageFromSamples) && File.Exists(bestUnityPackageFromSamples))
                {
                    EditorGUILayout.LabelField("检测到 Samples~ 包：", Path.GetFileName(bestUnityPackageFromSamples));
                    using(new EditorGUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button("一键安装（复制 Samples 并导入 Odin）", GUILayout.Height(26)))
                            InstallOdinFromSamplesAuto();
                        if (GUILayout.Button("在文件夹中显示", GUILayout.Height(26)))
                            EditorUtility.RevealInFinder(bestUnityPackageFromSamples);
                        if (GUILayout.Button("重新扫描 Samples~", GUILayout.Height(26)))
                        {
                            bestUnityPackageFromSamples = FindBestOdinUnitypackageUnderSamples(pkgRoot, true);
                            ShowTempNotification(bestUnityPackageFromSamples != null ? "已重新扫描" : "未找到 Odin unitypackage");
                        }
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("未在本包的 Samples~/… 中找到 Odin 的 .unitypackage。\n请确认已将 Odin unitypackage 放在 Samples~/Odin Inspector/ 下。", MessageType.Warning);
                    if (GUILayout.Button("重新扫描 Samples~", GUILayout.Height(24)))
                    {
                        bestUnityPackageFromSamples = FindBestOdinUnitypackageUnderSamples(pkgRoot, true);
                        ShowTempNotification(bestUnityPackageFromSamples != null ? "已重新扫描" : "未找到 Odin unitypackage");
                    }
                }
            }

            GUILayout.Space(6);

            using(new EditorGUILayout.VerticalScope("box"))
            {
                GUILayout.Label("选项 C：扫描并导入【已安装的 Sample】中的 Odin", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("已安装的 Sample 位置（猜测）：", installedSamplesRoot ?? "(未找到)");
                if (GUILayout.Button("扫描并导入已安装 Sample 中的 Odin", GUILayout.Height(24)))
                {
                    var pkg = FindBestOdinUnitypackageUnderInstalledSamples(installedSamplesRoot);
                    if (!string.IsNullOrEmpty(pkg)) ImportUnityPackage(pkg);
                    else
                        EditorUtility.DisplayDialog("未找到",
                            "未在已安装的 Samples 目录中找到 Odin 的 .unitypackage。\n" +
                            "请先在 Package Manager 中 Import 本包的 Samples，或使用“一键安装”。", "好");
                }
            }

            GUILayout.FlexibleSpace();

            using(new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Odin 官网", GUILayout.Height(22)))
                    Application.OpenURL("https://odininspector.com/");
                if (GUILayout.Button("Asset Store", GUILayout.Height(22)))
                    Application.OpenURL("https://assetstore.unity.com/packages/tools/utilities/odin-inspector-and-serializer-89041");
                GUILayout.FlexibleSpace();
            }
        }

        // -------- 一键安装主流程 --------
        public static void InstallOdinFromSamplesAuto()
        {
            if (string.IsNullOrEmpty(pkgRoot))
            {
                EditorUtility.DisplayDialog("失败", "未能定位当前包根目录。", "好");
                return;
            }

            var samplesFolder = Path.Combine(pkgRoot, "Samples~");
            if (!Directory.Exists(samplesFolder))
            {
                EditorUtility.DisplayDialog("失败", "未找到 Samples~ 目录。", "好");
                return;
            }

            var candidateDirs = Directory.EnumerateDirectories(samplesFolder, "*", SearchOption.AllDirectories)
               .Where(d =>
                {
                    var name = Path.GetFileName(d);
                    return name.IndexOf("odin", StringComparison.OrdinalIgnoreCase) >= 0
                        || name.IndexOf("sirenix", StringComparison.OrdinalIgnoreCase) >= 0;
                })
               .ToList();

            if (candidateDirs.Count == 0)
            {
                EditorUtility.DisplayDialog("失败", "未找到包含 Odin 的 Sample 文件夹。", "好");
                return;
            }

            var bestDir = candidateDirs
               .Select(dir => new
                {
                    Dir     = dir,
                    BestPkg = FindBestOdinUnitypackage(dir),
                    MTime   = Directory.GetLastWriteTimeUtc(dir)
                })
               .OrderByDescending(x => !string.IsNullOrEmpty(x.BestPkg))
               .ThenByDescending(x => ParseVersionSafe(Path.GetFileNameWithoutExtension(x.BestPkg ?? "")))
               .ThenByDescending(x => x.MTime)
               .First().Dir;

            var dstRoot = GuessInstalledSampleRoot(pkgDisplayName);
            Directory.CreateDirectory(dstRoot);
            var dst = Path.Combine(dstRoot, Path.GetFileName(bestDir));
            if (Directory.Exists(dst)) Directory.Delete(dst, true);
            CopyDir(bestDir, dst);
            AssetDatabase.Refresh();

            var pkg = FindBestOdinUnitypackage(dst);
            if (!string.IsNullOrEmpty(pkg)) ImportUnityPackage(pkg);
            else EditorUtility.DisplayDialog("注意", "已复制 Sample，但未在该 Sample 中发现 Odin 的 .unitypackage。", "好");
        }

        // -------- UI/PM/路径/扫描 辅助 --------

        private static void ShowTempNotification(string msg)
        {
            EditorWindow.focusedWindow?.ShowNotification(new GUIContent(msg));
        }

        private static string ResolveThisPackageDiskPath()
        {
            try
            {
                // 找到任一引用此工具类的脚本或回溯到 package.json
                var guids = AssetDatabase.FindAssets("t:Script OdinSamplesInstaller");
                foreach (var guid in guids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    if (string.IsNullOrEmpty(path)) continue;
                    var dir = new DirectoryInfo(Path.GetDirectoryName(path) ?? string.Empty);
                    while (dir != null && dir.Name != "Assets")
                    {
                        if (File.Exists(Path.Combine(dir.FullName, "package.json")))
                            return dir.FullName.Replace('\\', '/');
                        dir = dir.Parent;
                    }
                }
                // 兜底猜测
                var fullName = Directory.GetParent(Application.dataPath)?.FullName;
                if (fullName != null)
                {
                    var candidate = Path.Combine(fullName, "Packages", "Framework3");
                    if (Directory.Exists(candidate)) return candidate.Replace('\\', '/');
                }
                return null;
            } catch { return null; }
        }

        private static (string pkgName, string displayName) ReadPackageNameAndDisplayName(string pkgRoot)
        {
            try
            {
                var json = File.ReadAllText(Path.Combine(pkgRoot, "package.json"));

                string GetValue(string key)
                {
                    var m = Regex.Match(json, $"\"{Regex.Escape(key)}\"\\s*:\\s*\"([^\"]+)\"");
                    return m.Success ? m.Groups[1].Value : null;
                }

                return (GetValue("name"), GetValue("displayName") ?? "Framework3");
            } catch { return (null, "Framework3"); }
        }

        private static Version ParseVersionSafe(string nameNoExt)
        {
            if (string.IsNullOrEmpty(nameNoExt)) return new Version(0, 0);
            var m = Regex.Match(nameNoExt, @"(?<!\d)(\d+(?:\.\d+){1,3})(?!\d)");
            if (m.Success && Version.TryParse(m.Groups[1].Value, out var v)) return v;
            m = Regex.Match(nameNoExt, @"(?<!\d)(\d+\.\d+\.\d+)");
            if (m.Success && Version.TryParse(m.Groups[1].Value, out v)) return v;
            return new Version(0, 0);
        }

        private static string FindBestOdinUnitypackageUnderSamples(string pkgRoot, bool forceRescan = false)
        {
            if (string.IsNullOrEmpty(pkgRoot) || !Directory.Exists(pkgRoot)) return null;
            var samplesDir = Path.Combine(pkgRoot, "Samples~");
            if (!Directory.Exists(samplesDir)) return null;
            return FindBestOdinUnitypackage(samplesDir);
        }

        private static string FindBestOdinUnitypackage(string rootDir)
        {
            var candidates = Directory.EnumerateFiles(rootDir, "*.unitypackage", SearchOption.AllDirectories)
               .Where(p =>
                {
                    var file = Path.GetFileName(p);
                    return file.IndexOf("odin", StringComparison.OrdinalIgnoreCase) >= 0
                        || file.IndexOf("sirenix", StringComparison.OrdinalIgnoreCase) >= 0;
                })
               .Select(p => new
                {
                    Path  = p.Replace('\\', '/'),
                    Ver   = ParseVersionSafe(Path.GetFileNameWithoutExtension(p)),
                    MTime = File.GetLastWriteTimeUtc(p)
                })
               .OrderByDescending(x => x.Ver)
               .ThenByDescending(x => x.MTime)
               .Select(x => x.Path)
               .ToList();

            return candidates.FirstOrDefault();
        }

        private static string GuessInstalledSampleRoot(string pkgDisplayName)
        {
            var root = Path.Combine(Application.dataPath, "Samples", string.IsNullOrEmpty(pkgDisplayName) ? "Framework3" : pkgDisplayName);
            return root.Replace('\\', '/');
        }

        private static string FindBestOdinUnitypackageUnderInstalledSamples(string installedRoot)
        {
            if (string.IsNullOrEmpty(installedRoot) || !Directory.Exists(installedRoot)) return null;
            return FindBestOdinUnitypackage(installedRoot);
        }

        private static void CopyDir(string src, string dst)
        {
            foreach (var dir in Directory.GetDirectories(src, "*", SearchOption.AllDirectories))
            {
                var rel = dir.Substring(src.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                Directory.CreateDirectory(Path.Combine(dst, rel));
            }
            foreach (var file in Directory.GetFiles(src, "*", SearchOption.AllDirectories))
            {
                var rel = file.Substring(src.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                var to  = Path.Combine(dst, rel);
                Directory.CreateDirectory(Path.GetDirectoryName(to) ?? string.Empty);
                File.Copy(file, to, true);
            }
        }

        public static void ImportUnityPackage(string srcPath)
        {
            if (!File.Exists(srcPath))
            {
                EditorUtility.DisplayDialog("导入失败", "文件不存在：\n" + srcPath, "好");
                return;
            }
            try
            {
                var cacheDir = Path.Combine(Application.dataPath, "Framework3_Internal", "Cache");
                Directory.CreateDirectory(cacheDir);
                var dstPath = Path.Combine(cacheDir, Path.GetFileName(srcPath));
                File.Copy(srcPath, dstPath, true);

                AssetDatabase.ImportPackage(dstPath, /*interactive*/ false);
                EditorUtility.DisplayDialog("导入已开始",
                    "已触发 Odin .unitypackage 导入。导入完成后会域重载。\n若窗口未自动关闭，可手动关闭。", "好");
            } catch (Exception e)
            {
                Debug.LogException(e);
                EditorUtility.DisplayDialog("导入异常", e.Message, "好");
            }
        }

        public static void OpenPackageManagerToThisPackage(string pkgName, string displayName)
        {
            try
            {
                var uiAsm = AppDomain.CurrentDomain.GetAssemblies()
                   .FirstOrDefault(a => a.GetName().Name == "UnityEditor.PackageManager.UI");
                var winType = uiAsm?.GetType("UnityEditor.PackageManager.UI.Window");
                var open    = winType?.GetMethod("Open", new[] { typeof(string) });
                if (open != null && !string.IsNullOrEmpty(pkgName))
                {
                    open.Invoke(null, new object[] { pkgName });
                    return;
                }
            } catch
            { /* ignore */
            }

            EditorApplication.ExecuteMenuItem("Window/Package Manager");
            EditorUtility.DisplayDialog(
                "请在 Package Manager 中导入 Samples",
                "已打开 Package Manager。\n\n在搜索框输入：\n" +
                (string.IsNullOrEmpty(pkgName) ? displayName : pkgName) +
                "\n找到本包后，切换到 Samples 面板，点击 “Import”。",
                "好");
        }
    }
}