﻿// ------------------------------------------------------------
// @file       ICommand.cs
// @brief
// @author     zheliku
// @Modified   2024-10-04 11:10:52
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core
{
    /// <summary>
    /// Command 接口，无返回值
    /// </summary>
    public interface ICommand : // IBelongToArchitecture,
        ICanGetModel,           // ICommand 可获取 Model
        ICanGetSystem,          // ICommand 可获取 System
        ICanGetUtility,         // ICommand 可获取 Utility
        ICanSetArchitecture,    // ICommand 可设置 Architecture
        ICanSendCommand,        // ICommand 可发送 Command
        ICanSendEvent,          // ICommand 可发送 Event
        ICanSendQuery           // ICommand 可发送 Query
    {
        /// <summary>
        /// 执行 Command
        /// </summary>
        void Execute();
    }

    /// <summary>
    /// Command&lt;TResult&gt; 接口，有返回值
    /// </summary>
    public interface ICommand<out TResult> : // IBelongToArchitecture, 
        ICanGetModel,                        // ICommand 可获取 Model
        ICanGetSystem,                       // ICommand 可获取 System
        ICanGetUtility,                      // ICommand 可获取 Utility
        ICanSetArchitecture,                 // ICommand 可设置 Architecture
        ICanSendCommand,                     // ICommand 可发送 Command
        ICanSendEvent,                       // ICommand 可发送 Event
        ICanSendQuery                        // ICommand 可发送 Query
    {
        /// <summary>
        /// 执行 Command
        /// </summary>
        /// <returns>返回值</returns>
        TResult Execute();
    }
}