// ------------------------------------------------------------
// @file       IPanel3D.cs
// @brief
// @author     zheliku
// @Modified   2024-12-12 14:12:47
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.UIKit
{
    public enum UILevel
    {
        Bg     = -100, // 背景层
        Bottom = -99,  // 底部层
        Common = 0,    // 普通层
        Top    = 99    // 顶部层
    }

    public interface IPanel2D : IPanel
    {
        /// <summary>
        ///     Panel2D 层级
        /// </summary>
        UILevel Level { get; set; }
    }
}