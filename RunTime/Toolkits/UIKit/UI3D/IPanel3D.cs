// ------------------------------------------------------------
// @file       IPanel3D.cs
// @brief
// @author     zheliku
// @Modified   2024-12-12 14:12:47
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.UIKit
{
    using UnityEngine;

    public interface IPanel3D : IPanel
    {
        public Canvas Canvas { get; }
    }
}