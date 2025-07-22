// ------------------------------------------------------------
// @file       ICounterAppModel.cs
// @brief
// @author     zheliku
// @Modified   2024-10-09 00:10:51
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core.Example._0.CounterApp.Scripts.Model
{
    public interface ICounterAppModel : IModel
    {
        public BindableProperty<int> Count { get; }
    }
}