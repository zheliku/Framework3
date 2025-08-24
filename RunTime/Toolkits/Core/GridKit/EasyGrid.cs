// ------------------------------------------------------------
// @file       Grid.cs
// @brief      密集网格，用于固定行列范围的连续网格数据存储、访问与批量操作（支持自定义数据类型）
// @author     zheliku
// @Modified   2024-11-01 12:11:43
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

using Framework3.Core;

namespace Framework3.Toolkits.GridKit
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    using UnityEngine;

    /// <summary>
    /// 密集型网格
    /// </summary>
    /// <typeparam name="TValue">网格单元存储的数据类型（如单元格预制体、数值、状态标记等）</typeparam>
    [HideReferenceObjectPicker] // Odin Inspector特性：隐藏网格实例的引用对象选择器，简化编辑器中网格对象的引用配置
    public class EasyGrid<TValue> : IEnumerable<KeyValuePair<Vector2Int, TValue>>
    {
        /// <summary>
        /// 存储网格数据的核心二维数组（密集存储，每个行列位置均占用内存，即使值为默认）
        /// </summary>
        [ShowInInspector] // Odin Inspector特性：在编辑器中可视化网格数据
        [TableMatrix(Transpose = true)] // 自定义二维数组在编辑器中的显示样式（表格矩阵，行列转置以匹配直观的网格展示）
        private TValue[,] _grid;

        /// <summary>
        /// 初始化指定行列数的密集型网格实例
        /// </summary>
        /// <param name="row">网格的总行数（必须大于0，否则二维数组初始化会抛异常）</param>
        /// <param name="column">网格的总列数（必须大于0，否则二维数组初始化会抛异常）</param>
        /// <exception cref="ArgumentOutOfRangeException">当row或column小于等于0时，二维数组初始化会触发此异常</exception>
        public EasyGrid(int row, int column)
        {
            _grid = new TValue[row, column];
        }

        /// <summary>
        /// 获取当前网格的总行数（只读，由二维数组长度决定）
        /// </summary>
        public int Row { get => _grid.GetLength(0); }

        /// <summary>
        /// 获取当前网格的总列数（只读，由二维数组长度决定）
        /// </summary>
        public int Column { get => _grid.GetLength(1); }

        /// <summary>
        /// 通过【行索引+列索引】安全访问或设置网格单元数据（索引器）
        /// </summary>
        /// <param name="row">目标单元的行索引（范围：0 ≤ row &lt; <see cref="Row"/>）</param>
        /// <param name="column">目标单元的列索引（范围：0 ≤ column &lt; <see cref="Column"/>）</param>
        /// <value>目标位置的单元数据；设置时会覆盖原有数据（若在合法范围内）</value>
        /// <exception cref="FrameworkException">当索引超出【0~Row-1】或【0~Column-1】范围时抛出，提示索引越界</exception>
        /// <example>
        /// <code>
        /// // 初始化5行3列的网格
        /// var grid = new EasyGrid&lt;int>(5, 3);
        /// // 设置(2,1)位置的数值为10（合法索引）
        /// grid[2, 1] = 10;
        ///  
        /// // 获取(2,1)位置的数值（返回10）
        /// var value = grid[2, 1];
        ///  
        /// // 尝试访问(5,1)位置（行索引5 ≥ 总行数5），会抛FrameworkException
        /// // var invalidValue = grid[5, 1];
        /// </code>
        /// </example>
        public TValue this[int row, int column]
        {
            get
            {
                if (row >= 0 && row < Row && column >= 0 && column < Column)
                {
                    return _grid[row, column];
                }

                throw new FrameworkException($"Grid index ({row}, {column}) out of range ({Row}, {Column}");
            }

            set
            {
                if (row >= 0 && row < Row && column >= 0 && column < Column)
                {
                    _grid[row, column] = value;
                    return;
                }

                throw new FrameworkException($"Grid index ({row}, {column}) out of range ({Row}, {Column}");
            }
        }

        /// <summary>
        /// 用指定的统一值填充整个网格（覆盖所有单元原有数据）
        /// </summary>
        /// <param name="value">用于填充的统一值（可为&lt;TValue>的默认值，如null、0等）</param>
        /// <remarks>适用于网格初始化、批量重置场景（如棋盘重置为空白状态、数值网格重置为0）</remarks>
        /// <example>
        /// <code>
        /// // 将5x3的网格所有单元填充为0
        /// var grid = new EasyGrid&lt;int>(5, 3);
        /// grid.Fill(0);
        /// </code>
        /// </example>
        public void Fill(TValue value)
        {
            for (var i = 0; i < Row; i++)
            {
                for (var j = 0; j < Column; j++)
                {
                    _grid[i, j] = value;
                }
            }
        }

        /// <summary>
        /// 用自定义逻辑动态填充整个网格（支持按行列位置生成不同值）
        /// </summary>
        /// <param name="onFill">填充逻辑回调：参数1为行索引，参数2为列索引，返回当前位置需填充的值</param>
        /// <remarks>适用于网格单元值与位置相关的场景（如棋盘按位置设置黑白棋子、地图按坐标生成地形类型）</remarks>
        /// <example>
        /// <code>
        /// // 填充5x3的网格，使每个单元值 = 行索引 * 10 + 列索引
        /// var grid = new EasyGrid&lt;int>(5, 3);
        /// grid.Fill((row, col) => row * 10 + col);
        /// // 结果：(0,0)=0, (0,1)=1, ..., (2,3)=23
        /// </code>
        /// </example>
        public void Fill(Func<int, int, TValue> onFill)
        {
            for (var i = 0; i < Row; i++)
            {
                for (var j = 0; j < Column; j++)
                {
                    _grid[i, j] = onFill(i, j);
                }
            }
        }

        /// <summary>
        /// 调整网格的行列大小，并对新增的行列单元执行自定义初始化逻辑
        /// </summary>
        /// <param name="row">调整后的总行数</param>
        /// <param name="column">调整后的总列数</param>
        /// <param name="onAdd">新增单元初始化回调：参数1为新增单元的行索引，参数2为列索引，返回初始化值</param>
        /// <example>
        /// <code>
        /// // 初始化3x3的网格并填充初始值
        /// var grid = new EasyGrid&lt;int>(3, 3);
        /// grid.Fill((r, c) => r * 3 + c);
        ///  
        /// // 将网格调整为5x4，新增单元值设为-1
        /// grid.Resize(5, 4, (r, c) => -1);
        /// // 结果：原3x3区域保留原值，新增的(0,3)、(1,3)、(2,3)及第4行（索引3、4）所有列均为-1
        /// </code>
        /// </example>
        public void Resize(int row, int column, Func<int, int, TValue> onAdd)
        {
            var newGrid = new TValue[row, column];

            var minRow    = Mathf.Min(Row, row);
            var minColumn = Mathf.Min(Column, column);

            // 复制原有网格的重叠区域数据（行≤minRow-1，列≤minColumn-1）
            for (var i = 0; i < minRow; i++)
            {
                for (var j = 0; j < minColumn; j++)
                {
                    newGrid[i, j] = _grid[i, j];
                }

                // 处理当前行的新增列（列≥minColumn）
                for (int j = minColumn; j < column; j++)
                {
                    newGrid[i, j] = onAdd(i, j);
                }
            }

            // 处理新增行（行≥minRow）的所有列
            for (var i = minRow; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    newGrid[i, j] = onAdd(i, j);
                }
            }

            // 清空原网格数据（避免内存泄漏，尤其当TValue为引用类型时）
            Fill(default(TValue));

            // 替换为新网格
            _grid = newGrid;
        }

        /// <summary>
        /// 遍历网格中所有单元，执行带行列索引的自定义逻辑（按行优先顺序遍历）
        /// </summary>
        /// <param name="each">遍历回调函数：参数1为行索引，参数2为列索引，参数3为当前网格单元数据</param>
        /// <remarks>遍历顺序固定为“行从0到Row-1，列从0到Column-1”，适合需要按位置顺序处理的场景（如按行列绘制网格、批量计算位置相关逻辑）</remarks>
        /// <example>
        /// <code>
        /// // 遍历5x3的网格，打印每个单元的位置与值
        /// var grid = new EasyGrid&lt;int>(5, 3);
        /// grid.ForEach((row, col, value) => 
        /// {
        ///     Debug.Log($"Row: {row}, Col: {col}, Value: {value}");
        /// });
        /// </code>
        /// </example>
        public void ForEach(Action<int, int, TValue> each)
        {
            for (var i = 0; i < Row; i++)
            {
                for (var j = 0; j < Column; j++)
                {
                    each(i, j, _grid[i, j]);
                }
            }
        }

        /// <summary>
        /// 遍历网格中所有单元，仅执行数据相关的自定义逻辑（按行优先顺序遍历）
        /// </summary>
        /// <param name="each">遍历回调函数：参数为当前网格单元数据</param>
        /// <remarks>遍历顺序固定为“行从0到Row-1，列从0到Column-1”，适合仅需处理数据、无需位置信息的场景（如批量释放资源、数据汇总）</remarks>
        /// <example>
        /// <code>
        /// // 遍历网格，销毁所有单元对应的GameObject
        /// var grid = new EasyGrid&lt;GameObject>(5, 3);
        /// grid.ForEach(obj => 
        /// {
        ///     if (obj != null) Destroy(obj);
        /// });
        /// </code>
        /// </example>
        public void ForEach(Action<TValue> each)
        {
            for (var i = 0; i < Row; i++)
            {
                for (var j = 0; j < Column; j++)
                {
                    each(_grid[i, j]);
                }
            }
        }

        /// <summary>
        /// 清空网格中所有单元数据并释放网格引用（可选执行单元清理逻辑）
        /// </summary>
        /// <param name="cleanUpItem">单元清理回调（可选）：清空前对每个单元数据执行的自定义清理逻辑（如释放引用类型资源、注销事件）</param>
        /// <example>
        /// <code>
        /// // 清空网格并销毁所有单元的GameObject，最后释放网格引用
        /// var grid = new EasyGrid&lt;GameObject>(5, 3);
        /// grid.Clear(obj => 
        /// {
        ///     if (obj != null) Destroy(obj);
        /// });
        ///  
        /// // 直接清空网格（无额外清理，适用于值类型）
        /// var intGrid = new EasyGrid&lt;int>(5, 3);
        /// intGrid.Clear();
        /// </code>
        /// </example>
        public void Clear(Action<TValue> cleanUpItem = null)
        {
            for (var i = 0; i < Row; i++)
            {
                for (var j = 0; j < Column; j++)
                {
                    cleanUpItem?.Invoke(_grid[i, j]);
                    _grid[i, j] = default(TValue);
                }
            }

            _grid = null;
        }

        /// <summary>
        /// 实现<see cref="IEnumerable{KeyValuePair{Vector2Int, TValue}}"/>接口，获取网格的枚举器（按行优先顺序）
        /// </summary>
        /// <returns>枚举器：每个元素为键值对（Key为Vector2Int类型位置，Value为单元数据）</returns>
        /// <remarks>支持foreach遍历网格，自动将行列索引转换为Vector2Int（x=行，y=列），简化外部使用</remarks>
        public IEnumerator<KeyValuePair<Vector2Int, TValue>> GetEnumerator()
        {
            for (var i = 0; i < Row; i++)
            {
                for (var j = 0; j < Column; j++)
                {
                    yield return new KeyValuePair<Vector2Int, TValue>(
                        new Vector2Int(i, j),
                        _grid[i, j]
                    );
                }
            }
        }

        /// <summary>
        /// 实现<see cref="IEnumerable"/>接口，获取非泛型枚举器（内部调用泛型枚举器实现）
        /// </summary>
        /// <returns>非泛型枚举器</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}