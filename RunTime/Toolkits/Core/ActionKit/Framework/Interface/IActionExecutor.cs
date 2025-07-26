// ------------------------------------------------------------
// @file       IActionExcutor.cs
// @brief
// @author     zheliku
// @Modified   2024-10-24 22:10:27
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.ActionKit
{
    using System;

    public interface IActionExecutor
    {
        void Execute(IActionController controller, Action<IActionController> onFinish = null);
    }
}