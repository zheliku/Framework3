﻿// ------------------------------------------------------------
// @file       ICanGetModel.cs
// @brief
// @author     zheliku
// @Modified   2024-10-04 10:10:38
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core
{
    /// <summary>
    /// 可获取 Model，通过 Architecture 获取，由 CanGetModelExtension 扩展实现
    /// </summary>
    public interface ICanGetModel : IBelongToArchitecture
    {
    }
}