// ------------------------------------------------------------
// @file       Framework3Welcome.cs
// @brief
// @author     zheliku
// @Modified   2025-11-13 04:15:52
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------

using System;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace Framework3.Editor
{
    /// <summary>
    /// 首次安装弹窗（或可通过菜单手动打开）
    /// - 展示欢迎信息
    /// - 检测&引导安装多种插件（Odin / BG Database / Events Pro）
    /// - 支持：打开 Package Manager 到本包、从 Samples~ 自动安装（复制到 Assets/Samples 并导入 .unitypackage）
    /// </summary>
    public class Framework3Welcome : EditorWindow
    {
        private const  string MenuPath      = "Framework3/Welcome";
        private const  string SeenKeyPrefix = "Framework3.Welcome.Seen."; // + packageName + "@" + version
        private static string keyThisVersion;                            // 每个版本只弹一次

        // 插件“规格表”
        private SamplesIntegrationInstaller.PluginSpec[] _plugins;

        // UI 状态
        private Vector2 _scroll;

        [MenuItem(MenuPath, priority = 0)]
        public static void ShowWindow()
        {
            // if (!ShouldPopForThisVersion()) return;

            var win = GetWindow<Framework3Welcome>(true, "Welcome to Framework3", true);
            win.minSize = new Vector2(760, 520);
            win.ShowUtility();

            MarkSeenForThisVersion();
        }

        [InitializeOnLoadMethod]
        private static void OnLoad()
        {
            if (ShouldPopForThisVersion())
            {
                // 等 Editor 稳定后再弹，避免导入期干扰
                EditorApplication.update += DelayedOpen;
            }
        }

        private static void DelayedOpen()
        {
            EditorApplication.update -= DelayedOpen;
            ShowWindow();
        }

        private static bool ShouldPopForThisVersion()
        {
            SamplesIntegrationInstaller.InitPackageInfoIfNeeded(null, null); // 初始化包信息
            keyThisVersion = SeenKeyPrefix + SamplesIntegrationInstaller.PackageName + "@" + SamplesIntegrationInstaller.PackageVersion;
            return !EditorPrefs.GetBool(keyThisVersion, false);
        }
        private static void MarkSeenForThisVersion()
        {
            if (string.IsNullOrEmpty(keyThisVersion))
                keyThisVersion = SeenKeyPrefix + SamplesIntegrationInstaller.PackageName + "@" + SamplesIntegrationInstaller.PackageVersion;

            EditorPrefs.SetBool(keyThisVersion, true);
        }

        private void OnEnable()
        {
            titleContent = new GUIContent("Welcome to Framework3");

            SamplesIntegrationInstaller.InitPackageInfoIfNeeded(null, null);

            // 配置多插件支持：名称、关键字（用于检测与 Samples 匹配）、可选“类型/程序集”探测器（提高准确性）
            _plugins = new[]
            {
                new SamplesIntegrationInstaller.PluginSpec(
                    displayName: "Odin Inspector",
                    keywords: new[] { "odin", "sirenix" },
                    typeNamesForPresence: new[] { "Sirenix.OdinInspector.InlineEditorAttribute, Sirenix.OdinInspector.Attributes" },
                    assemblyNameHints: new[] { "Sirenix.OdinInspector" }
                ),
                new SamplesIntegrationInstaller.PluginSpec(
                    displayName: "BG Database",
                    keywords: new[] { "bg database", "bgdatabase", "brain games database" }, // 关键字可按你 samples 实际命名调整
                    typeNamesForPresence: new[] { "BGDatabase.BGRepo, BGDatabase" },
                    assemblyNameHints: new[] { "BGDatabase" }
                ),
                new SamplesIntegrationInstaller.PluginSpec(
                    displayName: "Events Pro",
                    keywords: new[] { "events pro", "event pro" }, // 同上，可按实际样本名称调整
                    typeNamesForPresence: new[] { "EventsPro.EventBus, EventsPro" },
                    assemblyNameHints: new[] { "EventsPro" }
                ),
            };
        }

        private void OnGUI()
        {
            // 顶部欢迎
            GUILayout.Space(8);
            GUILayout.Label("欢迎使用 Framework3 🎉", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "这是一次性欢迎界面。\n" +
                "我们检测到以下可选插件。你可以通过 Package Manager 的 Samples 导入，或使用下面的“一键安装（从 Samples~ 复制并导入）”。",
                MessageType.Info);

            // 包信息与全局操作
            DrawPackageSection();

            GUILayout.Space(8);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            // 插件列表
            _scroll = EditorGUILayout.BeginScrollView(_scroll);
            foreach (var p in _plugins)
                DrawPluginRow(p);
            EditorGUILayout.EndScrollView();

            GUILayout.FlexibleSpace();

            using(new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button("再次显示本欢迎界面（本版本）", GUILayout.Height(24)))
                {
                    // 清除“已看过”
                    var key = SeenKeyPrefix + SamplesIntegrationInstaller.PackageName + "@" + SamplesIntegrationInstaller.PackageVersion;
                    EditorPrefs.DeleteKey(key);
                    EditorUtility.DisplayDialog("已重置", "下次重新打开 Unity 或导入包时将再次弹出本欢迎界面。", "好");
                }
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("关闭", GUILayout.Height(24), GUILayout.Width(120)))
                    Close();
            }
        }

        private void DrawPackageSection()
        {
            using(new EditorGUILayout.VerticalScope("box"))
            {
                GUILayout.Label("包信息", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("Name", SamplesIntegrationInstaller.PackageName ?? "(unknown)");
                EditorGUILayout.LabelField("Display Name", SamplesIntegrationInstaller.PackageDisplayName ?? "(Framework3)");
                EditorGUILayout.LabelField("Version", SamplesIntegrationInstaller.PackageVersion ?? "(unknown)");

                GUILayout.Space(4);
                using(new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("打开 Package Manager（定位本包）", GUILayout.Height(24)))
                        SamplesIntegrationInstaller.OpenPackageManagerToThisPackage(
                            SamplesIntegrationInstaller.PackageName,
                            SamplesIntegrationInstaller.PackageDisplayName
                        );

                    if (GUILayout.Button("导入本包所有 Samples（自动）", GUILayout.Height(24)))
                    {
                        SamplesIntegrationInstaller.ImportAllSamplesToAssets();
                    }
                }
            }
        }

        private void DrawPluginRow(SamplesIntegrationInstaller.PluginSpec spec)
        {
            bool present = SamplesIntegrationInstaller.IsPluginPresent(spec);
            using(new EditorGUILayout.VerticalScope("box"))
            {
                using(new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label(spec.DisplayName, EditorStyles.boldLabel);
                    GUILayout.FlexibleSpace();

                    // 状态标签
                    var style = new GUIStyle(EditorStyles.helpBox)
                    {
                        alignment   = TextAnchor.MiddleCenter,
                        fontSize    = 11,
                        fixedHeight = 22
                    };
                    var color = present ? new Color(0.25f, 0.65f, 0.25f) : new Color(0.8f, 0.4f, 0.0f);
                    var old   = GUI.backgroundColor;
                    GUI.backgroundColor = color;
                    GUILayout.Label(present ? "已安装" : "未安装", style, GUILayout.Width(70));
                    GUI.backgroundColor = old;
                }

                EditorGUILayout.HelpBox(
                    present
                        ? "已检测到此插件，无需操作。"
                        : "未检测到此插件。你可以在 Package Manager 中导入本包 Samples，或点击“一键安装”从 Samples~ 自动复制并导入。",
                    present ? MessageType.Info : MessageType.Warning);

                using(new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("打开 Package Manager（到本包）", GUILayout.Height(22)))
                        SamplesIntegrationInstaller.OpenPackageManagerToThisPackage(
                            SamplesIntegrationInstaller.PackageName, SamplesIntegrationInstaller.PackageDisplayName);

                    if (GUILayout.Button("一键安装（从 Samples~ 复制&导入）", GUILayout.Height(22)))
                        SamplesIntegrationInstaller.InstallPluginFromSamples(spec);

                    if (GUILayout.Button("扫描已导入的 Samples 并导入 .unitypackage", GUILayout.Height(22)))
                        SamplesIntegrationInstaller.ImportFromInstalledSamples(spec);

                    if (GUILayout.Button("重新扫描 Samples~", GUILayout.Height(22)))
                        SamplesIntegrationInstaller.RescanSamples();
                }

                // 关键字显示（便于你检查匹配规则）
                var kws = string.Join(", ", spec.Keywords.Select(k => $"\"{k}\""));
                EditorGUILayout.LabelField("匹配关键词（Samples 文件/目录/包名）", kws);
            }
        }
    }
}