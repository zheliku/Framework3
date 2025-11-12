// ------------------------------------------------------------
// @file       CodeGenWindow.cs
// @brief      Odin-aware：未装 Odin 显示引导；已装照常使用 Odin 界面/逻辑
// @author     zheliku
// @Modified   2025-02-23 22:02:38
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.CodeGenKit.Editor
{
    using FluentAPI;
    using UnityEditor;
    using Framework3.Editor;
    using UnityEngine;

#if ODIN_INSPECTOR
    using System.IO;
    using Sirenix.OdinInspector;
    using Sirenix.OdinInspector.Editor;
#endif

    public class CodeGenWindow : OdinAwareEditorWindowBase
    {
        protected override string WindowTitle => "CodeGen";

        // 如需自定义 Samples 的显示名/包名以提高定位准确性，可在此覆盖
        // protected override string PackageDisplayName => "Framework3";
        // protected override string PackageName => "com.your.framework3";

#if ODIN_INSPECTOR
        [ShowInInspector] public string NameSpace
        {
            get => CodeGenPipeline.Default.NameSpace;
            set => CodeGenPipeline.Default.NameSpace = value;
        }

        [ShowInInspector, FolderPath] public string FolderPath
        {
            get => CodeGenPipeline.Default.FolderPath;
            set => CodeGenPipeline.Default.FolderPath = value;
        }

        [ShowInInspector] public string FileName
        {
            get => CodeGenPipeline.Default.FileName;
            set => CodeGenPipeline.Default.FileName = value;
        }

        [ShowInInspector] public GameObject SelectedGameObject
        {
            get => CodeGenPipeline.Default.LastSelectedGameObject;
            set
            {
                CodeGenPipeline.Default.LastSelectedGameObject = value;
                if (value != null) FileName = value.name.Replace(" ", "");
            }
        }

        [ShowInInspector] public string Architecture
        {
            get => CodeGenPipeline.Default.Architecture;
            set => CodeGenPipeline.Default.Architecture = value;
        }

        public bool IsGenerating
        {
            get => CodeGenPipeline.Default.IsGenerating;
            set => CodeGenPipeline.Default.IsGenerating = value;
        }

        [HorizontalGroup("Buttons"), Button(ButtonSizes.Large)]
        public void Generate()
        {
            CheckArchitectureExist();
            IsGenerating = true;
            var filePath = $"{FolderPath}/{FileName}.cs";
            if (!Directory.Exists(FolderPath)) Directory.CreateDirectory(FolderPath);
            File.WriteAllText(filePath, GenerateCodeContent);
            AssetDatabase.Refresh();
        }

        [HorizontalGroup("Buttons"), Button(ButtonSizes.Large)]
        public void GenerateAndOpen()
        {
            CheckArchitectureExist();
            IsGenerating = true;
            var filePath = $"{FolderPath}/{FileName}.cs";
            if (!Directory.Exists(FolderPath)) Directory.CreateDirectory(FolderPath);
            File.WriteAllText(filePath, GenerateCodeContent);
            CodeGenKit.OpenFile(filePath);
            AssetDatabase.Refresh();
        }

        private void CheckArchitectureExist()
        {
            if (Architecture.GetTypeByName() == null)
            {
                var architectureContent = CodeGenPipeline.Default.GenerateArchitectureCode();
                if (!Directory.Exists(FolderPath)) Directory.CreateDirectory(FolderPath);
                File.WriteAllText(FolderPath + "/" + Architecture + ".cs", architectureContent);
            }
        }

        [ShowInInspector, DisplayAsString(false), HideLabel]
        public string GenerateCodeContent => CodeGenPipeline.Default.GenerateViewCode();
#endif

        [MenuItem("Framework3/CodeGen/Open CodeGen Window &V")]
        private static void OpenWindow()
        {
            var window = GetWindow<CodeGenWindow>();
            window.Show();
#if ODIN_INSPECTOR
            window.LoadCodeGenData();
            window.SelectedGameObject = Selection.activeGameObject;
#endif
        }

#if ODIN_INSPECTOR
        private void LoadCodeGenData()
        {
            NameSpace    = CodeGenPipeline.Default.GlobalNameSpace;
            FolderPath   = CodeGenPipeline.Default.GlobalFolderPath;
            FileName     = CodeGenPipeline.Default.GlobalFileName;
            Architecture = CodeGenPipeline.Default.GlobalArchitecture;
        }

        [Button(ButtonSizes.Large), PropertySpace(5)]
        private void SaveCodeGenData()
        {
            CodeGenPipeline.Default.GlobalNameSpace    = NameSpace;
            CodeGenPipeline.Default.GlobalFolderPath   = FolderPath;
            CodeGenPipeline.Default.GlobalFileName     = FileName;
            CodeGenPipeline.Default.GlobalArchitecture = Architecture;
        }
#endif
    }
}
