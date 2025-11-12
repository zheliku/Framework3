// ------------------------------------------------------------
// @file       SamplesIntegrationInstaller.cs
// @brief
// @author     zheliku
// @Modified   2025-11-13 04:16:28
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Framework3.Editor
{
    /// <summary>
    /// 通用 Samples 安装器（多插件）：
    /// - 解析 package.json：name, displayName, version
    /// - 打开 Package Manager 到当前包
    /// - 扫描 Samples~/ 下任意版本的 unitypackage（匹配关键字）
    /// - 一键“模拟 UPM 导入 Sample”：复制到 Assets/Samples/&lt;DisplayName&gt;/… 并导入 .unitypackage
    /// - 支持“导入已安装 Samples 中的 unitypackage”
    /// - 多插件：通过 PluginSpec 定义匹配规则与存在性检测
    /// </summary>
    public static class SamplesIntegrationInstaller
    {
        // 公开只读：供 UI 使用
        public static string PackageName        { get; private set; }
        public static string PackageDisplayName { get; private set; }
        public static string PackageVersion     { get; private set; }
        public static string PackageRootPath    { get; private set; }

        private static bool   inited;
        private static string pkgRoot;              // 包的磁盘目录（含 package.json）
        private static string samplesRoot;          // Packages/.../Samples~
        private static string installedSamplesRoot; // Assets/Samples/<DisplayName>

        public static void InitPackageInfoIfNeeded(string customPkgName, string customDisplayName)
        {
            if (inited) return;
            inited = true;

            pkgRoot         = ResolveThisPackageDiskPath();
            PackageRootPath = pkgRoot;

            (PackageName, PackageDisplayName, PackageVersion) = ReadPackageInfos(pkgRoot);

            if (!string.IsNullOrEmpty(customPkgName)) PackageName            = customPkgName;
            if (!string.IsNullOrEmpty(customDisplayName)) PackageDisplayName = customDisplayName;

            samplesRoot          = string.IsNullOrEmpty(pkgRoot) ? null : Path.Combine(pkgRoot, "Samples~");
            installedSamplesRoot = GuessInstalledSampleRoot(PackageDisplayName);
        }

        // =============== 插件规格 & 检测 ===============

        public readonly struct PluginSpec
        {
            public string   DisplayName          { get; }
            public string[] Keywords             { get; }
            public string[] TypeNamesForPresence { get; }
            public string[] AssemblyNameHints    { get; }

            public PluginSpec(string displayName, string[] keywords, string[] typeNamesForPresence = null, string[] assemblyNameHints = null)
            {
                DisplayName          = displayName;
                Keywords             = keywords ?? Array.Empty<string>();
                TypeNamesForPresence = typeNamesForPresence ?? Array.Empty<string>();
                AssemblyNameHints    = assemblyNameHints ?? Array.Empty<string>();
            }
        }

        /// <summary> 判断插件是否“已安装”（通过类型名和程序集名的启发式检测） </summary>
        public static bool IsPluginPresent(in PluginSpec spec)
        {
            // 1) 类型名探测（最准确）
            foreach (var tname in spec.TypeNamesForPresence)
            {
                if (string.IsNullOrEmpty(tname)) continue;
                try
                {
                    if (Type.GetType(tname) != null) return true;
                } catch
                {
                    // ignored
                }
            }

            // 2) 程序集名探测
            if (spec.AssemblyNameHints != null && spec.AssemblyNameHints.Length > 0)
            {
                try
                {
                    var names = AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetName().Name).ToArray();
                    foreach (var hint in spec.AssemblyNameHints)
                        if (names.Any(n => n.IndexOf(hint, StringComparison.OrdinalIgnoreCase) >= 0))
                            return true;
                } catch
                {
                    // ignored
                }
            }

            return false;
        }

        // =============== UI 交互动作 ===============

        /// <summary> 打开 Package Manager 定位到本包 </summary>
        public static void OpenPackageManagerToThisPackage(string name, string displayName)
        {
            try
            {
                var uiAsm = AppDomain.CurrentDomain.GetAssemblies()
                   .FirstOrDefault(a => a.GetName().Name == "UnityEditor.PackageManager.UI");
                var winType = uiAsm?.GetType("UnityEditor.PackageManager.UI.Window");
                var open    = winType?.GetMethod("Open", new[] { typeof(string) });
                if (open != null && !string.IsNullOrEmpty(name))
                {
                    open.Invoke(null, new object[] { name });
                    return;
                }
            } catch
            { /* ignore */
            }

            EditorApplication.ExecuteMenuItem("Window/Package Manager");
            EditorUtility.DisplayDialog(
                "请在 Package Manager 中导入 Samples",
                "已打开 Package Manager。\n\n在搜索框输入：\n" +
                (string.IsNullOrEmpty(name) ? displayName : name) +
                "\n找到本包后，切换到 Samples 面板，点击 “Import”。",
                "好");
        }

        /// <summary> 导入本包所有 Samples（把 Samples~ 的各子目录复制到 Assets/Samples/&lt;DisplayName&gt;/…） </summary>
        public static void ImportAllSamplesToAssets()
        {
            if (!EnsureSamplesRoot()) return;

            foreach (var dir in Directory.EnumerateDirectories(samplesRoot, "*", SearchOption.TopDirectoryOnly))
            {
                CopySampleFolderToAssets(dir);
            }
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("完成", "已将本包所有 Samples 复制到 Assets/Samples 下。", "好");
        }

        /// <summary> 针对某插件：从 Samples~ 自动安装（复制对应样本目录并导入其中 .unitypackage） </summary>
        public static void InstallPluginFromSamples(in PluginSpec spec)
        {
            if (!EnsureSamplesRoot()) return;

            var sampleDirs = FindCandidateSampleDirs(spec, samplesRoot);
            if (sampleDirs.Count == 0)
            {
                EditorUtility.DisplayDialog("未找到", $"Samples~ 下没有与 “{spec.DisplayName}” 匹配的样本目录。请检查关键字或样本命名。", "好");
                return;
            }

            // 选一个最佳目录（含最高版本 unitypackage 优先）
            var best = ChooseBestSampleDir(sampleDirs);

            // 复制到 Assets/Samples/…
            var dst = CopySampleFolderToAssets(best.Dir);

            // 在目标样本目录里找 unitypackage 并导入（如果有）
            string pkg = FindBestUnitypackageIn(best.Dir);
            if (!string.IsNullOrEmpty(pkg))
            {
                ImportUnityPackageViaCache(pkg);
            }
            else
            {
                // 有的样本不含 unitypackage，只复制资源
                EditorUtility.DisplayDialog("已复制样本", $"已复制样本到：\n{dst}\n\n如样本包含自定义安装步骤，请按样本说明进行下一步。", "好");
            }
        }

        /// <summary> 针对某插件：从已安装的 Samples（Assets/Samples/…）里导入 unitypackage </summary>
        public static void ImportFromInstalledSamples(in PluginSpec spec)
        {
            string root = installedSamplesRoot;
            if (string.IsNullOrEmpty(root) || !Directory.Exists(root))
            {
                EditorUtility.DisplayDialog("未找到", "未找到已安装的 Samples 目录。请先在 Package Manager 导入 Samples 或使用“一键安装”。", "好");
                return;
            }

            // 在已安装的 Samples 下按关键字匹配某插件的样本目录
            var sampleDirs = FindCandidateSampleDirs(spec, root);
            if (sampleDirs.Count == 0)
            {
                EditorUtility.DisplayDialog("未找到", $"已安装的 Samples 中找不到 “{spec.DisplayName}” 的样本。", "好");
                return;
            }

            var    best = ChooseBestSampleDir(sampleDirs);
            string pkg  = FindBestUnitypackageIn(best.Dir);

            if (!string.IsNullOrEmpty(pkg))
                ImportUnityPackageViaCache(pkg);
            else
                EditorUtility.DisplayDialog("未找到", "该样本目录中未找到 .unitypackage。", "好");
        }

        /// <summary> 重新扫描 Samples~（用于刷新缓存或新放入的 unitypackage） </summary>
        public static void RescanSamples()
        {
            // 实际扫描行为在 Install/Import 时即时进行，这里仅提示刷新
            EditorUtility.DisplayDialog("已触发", "重新扫描将在下一次操作时执行（无需额外缓存）。", "好");
        }

        // =============== 内部：样本与包工具 ===============

        private static bool EnsureSamplesRoot()
        {
            if (string.IsNullOrEmpty(samplesRoot) || !Directory.Exists(samplesRoot))
            {
                EditorUtility.DisplayDialog("未找到 Samples~", "未在当前包找到 Samples~ 目录。", "好");
                return false;
            }
            return true;
        }

        private static List<SampleDirInfo> FindCandidateSampleDirs(in PluginSpec spec, string searchRoot)
        {
            var list = new List<SampleDirInfo>();
            foreach (var dir in Directory.EnumerateDirectories(searchRoot, "*", SearchOption.AllDirectories))
            {
                string name = Path.GetFileName(dir);
                // 名称或路径含任一关键字即可认为是候选
                if (spec.Keywords.Any(k => name.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    spec.Keywords.Any(k => dir.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    list.Add(new SampleDirInfo
                    {
                        Dir     = dir.Replace('\\', '/'),
                        BestPkg = FindBestUnitypackageIn(dir),
                        MTime   = Directory.GetLastWriteTimeUtc(dir)
                    });
                }
            }
            return list;
        }

        private static SampleDirInfo ChooseBestSampleDir(List<SampleDirInfo> dirs)
        {
            return dirs
               .OrderByDescending(x => !string.IsNullOrEmpty(x.BestPkg))
               .ThenByDescending(x => ParseVersionSafe(Path.GetFileNameWithoutExtension(x.BestPkg ?? "")))
               .ThenByDescending(x => x.MTime)
               .First();
        }

        private struct SampleDirInfo
        {
            public string   Dir;
            public string   BestPkg;
            public DateTime MTime;
        }

        private static string CopySampleFolderToAssets(string sampleDir)
        {
            string root = installedSamplesRoot;
            Directory.CreateDirectory(root);
            string dst = Path.Combine(root, Path.GetFileName(sampleDir));
            if (Directory.Exists(dst)) Directory.Delete(dst, true);

            // 复制
            foreach (string dir in Directory.GetDirectories(sampleDir, "*", SearchOption.AllDirectories))
            {
                string rel = dir.Substring(sampleDir.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                Directory.CreateDirectory(Path.Combine(dst, rel));
            }
            foreach (string file in Directory.GetFiles(sampleDir, "*", SearchOption.AllDirectories))
            {
                string rel = file.Substring(sampleDir.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                string to  = Path.Combine(dst, rel);
                Directory.CreateDirectory(Path.GetDirectoryName(to) ?? string.Empty);
                File.Copy(file, to, true);
            }

            AssetDatabase.Refresh();
            return dst.Replace('\\', '/');
        }

        private static string FindBestUnitypackageIn(string dir)
        {
            var candidates = Directory.EnumerateFiles(dir, "*.unitypackage", SearchOption.AllDirectories)
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

        private static void ImportUnityPackageViaCache(string srcPath)
        {
            if (!File.Exists(srcPath))
            {
                EditorUtility.DisplayDialog("导入失败", "文件不存在：\n" + srcPath, "好");
                return;
            }
            try
            {
                string cacheDir = Path.Combine(Application.dataPath, "Framework3_Internal", "Cache");
                Directory.CreateDirectory(cacheDir);
                string dstPath = Path.Combine(cacheDir, Path.GetFileName(srcPath));
                File.Copy(srcPath, dstPath, true);

                AssetDatabase.ImportPackage(dstPath, /*interactive*/ false);
                EditorUtility.DisplayDialog("导入已开始",
                    "已触发 .unitypackage 导入。导入完成后会域重载。", "好");
            } catch (Exception e)
            {
                Debug.LogException(e);
                EditorUtility.DisplayDialog("导入异常", e.Message, "好");
            }
        }

        private static string ResolveThisPackageDiskPath()
        {
            try
            {
                // 通过这个工具类自身定位当前包根
                string[] guids = AssetDatabase.FindAssets("t:Script SamplesIntegrationInstaller");
                foreach (var guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
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
                    string candidate = Path.Combine(fullName, "Packages", "Framework3");
                    if (Directory.Exists(candidate)) return candidate.Replace('\\', '/');
                }
                return null;
            } catch { return null; }
        }

        private static (string name, string display, string version) ReadPackageInfos(string pkgRoot)
        {
            try
            {
                string json = File.ReadAllText(Path.Combine(pkgRoot, "package.json"));

                string Get(string key)
                {
                    var m = Regex.Match(json, $"\"{Regex.Escape(key)}\"\\s*:\\s*\"([^\"]+)\"");
                    return m.Success ? m.Groups[1].Value : null;
                }

                var name    = Get("name");
                var display = Get("displayName") ?? "Framework3";
                var version = Get("version");
                return (name, display, version);
            } catch
            {
                return (null, "Framework3", null);
            }
        }

        private static string GuessInstalledSampleRoot(string pkgDisplayName)
        {
            string root = Path.Combine(Application.dataPath, "Samples", string.IsNullOrEmpty(pkgDisplayName) ? "Framework3" : pkgDisplayName);
            return root.Replace('\\', '/');
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
    }
}