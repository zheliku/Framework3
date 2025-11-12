// ------------------------------------------------------------
// @file       DynamicGrid.cs
// @brief      动态稀疏网格，用于非连续网格数据的存储、访问与遍历（支持自定义数据类型）
// @author     zheliku
// @Modified   2024-11-01 13:11:13
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.GridKit
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    /// <summary>
    ///     动态稀疏网格
    /// </summary>
    /// <typeparam name="TValue">网格单元存储的数据类型（如GameObject、配置数据、自定义实体类等）</typeparam>
#if ODIN_INSPECTOR
    [HideReferenceObjectPicker]
    // Odin Inspector特性：隐藏网格实例的引用对象选择器，简化编辑器中网格对象的引用配置
#endif
    public partial class DynamicGrid<TValue> : IEnumerable<KeyValuePair<Vector2Int, TValue>>
    {
        /// <summary>
        ///     存储网格数据的核心字典：Key为自定义索引结构<see cref="Index" />（行列组合），Value为网格单元数据
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector]
        // Odin Inspector特性：在编辑器中可视化网格数据
        [DictionaryDrawerSettings(KeyLabel = "Row, Column", ValueLabel = "Value")] // 自定义字典在编辑器中的显示标签，提升可读性
    #endif
        private readonly Dictionary<Index, TValue> _grid;

        /// <summary>
        ///     初始化动态网格的空实例
        /// </summary>
        public DynamicGrid()
        {
            _grid = new Dictionary<Index, TValue>();
        }

        /// <summary>
        ///     获取网格中已存储的单元数量（只读）
        /// </summary>
        /// <value>当前网格中非空单元的总数（即字典中键值对的数量）</value>
        public int Count
        {
            get => _grid.Count;
        }

        /// <summary>
        ///     通过【行索引+列索引】访问或设置网格单元数据（索引器重载1）
        /// </summary>
        /// <param name="row">目标单元的行索引（支持正负整数，无范围限制）</param>
        /// <param name="column">目标单元的列索引（支持正负整数，无范围限制）</param>
        /// <value>目标位置的单元数据；若设置时位置不存在，会自动创建新单元</value>
        /// <remarks>
        ///     - 读取时：若目标位置无数据，返回&lt;TValue>的默认值（如引用类型为null，值类型为默认值）；
        ///     - 写入时：若目标位置已存在数据，会覆盖原有数据；若不存在，会新增键值对
        /// </remarks>
        /// <example>
        ///     <code>
        /// // 设置(2,3)位置的单元数据
        /// dynamicGrid[2, 3] = new MyGridData();
        ///  
        /// // 获取(2,3)位置的单元数据
        /// var data = dynamicGrid[2, 3];
        /// </code>
        /// </example>
        public TValue this[int row, int column]
        {
            get
            {
                var key = new Index(row, column);
                return _grid.GetValueOrDefault(key);
            }
            set
            {
                var key = new Index(row, column);
                _grid[key] = value;
            }
        }

        /// <summary>
        ///     通过【Vector2Int索引】访问或设置网格单元数据（索引器重载2）
        /// </summary>
        /// <param name="index">目标单元的位置索引（x对应行索引，y对应列索引）</param>
        /// <value>目标位置的单元数据；若设置时位置不存在，会自动创建新单元</value>
        /// <remarks>功能与【行+列】索引器一致，仅适配Vector2Int类型的位置参数（如Unity中的坐标点）</remarks>
        /// <example>
        ///     <code>
        /// // 设置Vector2Int(2,3)位置的单元数据
        /// dynamicGrid[new Vector2Int(2, 3)] = new MyGridData();
        ///  
        /// // 获取Vector2Int(2,3)位置的单元数据
        /// var data = dynamicGrid[new Vector2Int(2, 3)];
        /// </code>
        /// </example>
        public TValue this[Vector2Int index]
        {
            get
            {
                var key = new Index(index.x, index.y);
                return _grid.GetValueOrDefault(key);
            }
            set
            {
                var key = new Index(index.x, index.y);
                _grid[key] = value;
            }
        }

        /// <summary>
        ///     实现<see cref="IEnumerable{KeyValuePair{Vector2Int, TValue}}" />接口，获取网格的枚举器
        /// </summary>
        /// <returns>枚举器：每个元素为键值对（Key为Vector2Int类型位置，Value为单元数据）</returns>
        /// <remarks>支持foreach遍历网格，自动将内部<see cref="Index" />转换为Vector2Int，简化外部使用</remarks>
        public IEnumerator<KeyValuePair<Vector2Int, TValue>> GetEnumerator()
        {
            foreach (var kvp in _grid)
            {
                yield return new KeyValuePair<Vector2Int, TValue>(
                    new Vector2Int(kvp.Key.Row, kvp.Key.Column), kvp.Value
                );
            }
        }

        /// <summary>
        ///     实现<see cref="IEnumerable" />接口，获取非泛型枚举器
        /// </summary>
        /// <returns>非泛型枚举器（内部调用泛型枚举器实现）</returns>
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

        /// <summary>
        ///     遍历网格中所有单元，执行带行列索引的自定义逻辑
        /// </summary>
        /// <param name="each">遍历回调函数：参数1为行索引，参数2为列索引，参数3为当前网格单元数据</param>
        /// <remarks>遍历顺序与字典存储顺序一致（非有序），适合需要获取单元位置信息的场景（如绘制网格、位置相关计算）</remarks>
        /// <example>
        ///     <code>
        /// // 遍历网格并打印每个单元的位置与数据
        /// dynamicGrid.ForEach((row, col, value) => 
        /// {
        ///     Debug.Log($"Row: {row}, Col: {col}, Value: {value}");
        /// });
        /// </code>
        /// </example>
        public void ForEach(Action<int, int, TValue> each)
        {
            foreach (var kvp in _grid)
            {
                each(kvp.Key.Row, kvp.Key.Column, kvp.Value);
            }
        }

        /// <summary>
        ///     遍历网格中所有单元，仅执行数据相关的自定义逻辑
        /// </summary>
        /// <param name="each">遍历回调函数：参数为当前网格单元数据</param>
        /// <remarks>遍历顺序与字典存储顺序一致（非有序），适合仅需处理数据、无需位置信息的场景（如批量释放资源、数据校验）</remarks>
        /// <example>
        ///     <code>
        /// // 遍历网格并销毁所有单元对应的GameObject
        /// dynamicGrid.ForEach(value => 
        /// {
        ///     if (value is GameObject obj) Destroy(obj);
        /// });
        /// </code>
        /// </example>
        public void ForEach(Action<TValue> each)
        {
            foreach (var kvp in _grid)
            {
                each(kvp.Value);
            }
        }

        /// <summary>
        ///     清空网格中所有单元数据（可选执行单元清理逻辑）
        /// </summary>
        /// <param name="cleanupItem">单元清理回调（可选）：清空前对每个单元数据执行的自定义清理逻辑（如释放资源、注销事件）</param>
        /// <remarks>
        ///     - 若传入cleanupItem，会先遍历所有单元执行清理逻辑，再清空字典；
        ///     - 若未传入cleanupItem，直接清空字典（适用于无需额外清理的简单数据类型，如int、string）
        /// </remarks>
        /// <example>
        ///     <code>
        /// // 清空网格并销毁所有单元对应的GameObject
        /// dynamicGrid.Clear(value => 
        /// {
        ///     if (value is GameObject obj) Destroy(obj);
        /// });
        ///  
        /// // 直接清空网格（无额外清理）
        /// dynamicGrid.Clear();
        /// </code>
        /// </example>
        public void Clear(Action<TValue> cleanupItem = null)
        {
            // 补充：原代码未执行cleanupItem，此处注释提示潜在优化点（若需生效需添加遍历执行逻辑）
            if (cleanupItem != null)
            {
                foreach (var kvp in _grid)
                {
                    cleanupItem(kvp.Value);
                }
            }
            _grid.Clear();
        }
    }

    /// <summary>
    ///     <see cref="DynamicGrid{TValue}" />的部分类，包含网格内部索引结构<see cref="Index" />
    /// </summary>
    public partial class DynamicGrid<TValue>
    {
        /// <summary>
        ///     网格内部的索引结构（值类型），用于封装行、列组合，作为字典的Key
        /// </summary>
        /// <remarks>
        ///     相比直接使用Vector2Int，自定义值类型索引可减少装箱操作，提升字典查找效率；
        ///     实现<see cref="IEquatable{Index}" />接口确保相等性判断与哈希计算的正确性，避免字典Key冲突
        /// </remarks>
        private readonly struct Index : IEquatable<Index>
        {
            /// <summary>
            ///     索引的行号（在编辑器中水平分组显示，标签宽度30）
            /// </summary>
        #if ODIN_INSPECTOR
            [ShowInInspector]
            [LabelWidth(30)]
            [HorizontalGroup("Index")]
        #endif
            public readonly int Row;

            /// <summary>
            ///     索引的列号（在编辑器中水平分组显示，标签简化为"Col"，宽度30）
            /// </summary>
        #if ODIN_INSPECTOR
            [ShowInInspector]
            [LabelWidth(30)]
            [LabelText("Col")]
            [HorizontalGroup("Index")]
        #endif
            public readonly int Column;

            /// <summary>
            ///     初始化索引结构的实例
            /// </summary>
            /// <param name="row">行索引</param>
            /// <param name="column">列索引</param>
            public Index(int row, int column)
            {
                Row    = row;
                Column = column;
            }

            /// <summary>
            ///     判断当前索引是否与另一个索引相等（基于行和列的数值比较）
            /// </summary>
            /// <param name="other">待比较的另一个索引</param>
            /// <returns>true：行和列均相等；false：行或列不相等</returns>
            public bool Equals(Index other)
            {
                return Row == other.Row && Column == other.Column;
            }

            /// <summary>
            ///     判断当前索引是否与指定对象相等（重写Object.Equals）
            /// </summary>
            /// <param name="obj">待比较的对象</param>
            /// <returns>true：对象为Index类型且行、列均相等；false：对象类型不匹配或数值不相等</returns>
            public override bool Equals(object obj)
            {
                return obj is Index other && Equals(other);
            }

            /// <summary>
            ///     获取当前索引的哈希码（用于字典Key的哈希计算）
            /// </summary>
            /// <returns>基于行和列组合的哈希码</returns>
            public override int GetHashCode()
            {
                return HashCode.Combine(Row, Column);
            }
        }
    }
}