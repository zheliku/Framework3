// ------------------------------------------------------------
// @file       UIPanel3D.cs
// @brief
// @author     zheliku
// @Modified   2024-12-12 19:12:14
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.UIKit
{
    using UnityEngine;

    /// <summary>
    /// 挂载到 Canvas 上，并将 Canvas 设置为 World Space
    /// </summary>
    public abstract class UIPanel3D : UIPanel, IPanel3D
    {
        public Canvas Canvas { get => GetComponent<Canvas>(); }
    }
}