// ------------------------------------------------------------
// @file       10.UnityEngineOthersExtension.cs
// @brief      Unity相关类型的随机扩展方法
// @author     zheliku
// @Modified   2024-12-17 21:12:22
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.FluentAPI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Random = UnityEngine.Random;

    /// <summary>
    /// 提供Unity随机数相关的扩展方法
    /// </summary>
    public static class UnityEngineRandomExtension
    {
        /// <summary>
        /// 从列表中随机获取一个元素
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="list">要从中获取元素的列表</param>
        /// <returns>随机选中的元素</returns>
        /// <exception cref="ArgumentNullException">当列表为null时抛出</exception>
        /// <exception cref="ArgumentException">当列表为空时抛出</exception>
        /// <example>
        /// <code>
        /// var list = new List&lt;int&gt; {1, 2, 3, 4};
        /// int randomItem = list.RandomTakeOne();
        /// </code>
        /// </example>
        public static T RandomTakeOne<T>(this IList<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        /// <summary>
        /// 从可枚举集合中随机获取一个元素
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="enumerable">要从中获取元素的可枚举集合</param>
        /// <returns>随机选中的元素</returns>
        /// <exception cref="ArgumentNullException">当集合为null时抛出</exception>
        /// <exception cref="ArgumentException">当集合为空时抛出</exception>
        /// <example>
        /// <code>
        /// var array = new int[] {1, 2, 3, 4};
        /// int randomItem = array.RandomTakeOne();
        /// </code>
        /// </example>
        public static T RandomTakeOne<T>(this IEnumerable<T> enumerable)
        {
            var list = enumerable.ToList();
            return list[Random.Range(0, list.Count)];
        }

        /// <summary>
        /// 从列表中随机获取一个元素并将其从列表中移除
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="list">要操作的列表</param>
        /// <returns>随机选中并移除的元素</returns>
        /// <exception cref="ArgumentNullException">当列表为null时抛出</exception>
        /// <exception cref="ArgumentException">当列表为空时抛出</exception>
        /// <example>
        /// <code>
        /// var list = new List&lt;int&gt; {1, 2, 3, 4};
        /// int removedItem = list.RandomTakeOneAndRemove();
        /// // list现在包含3个元素
        /// </code>
        /// </example>
        public static T RandomTakeOneAndRemove<T>(this IList<T> list)
        {
            var randomIndex = Random.Range(0, list.Count);
            var randomItem  = list[randomIndex];
            list.RemoveAt(randomIndex);
            return randomItem;
        }

        /// <summary>
        /// 从列表中随机获取指定数量的元素
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="list">要从中获取元素的列表</param>
        /// <param name="count">要获取的元素数量</param>
        /// <returns>包含随机元素的新列表</returns>
        /// <remarks>如果请求的数量超过列表元素总数，则返回包含所有元素的新列表</remarks>
        /// <exception cref="ArgumentNullException">当列表为null时抛出</exception>
        /// <exception cref="ArgumentOutOfRangeException">当count为负数时抛出</exception>
        /// <example>
        /// <code>
        /// var list = new List&lt;int&gt; {1, 2, 3, 4, 5};
        /// var randomItems = list.RandomTake(2);
        /// // randomItems包含2个随机元素
        /// </code>
        /// </example>
        public static IList<T> RandomTake<T>(this IList<T> list, int count)
        {
            // 如果要求的数量超过了总元素数量，则返回全部元素
            count = Math.Min(count, list.Count);

            var newList = new List<T>(list);

            // 随机移除元素，直到剩余元素数量等于 count
            for (int i = 0; i < list.Count - count; i++)
            {
                var randomIndex = Random.Range(0, list.Count);
                list.RemoveAt(randomIndex);
            }

            return newList;
        }

        /// <summary>
        /// 从可枚举集合中随机获取指定数量的元素
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="enumerable">要从中获取元素的可枚举集合</param>
        /// <param name="count">要获取的元素数量</param>
        /// <returns>包含随机元素的可枚举集合</returns>
        /// <remarks>如果请求的数量超过集合元素总数，则返回所有元素</remarks>
        /// <exception cref="ArgumentNullException">当集合为null时抛出</exception>
        /// <exception cref="ArgumentOutOfRangeException">当count为负数时抛出</exception>
        /// <example>
        /// <code>
        /// var array = new int[] {1, 2, 3, 4, 5};
        /// var randomItems = array.RandomTake(3).ToList();
        /// // randomItems包含3个随机元素
        /// </code>
        /// </example>
        public static IEnumerable<T> RandomTake<T>(this IEnumerable<T> enumerable, int count)
        {
            var list = enumerable.ToList(); // 将源转换为列表

            // 如果要求的数量超过了总元素数量，则返回全部元素
            count = Math.Min(count, list.Count);

            for (int i = 0; i < count; i++)
            {
                var randomIndex = Random.Range(0, list.Count);
                var randomItem  = list[randomIndex];
                list.RemoveAt(randomIndex);
                yield return randomItem;
            }
        }

        /// <summary>
        /// 从列表中随机获取指定数量的元素并将这些元素从原列表中移除
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="list">要操作的列表</param>
        /// <param name="count">要获取并移除的元素数量</param>
        /// <returns>包含随机元素的新列表</returns>
        /// <remarks>如果请求的数量超过列表元素总数，则返回并移除所有元素</remarks>
        /// <exception cref="ArgumentNullException">当列表为null时抛出</exception>
        /// <exception cref="ArgumentOutOfRangeException">当count为负数时抛出</exception>
        /// <example>
        /// <code>
        /// var list = new List&lt;int&gt; {1, 2, 3, 4, 5};
        /// var removedItems = list.RandomTakeAndRemove(2);
        /// // list现在包含3个元素，removedItems包含2个随机元素
        /// </code>
        /// </example>
        public static IList<T> RandomTakeAndRemove<T>(this IList<T> list, int count)
        {
            // 如果要求的数量超过了总元素数量，则返回全部元素
            count = Math.Min(count, list.Count);

            var newList = new List<T>(count);

            for (int i = 0; i < count; i++)
            {
                newList.Add(list.RandomTakeOneAndRemove());
            }

            return newList;
        }

        /// <summary>
        /// 对IList进行随机打乱顺序（原地洗牌）
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="source">要打乱顺序的集合</param>
        /// <returns>打乱顺序后的原集合（与输入为同一实例）</returns>
        /// <remarks>使用Fisher-Yates洗牌算法，时间复杂度为O(n)</remarks>
        /// <exception cref="ArgumentNullException">当源集合为null时抛出</exception>
        /// <example>
        /// <code>
        /// var list = new List&lt;int&gt; {1, 2, 3, 4, 5};
        /// list.Shuffle();
        /// // list现在是打乱顺序的
        /// </code>
        /// </example>
        public static IList<T> Shuffle<T>(this IList<T> source)
        {
            // Fisher-Yates 洗牌算法
            for (int i = source.Count - 1; i > 0; i--)
            {
                int j = (0, i + 1).RandomSelect(); // 生成随机索引

                // 交换元素
                (source[i], source[j]) = (source[j], source[i]);
            }

            return source;
        }

        /// <summary>
        /// 对IEnumerable进行随机打乱顺序
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="source">要打乱顺序的可枚举集合</param>
        /// <returns>包含打乱顺序元素的新集合</returns>
        /// <remarks>使用Fisher-Yates洗牌算法，会先将集合转换为列表</remarks>
        /// <exception cref="ArgumentNullException">当源集合为null时抛出</exception>
        /// <example>
        /// <code>
        /// var array = new int[] {1, 2, 3, 4, 5};
        /// var shuffled = array.Shuffle().ToList();
        /// // shuffled是打乱顺序的新列表
        /// </code>
        /// </example>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            // 将 IEnumerable 转换为列表以便随机访问
            var list = source.ToList();

            // Fisher-Yates 洗牌算法
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = (0, i + 1).RandomSelect(); // 生成随机索引

                // 交换元素
                (list[i], list[j]) = (list[j], list[i]);
            }

            return list;
        }

        /// <summary>
        /// 从Vector2表示的区间[vec2.x, vec2.y]中随机选择一个浮点数
        /// </summary>
        /// <param name="vec2">表示区间的向量，x为最小值，y为最大值</param>
        /// <returns>区间内的随机浮点数</returns>
        /// <example>
        /// <code>
        /// Vector2 range = new Vector2(1.5f, 3.5f);
        /// float randomValue = range.RandomSelect();
        /// // randomValue是1.5f到3.5f之间的随机数
        /// </code>
        /// </example>
        public static float RandomSelect(this Vector2 vec2)
        {
            return Random.Range(vec2.x, vec2.y);
        }

        /// <summary>
        /// 从整数元组表示的区间[range.a, range.b)中随机选择一个整数
        /// </summary>
        /// <param name="range">表示区间的元组，a为最小值（包含），b为最大值（不包含）</param>
        /// <returns>区间内的随机整数</returns>
        /// <example>
        /// <code>
        /// var range = (1, 5);
        /// int randomValue = range.RandomSelect();
        /// // randomValue可能是1、2、3或4
        /// </code>
        /// </example>
        public static int RandomSelect(this (int a, int b) range)
        {
            return Random.Range(range.a, range.b);
        }

        /// <summary>
        /// 从浮点数元组表示的区间[range.a, range.b]中随机选择一个浮点数
        /// </summary>
        /// <param name="range">表示区间的元组，a为最小值，b为最大值</param>
        /// <returns>区间内的随机浮点数</returns>
        /// <example>
        /// <code>
        /// var range = (1.5f, 3.5f);
        /// float randomValue = range.RandomSelect();
        /// // randomValue是1.5f到3.5f之间的随机数
        /// </code>
        /// </example>
        public static float RandomSelect(this (float a, float b) range)
        {
            return Random.Range(range.a, range.b);
        }

        /// <summary>
        /// 从区间[0, a]中随机选择一个浮点数
        /// </summary>
        /// <param name="a">区间的最大值</param>
        /// <returns>0到a之间的随机浮点数（包含0，不包含a）</returns>
        /// <example>
        /// <code>
        /// float max = 5.0f;
        /// float randomValue = max.RandomTo0();
        /// // randomValue是0到5.0f之间的随机数
        /// </code>
        /// </example>
        public static float RandomTo0(this float a)
        {
            return Random.Range(0, a);
        }

        /// <summary>
        /// 从区间[a, b]中随机选择一个浮点数
        /// </summary>
        /// <param name="a">区间的最小值</param>
        /// <param name="b">区间的最大值</param>
        /// <returns>a到b之间的随机浮点数</returns>
        /// <example>
        /// <code>
        /// float randomValue = 2.0f.RandomToY(5.0f);
        /// // randomValue是2.0f到5.0f之间的随机数
        /// </code>
        /// </example>
        public static float RandomToY(this float a, float b)
        {
            return Random.Range(a, b);
        }

        /// <summary>
        /// 从区间[-a, a]中随机选择一个浮点数
        /// </summary>
        /// <param name="a">区间的绝对值范围</param>
        /// <returns>-a到a之间的随机浮点数</returns>
        /// <example>
        /// <code>
        /// float range = 3.0f;
        /// float randomValue = range.RandomToNeg();
        /// // randomValue是-3.0f到3.0f之间的随机数
        /// </code>
        /// </example>
        public static float RandomToNeg(this float a)
        {
            return Random.Range(-a, a);
        }

        /// <summary>
        /// 判断当前值是否小于等于[0, 1]区间内的随机数
        /// </summary>
        /// <param name="value">要比较的值（建议在0到1之间）</param>
        /// <returns>如果随机数小于等于当前值则返回true，否则返回false</returns>
        /// <remarks>可用于实现概率判断，例如value=0.3f时有30%概率返回true</remarks>
        /// <example>
        /// <code>
        /// // 30%概率执行某些操作
        /// if (0.3f.LessThanRandom01())
        /// {
        ///     // 执行操作
        /// }
        /// </code>
        /// </example>
        public static bool LessThanRandom01(this float value)
        {
            return Random.Range(0f, 1f) <= value;
        }
    }
}