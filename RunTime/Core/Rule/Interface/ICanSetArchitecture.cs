// ------------------------------------------------------------
// @file       ICanSetArchitecture.cs
// @brief
// @author     zheliku
// @Modified   2024-10-04 10:10:39
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core
{
    /// <summary>
    /// 继承该接口，即可设置 Architecture
    /// </summary>
    public interface ICanSetArchitecture : IBelongToArchitecture
    {
        new IArchitecture Architecture { set; }
    }
}