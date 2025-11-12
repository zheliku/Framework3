// ------------------------------------------------------------
// @file       IPanel.cs
// @brief
// @author     zheliku
// @Modified   2025-09-08 00:27:06
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.UIKit
{
    using UnityEngine;

    public enum PanelState
    {
        Loaded,
        Shown,
        Hidden,
        Unloaded
    }

    public interface IPanel
    {
        /// <summary>
        ///     Panel 依附的 Transform
        /// </summary>
        Transform Transform { get; }

        /// <summary>
        ///     Panel 状态
        /// </summary>
        PanelState State { get; }

        /// <summary>
        ///     显示 Panel
        /// </summary>
        void Show();

        /// <summary>
        ///     隐藏 Panel
        /// </summary>
        void Hide();
    }
}