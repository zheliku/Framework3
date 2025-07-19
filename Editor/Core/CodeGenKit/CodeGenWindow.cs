// ------------------------------------------------------------
// @file       CodeGenWindow.cs
// @brief
// @author     zheliku
// @Modified   2025-02-23 22:02:38
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------

using Framework.Toolkits.FluentAPI;

namespace Framework.Toolkits.CodeGenKit.Editor
{
    using System.IO;
    using Sirenix.OdinInspector;
    using Sirenix.OdinInspector.Editor;
    using UnityEditor;
    using UnityEngine;

    public class CodeGenWindow : OdinEditorWindow
    {
        [ShowInInspector]
        public string NameSpace
        {
            get => CodeGenPipeline.Default.NameSpace;
            set => CodeGenPipeline.Default.NameSpace = value;
        }

        [ShowInInspector] [FolderPath]
        public string FolderPath
        {
            get => CodeGenPipeline.Default.FolderPath;
            set => CodeGenPipeline.Default.FolderPath = value;
        }

        [ShowInInspector]
        public string FileName
        {
            get => CodeGenPipeline.Default.FileName;
            set => CodeGenPipeline.Default.FileName = value;
        }

        [ShowInInspector]
        public GameObject SelectedGameObject
        {
            get => CodeGenPipeline.Default.LastSelectedGameObject;
            set
            {
                CodeGenPipeline.Default.LastSelectedGameObject = value;

                if (value != null)
                {
                    FileName = value.name.Replace(" ", "");
                }
            }
        }

        [ShowInInspector]
        public string Architecture
        {
            get => CodeGenPipeline.Default.Architecture;
            set => CodeGenPipeline.Default.Architecture = value;
        }

        public bool IsGenerating
        {
            get => CodeGenPipeline.Default.IsGenerating;
            set => CodeGenPipeline.Default.IsGenerating = value;
        }

        [HorizontalGroup("Buttons")]
        [Button(ButtonSizes.Large)]
        public void Generate()
        {
            CheckArchitectureExist();

            IsGenerating = true;

            var filePath = $"{FolderPath}/{FileName}.cs";

            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

            File.WriteAllText(filePath, GenerateCodeContent);

            // 刷新 Unity 资源数据库，触发脚本编译
            AssetDatabase.Refresh();
        }

        [HorizontalGroup("Buttons")]
        [Button(ButtonSizes.Large)]
        public void GenerateAndOpen()
        {
            CheckArchitectureExist();

            IsGenerating = true;

            // SaveCodeGenData();

            var filePath = $"{FolderPath}/{FileName}.cs";

            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

            File.WriteAllText(filePath, GenerateCodeContent);

            CodeGenKit.OpenFile(filePath); // 打开文件

            // 刷新 Unity 资源数据库，触发脚本编译
            AssetDatabase.Refresh();
        }

        private void CheckArchitectureExist()
        {
            if (Architecture.GetTypeByName() == null)
            {
                var architectureContent = CodeGenPipeline.Default.GenerateArchitectureCode();

                File.WriteAllText(FolderPath + "/" + Architecture + ".cs", architectureContent);
            }
        }

        [ShowInInspector] [DisplayAsString(false)] [HideLabel]
        public string GenerateCodeContent
        {
            get => CodeGenPipeline.Default.GenerateViewCode();
        }

        [MenuItem("Framework/CodeGen/Open CodeGen Window &V")]
        private static void OpenWindow()
        {
            var window = GetWindow<CodeGenWindow>();
            window.Show();

            window.LoadCodeGenData();

            window.SelectedGameObject = Selection.activeGameObject;
        }

        private void LoadCodeGenData()
        {
            NameSpace    = CodeGenPipeline.Default.GlobalNameSpace;
            FolderPath   = CodeGenPipeline.Default.GlobalFolderPath;
            FileName     = CodeGenPipeline.Default.GlobalFileName;
            Architecture = CodeGenPipeline.Default.GlobalArchitecture;
        }

        [Button(ButtonSizes.Large)] [PropertySpace(5)]
        private void SaveCodeGenData()
        {
            CodeGenPipeline.Default.GlobalNameSpace    = NameSpace;
            CodeGenPipeline.Default.GlobalFolderPath   = FolderPath;
            CodeGenPipeline.Default.GlobalFileName     = FileName;
            CodeGenPipeline.Default.GlobalArchitecture = Architecture;
        }
    }
}
