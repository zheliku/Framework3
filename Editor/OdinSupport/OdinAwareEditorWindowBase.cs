// ------------------------------------------------------------
// @file       OdinAwareEditorWindowBase.cs
// @brief
// @author     zheliku
// @Modified   2025-11-13 03:46:56
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------

namespace Framework3.Editor
{
    using UnityEditor;
    using UnityEngine;

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector.Editor;

    /// <summary>
    /// 已装 Odin：继承 OdinEditorWindow；未装 Odin：见下方的无 Odin 分支。
    /// 提供统一的“未安装 Odin 的引导 UI”以及“已安装 Odin 的绘制切换”。
    /// </summary>
    public abstract class OdinAwareEditorWindowBase : OdinEditorWindow
#else
    /// <summary>
    ///     未装 Odin：退回 EditorWindow，但同样具备“引导 UI”与生命周期。
    /// </summary>
    public abstract class OdinAwareEditorWindowBase : EditorWindow
#endif
    {
        /// <summary>
        ///     子类可覆盖：窗口标题
        /// </summary>
        protected virtual string WindowTitle
        {
            get => GetType().Name;
        }

        /// <summary>
        ///     子类可覆盖：用于 Samples 安装时使用的包显示名（用于 Assets/Samples/&lt;displayName&gt;/…）
        ///     返回 null 时自动从 package.json 读取。
        /// </summary>
        protected virtual string _PackageDisplayName
        {
            get => null;
        }

        /// <summary>
        ///     子类可覆盖：用于 Package Manager 精确定位的包名（com.xxx.yyy）。返回 null 时自动从 package.json 读取。
        /// </summary>
        protected virtual string _PackageName
        {
            get => null;
        }

    #if ODIN_INSPECTOR
        protected override void OnEnable()
    #else
        protected void OnEnable()
    #endif
        {
            titleContent = new GUIContent(WindowTitle);
            OdinSamplesInstaller.InitPackageInfoIfNeeded(_PackageName, _PackageDisplayName);
        }

        /// <summary>
        ///     子类实现：当 Odin 存在时绘制窗口内容（通常你仅需保持现有逻辑，不必实现 OnGUI）。
        /// </summary>
        protected virtual void DrawBodyWithOdin() { }

    #if !ODIN_INSPECTOR
        // 未装 Odin：我们接管 OnGUI，显示引导面板。
        protected virtual void OnGUI()
        {
            OdinSamplesInstaller.DrawInstallGuideUI(WindowTitle);
        }
    #else
        // 已装 Odin：让 Odin 负责绘制；我们在底部加个提示区域（可选）
        // protected override void OnGUI()
        // {
        //     base.OnGUI(); // Odin 照常绘制组件
        //     // 需要的话，可在此添加公共尾注
        // }
    #endif
    }
}