// ------------------------------------------------------------
// @file       CodeGenPipeline.cs
// @brief
// @author     zheliku
// @Modified   2025-02-24 01:02:06
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.CodeGenKit.Editor
{
    using System;
    using System.IO;
    using Sirenix.OdinInspector;
    using UnityEditor;
    using UnityEditor.Callbacks;
    using UnityEngine;

    public class CodeGenPipeline : ScriptableObject
    {
        private static CodeGenPipeline _Default;

        public static CodeGenPipeline Default
        {
            get
            {
                if (_Default) return _Default;

                var filePath = CodeGenKit.PIPE_LINE_PATH;
                var directoryPath = Path.GetDirectoryName(filePath);

                if (File.Exists(filePath))
                {
                    return _Default = AssetDatabase.LoadAssetAtPath<CodeGenPipeline>(filePath);
                }

                _Default = CreateInstance<CodeGenPipeline>();

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // 保存为 asset 文件
                AssetDatabase.CreateAsset(_Default, filePath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                return _Default;
            }
        }

        public string GlobalNameSpace = "Game";

        [FolderPath]
        public string GlobalFolderPath = "Assets/Scripts";

        public string GlobalFileName;

        public string GlobalArchitecture = "Game";

        [ReadOnly]
        public bool IsGenerating;

        [ReadOnly]
        public GameObject LastSelectedGameObject;

        [ReadOnly]
        public string NameSpace = "Game";

        [ReadOnly] [FolderPath]
        public string FolderPath = "Assets/Scripts";

        [ReadOnly]
        public string FileName;

        [ReadOnly]
        public string Architecture = "Game";

        public string GenerateViewCode()
        {
            var templateContent = CodeGenTemplate.VIEW_CODE;

            // 获取当前时间，格式化为 yyyy-MM-dd HH:mm:ss
            var modifiedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // 替换占位符
            string content = templateContent
               .Replace("__Game__", NameSpace)
               .Replace("CodeTemplate", FileName)
               .Replace("__modifiedTime__", modifiedTime)
               .Replace("null", string.IsNullOrEmpty(Architecture) ? "null" : Architecture + ".Architecture");

            return content;
        }

        public string GenerateArchitectureCode()
        {
            var templateContent = CodeGenTemplate.ARCHITECTURE_CODE;

            // 获取当前时间，格式化为 yyyy-MM-dd HH:mm:ss
            var modifiedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // 替换占位符
            string content = templateContent
                .Replace("__Game__", NameSpace)
                .Replace("ArchitectureTemplate", Architecture)
                .Replace("__modifiedTime__", modifiedTime);

            return content;
        }

        /// <summary>
        /// Unity 编译脚本后执行的回调函数
        /// </summary>
        private void OnDidReloadScripts()
        {
            if (!IsGenerating) return;

            IsGenerating = false;

            if (LastSelectedGameObject == null) return;

            var scriptType = Type.GetType($"{NameSpace}.{FileName}, Assembly-CSharp");

            if (scriptType != null)
            {
                // 将脚本绑定到 GameObject
                LastSelectedGameObject.AddComponent(scriptType);
                Debug.Log($"脚本 {FileName}.cs 已成功绑定到 {LastSelectedGameObject.name}");
                LastSelectedGameObject = null;
            }
            else
            {
                Debug.LogWarning($"脚本 {FileName} 未找到，请确保命名空间和类名正确");
            }
        }

        [DidReloadScripts]
        private static void DidReloadScripts()
        {
            Default?.OnDidReloadScripts();
        }

        [MenuItem("Framework/CodeGen/Select CodeGen Pipeline &S")]
        public static void SelectPipeline()
        {
            EditorGUIUtility.PingObject(Default);
        }
    }
}
