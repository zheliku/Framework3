﻿// ------------------------------------------------------------
// @file       ICanGetUtility.cs
// @brief
// @author     zheliku
// @Modified   2024-10-04 11:10:57
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core
{
    /// <summary>
    /// 可获取 Utility，通过 Architecture 获取，由 CanGetUtilityExtension 扩展实现
    /// </summary>
    public interface ICanGetUtility : IBelongToArchitecture
    {
    }
}