// ------------------------------------------------------------
// @file       PlayerPrefsWindow.cs
// @brief      Odin-aware：未装 Odin 显示引导；已装照常使用 Odin 界面/逻辑
// @author     zheliku
// @Modified   2025-06-10 01:36:18
// ------------------------------------------------------------

namespace Framework3.Toolkits.BindableKit.Editor
{
    using System;
    using System.Diagnostics;
    using System.Text;
    using Microsoft.Win32;
    using UnityEditor;
    using UnityEngine;
    using Framework3.Editor; // 引入基类与安装器

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
    using Sirenix.OdinInspector.Editor;
#endif

    public class PlayerPrefsWindow : OdinAwareEditorWindowBase
    {
        protected override string WindowTitle => "PlayerPrefs";

        [MenuItem("Framework3/BindableKit/Open PlayerPrefs Window")]
        private static void OpenWindow()
        {
            var window = GetWindow<PlayerPrefsWindow>();
            window.Show();
#if ODIN_INSPECTOR
            window.InitializeList();
#endif
        }

        [MenuItem("Framework3/BindableKit/Open Registry Window")]
        private static void OpenRegistry()
        {
#if UNITY_EDITOR_WIN
            var companyName = PlayerSettings.companyName;
            var productName = PlayerSettings.productName;
            var lastKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Applets\Regedit";
            var loc = $"HKEY_CURRENT_USER\\Software\\Unity\\UnityEditor\\{companyName}\\{productName}";
            Registry.SetValue(lastKey, "LastKey", loc);
            Process.Start(new ProcessStartInfo("regedit.exe"){ UseShellExecute = true });
#else
            EditorUtility.DisplayDialog("Not Supported","仅 Windows 支持打开注册表。","OK");
#endif
        }

#if ODIN_INSPECTOR
        // ---- 你的原有字段/逻辑（仅在 Odin 存在时编译）----
        public BindableList<PlayerPrefPair> PlayerPrefPairs;

        private void InitializeList()
        {
            if (PlayerPrefPairs != null) return;

            var companyName = PlayerSettings.companyName;
            var productName = PlayerSettings.productName;
            var keyValues = GetAll(companyName, productName);

            PlayerPrefPairs = new BindableList<PlayerPrefPair>(keyValues);
            PlayerPrefPairs.OnRemove.Register((i, pair) => pair.DeleteFromPlayerPrefs());
            PlayerPrefPairs.OnReplace.Register((i, oldPair, newPair) =>
            {
                oldPair.DeleteFromPlayerPrefs();
                newPair.WriteToPlayerPrefs();
            });
            PlayerPrefPairs.OnClear.Register(() =>
            {
                foreach (var pair in keyValues) pair.DeleteFromPlayerPrefs();
            });
        }
#endif

        // ---- 通用：读取 PlayerPrefs（Windows）----
        public static PlayerPrefPair[] GetAll(string companyName, string productName)
        {
            if (Application.platform != RuntimePlatform.WindowsEditor)
                throw new NotSupportedException("PlayerPrefsWindow 目前仅支持在 Windows Editor 下读取注册表。");

#if UNITY_5_5_OR_NEWER
            var registryKey =
                Registry.CurrentUser.OpenSubKey("Software\\Unity\\UnityEditor\\" + companyName + "\\" + productName);
#else
            var registryKey =
                Registry.CurrentUser.OpenSubKey("Software\\" + companyName + "\\" + productName);
#endif
            if (registryKey == null) return Array.Empty<PlayerPrefPair>();

            var valueNames = registryKey.GetValueNames();
            var temp = new PlayerPrefPair[valueNames.Length];

            for (var i = 0; i < valueNames.Length; i++)
            {
                var key = valueNames[i];
                var idx = key.LastIndexOf("_", StringComparison.Ordinal);
                if (idx >= 0) key = key.Remove(idx, key.Length - idx);

                var ambiguous = registryKey.GetValue(valueNames[i]);
                var kind = registryKey.GetValueKind(valueNames[i]);

                if (kind == RegistryValueKind.DWord)
                {
                    if (PlayerPrefs.GetInt(key, -1) == -1 && PlayerPrefs.GetInt(key, 0) == 0)
                        ambiguous = PlayerPrefs.GetFloat(key);
                }
                else if (kind == RegistryValueKind.Binary)
                {
                    ambiguous = Encoding.Default.GetString((byte[])ambiguous);
                }

                temp[i] = new PlayerPrefPair { Key = key, Value = ambiguous };
            }
            return temp;
        }

        [Serializable]
        public struct PlayerPrefPair
        {
#if ODIN_INSPECTOR
            [HorizontalGroup("PlayerPref", Gap = 50)]
            [LabelWidth(50)]
#endif
            public string Key;

#if ODIN_INSPECTOR
            [HorizontalGroup("PlayerPref")]
            [ShowInInspector]
            [HideReferenceObjectPicker]
            [LabelWidth(50)]
#endif
            public object Value;

            public void WriteToPlayerPrefs()
            {
                if (string.IsNullOrEmpty(Key)) return;
                if (Value is string s) PlayerPrefs.SetString(Key, s);
                else if (Value is int i) PlayerPrefs.SetInt(Key, i);
                else if (Value is float f) PlayerPrefs.SetFloat(Key, f);
            }

            public void DeleteFromPlayerPrefs()
            {
                if (!string.IsNullOrEmpty(Key)) PlayerPrefs.DeleteKey(Key);
            }
        }
    }
}
