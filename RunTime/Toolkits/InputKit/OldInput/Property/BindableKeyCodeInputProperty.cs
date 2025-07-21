// ------------------------------------------------------------
// @file       BindableKeyCodeProperty.cs
// @brief
// @author     zheliku
// @Modified   2024-12-03 14:12:20
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

using Framework3.Core;

namespace Framework3.Toolkits.InputKit
{
    using Framework3.Core;

    public class BindableKeyCodeInputProperty : BindableProperty<bool>
    {
        public BindableKeyCodeInputProperty() : base(false, true)
        { }
    }
}