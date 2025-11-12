// ------------------------------------------------------------
// @file       CounterAppModel.cs
// @brief
// @author     zheliku
// @Modified   2024-10-09 00:10:06
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core.Example._0.CounterApp.Scripts.Model
{
    using Utility;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    // 计数应用模型，实现了 ICounterAppModel 接口，继承自 AbstractModel
    public class CounterAppModel : AbstractModel, ICounterAppModel
    {
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public BindableProperty<int> Count { get; set; } = new();

        protected override void OnInit()
        {
            var storage = this.GetUtility<IStorage>();

            // 设置初始值（不触发事件）
            Count.SetValueWithoutNotify(storage.LoadInt(nameof(Count)));

            // 当数据变更时 存储数据
            Count.Register((oldValue, newCount) =>
            {
                storage.SaveInt(nameof(Count), newCount);
            });
        }
    }
}