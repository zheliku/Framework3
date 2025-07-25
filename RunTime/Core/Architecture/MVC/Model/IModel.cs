﻿// ------------------------------------------------------------
// @file       IModel.cs
// @brief
// @author     zheliku
// @Modified   2024-10-04 11:10:28
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core
{
    /// <summary>
    /// Model 接口
    /// </summary>
    public interface IModel : // IBelongToArchitecture, 
        ICanInit,             // IModel 可初始化
        ICanGetUtility,       // IModel 可获取 Utility
        ICanSetArchitecture,  // IModel 可设置 Architecture
        ICanSendEvent         // IModel 可发送 Event
    {
    }
}