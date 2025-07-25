﻿// ------------------------------------------------------------
// @file       IUnRegisterList.cs
// @brief
// @author     zheliku
// @Modified   2024-10-04 16:10:02
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// 注销器列表接口
    /// </summary>
    public interface IUnRegisterList
    {
        /// <summary>
        /// 注销器列表
        /// </summary>
        List<IUnRegister> UnregisterList { get; }
    }
}