// ------------------------------------------------------------
// @file       UIPanel3D.cs
// @brief
// @author     zheliku
// @Modified   2024-12-12 19:12:14
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.UIKit
{
    using System;

    /// <summary>
    ///     挂载到 Canvas 下的 Panel 上
    /// </summary>
    public abstract class UIPanel2D : UIPanel, IPanel2D
    {
        public UILevel Level
        {
            get => Enum.Parse<UILevel>(transform.parent?.name); // 父物体名称是该 Panel2D 的 Level
            set
            {
                if (Enum.TryParse<UILevel>(transform.parent?.name, out var level))
                {
                    if (level == value) // 相同的 level 不做处理
                    {
                        return;
                    }
                }
                UI2DRoot.Instance.SetLevelOfPanel(value, this);
            }
        }
    }
}