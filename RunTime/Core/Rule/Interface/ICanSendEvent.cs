﻿// ------------------------------------------------------------
// @file       ICanSendEvent.cs
// @brief
// @author     zheliku
// @Modified   2024-10-04 11:10:34
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core
{
    /// <summary>
    /// 可发送 Event，通过 Architecture 发送，由 CanSendEventExtension 扩展实现
    /// </summary>
    public interface ICanSendEvent : IBelongToArchitecture
    {
    }
}