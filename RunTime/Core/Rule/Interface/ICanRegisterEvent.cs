﻿// ------------------------------------------------------------
// @file       ICanRigisterEvent.cs
// @brief
// @author     zheliku
// @Modified   2024-10-04 11:10:22
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core
{
    /// <summary>
    /// 可注册 Event，通过 Architecture 注册，由 CanRegisterEventExtension 扩展实现
    /// </summary>
    public interface ICanRegisterEvent : IBelongToArchitecture
    {
    }
}