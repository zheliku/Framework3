// ------------------------------------------------------------
// @file       10.UnityEngineOthersExtension.cs
// @brief
// @author     zheliku
// @Modified   2024-12-17 21:12:22
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.FluentAPI
{
    using UnityEngine;

    public static class UnityEngineMathfExtension
    {
        /// <summary>
        ///     使用线性插值计算两个值之间的插值。
        /// </summary>
        /// <param name="self">插值因子，范围为 [0, 1]。</param>
        /// <param name="a">起始值。</param>
        /// <param name="b">目标值。</param>
        /// <returns>根据插值因子计算出的值。</returns>
        public static float Lerp(this float self, float a, float b)
        {
            return Mathf.Lerp(a, b, self);
        }

        /// <summary>
        ///     使用线性插值计算两个角度之间的插值。
        /// </summary>
        /// <param name="self">插值因子，范围为 [0, 1]。</param>
        /// <param name="angle1">起始角度。</param>
        /// <param name="angle2">目标角度。</param>
        /// <returns>根据插值因子计算出的角度值。</returns>
        public static float LerpAngle(this float self, float angle1, float angle2)
        {
            return Mathf.LerpAngle(angle1, angle2, self);
        }

        /// <summary>
        ///     平滑过渡
        /// </summary>
        /// <param name="self">起始与目标值</param>
        /// <param name="speed">过渡速度</param>
        /// <returns></returns>
        public static float LerpWithSpeed(this (float start, float target) self, float speed)
        {
            return Mathf.Lerp(self.start, self.target, 1f - Mathf.Exp(-speed));
        }

        /// <summary>
        ///     平滑过渡（角度）
        /// </summary>
        /// <param name="self">起始与目标值</param>
        /// <param name="speed">过渡速度</param>
        /// <returns></returns>
        public static float LerpAngleWithSpeed(this (float start, float target) self, float speed)
        {
            return Mathf.LerpAngle(self.start, self.target, 1f - Mathf.Exp(-speed));
        }

        /// <summary>
        ///     获取浮点数的绝对值。
        /// </summary>
        /// <param name="self">浮点数值。</param>
        /// <returns>浮点数的绝对值。</returns>
        public static float Abs(this float self)
        {
            return Mathf.Abs(self);
        }

        /// <summary>
        ///     获取整数的绝对值。
        /// </summary>
        /// <param name="self">整数值。</param>
        /// <returns>整数的绝对值。</returns>
        public static int Abs(this int self)
        {
            return Mathf.Abs(self);
        }

        /// <summary>
        ///     计算浮点数与目标浮点数之间的绝对差值。
        /// </summary>
        /// <param name="self">浮点数值。</param>
        /// <param name="to">目标浮点数。</param>
        /// <returns>浮点数与目标浮点数之间的绝对差值。</returns>
        public static float AbsTo(this float self, float to)
        {
            return Mathf.Abs(self - to);
        }

        /// <summary>
        ///     计算整数与目标浮点数之间的绝对差值。
        /// </summary>
        /// <param name="self">整数值。</param>
        /// <param name="to">目标浮点数。</param>
        /// <returns>整数与目标浮点数之间的绝对差值。</returns>
        public static float AbsTo(this int self, float to)
        {
            return Mathf.Abs(self - to);
        }

        /// <summary>
        ///     计算整数与目标整数之间的绝对差值。
        /// </summary>
        /// <param name="self">整数值。</param>
        /// <param name="to">目标整数。</param>
        /// <returns>整数与目标整数之间的绝对差值。</returns>
        public static float AbsTo(this int self, int to)
        {
            return Mathf.Abs(self - to);
        }

        /// <summary>
        ///     计算浮点数与目标整数之间的绝对差值。
        /// </summary>
        /// <param name="self">浮点数值。</param>
        /// <param name="to">目标整数。</param>
        /// <returns>浮点数与目标整数之间的绝对差值。</returns>
        public static float AbsTo(this float self, int to)
        {
            return Mathf.Abs(self - to);
        }

        /// <summary>
        ///     判断两个浮点数是否近似相等。
        /// </summary>
        /// <param name="self">浮点数值。</param>
        /// <param name="other">另一个浮点数。</param>
        /// <returns>如果两个浮点数近似相等，则返回 true；否则返回 false。</returns>
        public static bool Approximately(this float self, float other)
        {
            return Mathf.Approximately(self, other);
        }

        /// <summary>
        ///     判断浮点数与整数是否近似相等。
        /// </summary>
        /// <param name="self">浮点数值。</param>
        /// <param name="other">整数值。</param>
        /// <returns>如果浮点数与整数近似相等，则返回 true；否则返回 false。</returns>
        public static bool Approximately(this float self, int other)
        {
            return Mathf.Approximately(self, other);
        }

        /// <summary>
        ///     判断两个整数是否近似相等。
        /// </summary>
        /// <param name="self">整数值。</param>
        /// <param name="other">另一个整数。</param>
        /// <returns>如果两个整数近似相等，则返回 true；否则返回 false。</returns>
        public static bool Approximately(this int self, int other)
        {
            return Mathf.Approximately(self, other);
        }

        /// <summary>
        ///     判断整数与浮点数是否近似相等。
        /// </summary>
        /// <param name="self">整数值。</param>
        /// <param name="other">浮点数值。</param>
        /// <returns>如果整数与浮点数近似相等，则返回 true；否则返回 false。</returns>
        public static bool Approximately(this int self, float other)
        {
            return Mathf.Approximately(self, other);
        }

        /// <summary>
        ///     计算浮点数的指数值（e^x）。
        /// </summary>
        /// <param name="self">浮点数值。</param>
        /// <returns>浮点数的指数值。</returns>
        public static float Exp(this float self)
        {
            return Mathf.Exp(self);
        }

        /// <summary>
        ///     计算整数的指数值（e^x）。
        /// </summary>
        /// <param name="self">整数值。</param>
        /// <returns>整数的指数值。</returns>
        public static float Exp(this int self)
        {
            return Mathf.Exp(self);
        }

        /// <summary>
        ///     计算浮点数的自然对数值（ln(x)）。
        /// </summary>
        /// <param name="self">浮点数值。</param>
        /// <returns>浮点数的自然对数值。</returns>
        public static float Ln(this float self)
        {
            return Mathf.Log(self);
        }

        /// <summary>
        ///     计算整数的自然对数值（ln(x)）。
        /// </summary>
        /// <param name="self">整数值。</param>
        /// <returns>整数的自然对数值。</returns>
        public static float Ln(this int self)
        {
            return Mathf.Log(self);
        }

        /// <summary>
        ///     计算浮点数的以 10 为底的对数值（log10(x)）。
        /// </summary>
        /// <param name="self">浮点数值。</param>
        /// <returns>浮点数的以 10 为底的对数值。</returns>
        public static float Log10(this float self)
        {
            return Mathf.Log10(self);
        }

        /// <summary>
        ///     计算整数的以 10 为底的对数值（log10(x)）。
        /// </summary>
        /// <param name="self">整数值。</param>
        /// <returns>整数的以 10 为底的对数值。</returns>
        public static float Log10(this int self)
        {
            return Mathf.Log10(self);
        }

        /// <summary>
        ///     计算浮点数的以指定底数的对数值（log_base(x)）。
        /// </summary>
        /// <param name="self">浮点数值。</param>
        /// <param name="newBase">对数的底数。</param>
        /// <returns>浮点数的以指定底数的对数值。</returns>
        public static float Log(this float self, float newBase)
        {
            return Mathf.Log(self, newBase);
        }

        /// <summary>
        ///     计算整数的以指定底数的对数值（log_base(x)）。
        /// </summary>
        /// <param name="self">整数值。</param>
        /// <param name="newBase">对数的底数。</param>
        /// <returns>整数的以指定底数的对数值。</returns>
        public static float Log(this int self, float newBase)
        {
            return Mathf.Log(self, newBase);
        }

        /// <summary>
        ///     计算浮点数的幂值（self^power）。
        /// </summary>
        /// <param name="self">底数。</param>
        /// <param name="power">指数。</param>
        /// <returns>浮点数的幂值。</returns>
        public static float Pow(this float self, float power)
        {
            return Mathf.Pow(self, power);
        }

        /// <summary>
        ///     计算整数的幂值（self^power）。
        /// </summary>
        /// <param name="self">底数。</param>
        /// <param name="power">指数。</param>
        /// <returns>整数的幂值。</returns>
        public static float Pow(this int self, float power)
        {
            return Mathf.Pow(self, power);
        }

        /// <summary>
        ///     计算浮点数的平方根。
        /// </summary>
        /// <param name="self">浮点数值。</param>
        /// <returns>浮点数的平方根。</returns>
        public static float Sqrt(this float self)
        {
            return Mathf.Sqrt(self);
        }

        /// <summary>
        ///     计算整数的平方根。
        /// </summary>
        /// <param name="self">整数值。</param>
        /// <returns>整数的平方根。</returns>
        public static float Sqrt(this int self)
        {
            return Mathf.Sqrt(self);
        }

        /// <summary>
        ///     获取浮点数的符号值。
        /// </summary>
        /// <param name="self">浮点数值。</param>
        /// <returns>如果为正数返回 1，负数返回 -1，零返回 0。</returns>
        public static float Sign(this float self)
        {
            return self switch
            {
                < 0 => -1,
                > 0 => 1,
                _   => 0
            };
        }

        /// <summary>
        ///     获取整数的符号值。
        /// </summary>
        /// <param name="self">整数值。</param>
        /// <returns>如果为正数返回 1，负数返回 -1，零返回 0。</returns>
        public static float Sign(this int self)
        {
            return self switch
            {
                < 0 => -1,
                > 0 => 1,
                _   => 0
            };
        }

        /// <summary>
        ///     计算浮点数的余弦值。
        /// </summary>
        /// <param name="self">浮点数值（弧度）。</param>
        /// <returns>浮点数的余弦值。</returns>
        public static float Cos(this float self)
        {
            return Mathf.Cos(self);
        }

        /// <summary>
        ///     计算整数的余弦值。
        /// </summary>
        /// <param name="self">整数值（弧度）。</param>
        /// <returns>整数的余弦值。</returns>
        public static float Cos(this int self)
        {
            return Mathf.Cos(self);
        }

        /// <summary>
        ///     计算浮点数的正弦值。
        /// </summary>
        /// <param name="self">浮点数值（弧度）。</param>
        /// <returns>浮点数的正弦值。</returns>
        public static float Sin(this float self)
        {
            return Mathf.Sin(self);
        }

        /// <summary>
        ///     计算整数的正弦值。
        /// </summary>
        /// <param name="self">整数值（弧度）。</param>
        /// <returns>整数的正弦值。</returns>
        public static float Sin(this int self)
        {
            return Mathf.Sin(self);
        }

        /// <summary>
        ///     计算浮点数的正切值。
        /// </summary>
        /// <param name="self">浮点数值（弧度）。</param>
        /// <returns>浮点数的正切值。</returns>
        public static float Tan(this float self)
        {
            return Mathf.Tan(self);
        }
        /// <summary>
        ///     计算整数的正切值。
        /// </summary>
        /// <param name="self">整数值（弧度）。</param>
        /// <returns>整数的正切值。</returns>
        public static float Tan(this int self)
        {
            return Mathf.Tan(self);
        }

        /// <summary>
        ///     角度转弧度
        /// </summary>
        /// <param name="self">角度</param>
        /// <returns>弧度</returns>
        public static float Deg2Rad(this float self)
        {
            return self * Mathf.Deg2Rad;
        }

        /// <summary>
        ///     角度转弧度
        /// </summary>
        /// <param name="self">角度</param>
        /// <returns>弧度</returns>
        public static float Deg2Rad(this int self)
        {
            return self * Mathf.Deg2Rad;
        }

        /// <summary>
        ///     弧度转角度
        /// </summary>
        /// <param name="self">弧度</param>
        /// <returns>角度</returns>
        public static float Rad2Deg(this float self)
        {
            return self * Mathf.Rad2Deg;
        }

        /// <summary>
        ///     弧度转角度
        /// </summary>
        /// <param name="self">弧度</param>
        /// <returns>角度</returns>
        public static float Rad2Deg(this int self)
        {
            return self * Mathf.Rad2Deg;
        }

        /// <summary>
        ///     计算二维向量的角度（以度为单位）。
        /// </summary>
        /// <param name="self">二维向量</param>
        /// <returns>角度</returns>
        public static float ToAngle(this Vector2 self)
        {
            return Mathf.Atan2(self.y, self.x) * Mathf.Rad2Deg;
        }

        /// <summary>
        ///     将浮点数限制在指定的最小值和最大值之间。
        /// </summary>
        /// <param name="self">浮点数</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>限制后的值</returns>
        public static float Clamp(this float self, float min, float max)
        {
            return Mathf.Clamp(self, min, max);
        }

        /// <summary>
        ///     将整数限制在指定的最小值和最大值之间。
        /// </summary>
        /// <param name="self">浮点数</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns>限制后的值</returns>
        public static float Clamp(this int self, int min, int max)
        {
            return Mathf.Clamp(self, min, max);
        }

        /// <summary>
        ///     将浮点数限制在 0 到 1 之间。
        /// </summary>
        /// <param name="self">浮点数</param>
        /// <returns>限制后的值</returns>
        public static float Clamp01(this float self)
        {
            return Mathf.Clamp01(self);
        }

        /// <summary>
        ///     与指定的最小值进行比较，返回两者中较小的float值
        /// </summary>
        /// <param name="self">当前float值</param>
        /// <param name="min">用于比较的最小值（float类型）</param>
        /// <returns>当前值与最小值中较小的那个float值</returns>
        /// <example>
        ///     <code>
        /// float value = 5.5f;
        /// float result = value.MinWith(3.2f); // result将是3.2f
        /// </code>
        /// </example>
        public static float MinWith(this float self, float min)
        {
            return Mathf.Min(self, min);
        }

        /// <summary>
        ///     与指定的最小值进行比较，返回两者中较小的float值
        /// </summary>
        /// <param name="self">当前float值</param>
        /// <param name="min">用于比较的最小值（int类型）</param>
        /// <returns>当前值与最小值中较小的那个值，转换为float类型返回</returns>
        /// <example>
        ///     <code>
        /// float value = 5.5f;
        /// float result = value.MinWith(3); // result将是3.0f
        /// </code>
        /// </example>
        public static float MinWith(this float self, int min)
        {
            return Mathf.Min(self, min);
        }

        /// <summary>
        ///     与指定的最小值进行比较，返回两者中较小的float值
        /// </summary>
        /// <param name="self">当前int值</param>
        /// <param name="min">用于比较的最小值（float类型）</param>
        /// <returns>当前值与最小值中较小的那个值，转换为float类型返回</returns>
        /// <example>
        ///     <code>
        /// int value = 5;
        /// float result = value.MinWith(3.2f); // result将是3.2f
        /// </code>
        /// </example>
        public static float MinWith(this int self, float min)
        {
            return Mathf.Min(self, min);
        }

        /// <summary>
        ///     与指定的最小值进行比较，返回两者中较小的int值
        /// </summary>
        /// <param name="self">当前int值</param>
        /// <param name="min">用于比较的最小值（int类型）</param>
        /// <returns>当前值与最小值中较小的那个int值</returns>
        /// <example>
        ///     <code>
        /// int value = 5;
        /// int result = value.MinWith(3); // result将是3
        /// </code>
        /// </example>
        public static int MinWith(this int self, int min)
        {
            return Mathf.Min(self, min);
        }

        /// <summary>
        ///     与指定的最大值进行比较，返回两者中较大的float值
        /// </summary>
        /// <param name="self">当前float值</param>
        /// <param name="max">用于比较的最大值（float类型）</param>
        /// <returns>当前值与最大值中较大的那个float值</returns>
        /// <example>
        ///     <code>
        /// float value = 5.5f;
        /// float result = value.MaxWith(8.2f); // result将是8.2f
        /// </code>
        /// </example>
        public static float MaxWith(this float self, float max)
        {
            return Mathf.Max(self, max);
        }

        /// <summary>
        ///     与指定的最大值进行比较，返回两者中较大的float值
        /// </summary>
        /// <param name="self">当前float值</param>
        /// <param name="max">用于比较的最大值（int类型）</param>
        /// <returns>当前值与最大值中较大的那个值，转换为float类型返回</returns>
        /// <example>
        ///     <code>
        /// float value = 5.5f;
        /// float result = value.MaxWith(8); // result将是8.0f
        /// </code>
        /// </example>
        public static float MaxWith(this float self, int max)
        {
            return Mathf.Max(self, max);
        }

        /// <summary>
        ///     与指定的最大值进行比较，返回两者中较大的float值
        /// </summary>
        /// <param name="self">当前int值</param>
        /// <param name="max">用于比较的最大值（float类型）</param>
        /// <returns>当前值与最大值中较大的那个值，转换为float类型返回</returns>
        /// <example>
        ///     <code>
        /// int value = 5;
        /// float result = value.MaxWith(8.2f); // result将是8.2f
        /// </code>
        /// </example>
        public static float MaxWith(this int self, float max)
        {
            return Mathf.Max(self, max);
        }

        /// <summary>
        ///     与指定的最大值进行比较，返回两者中较大的int值
        /// </summary>
        /// <param name="self">当前int值</param>
        /// <param name="max">用于比较的最大值（int类型）</param>
        /// <returns>当前值与最大值中较大的那个int值</returns>
        /// <example>
        ///     <code>
        /// int value = 5;
        /// int result = value.MaxWith(8); // result将是8
        /// </code>
        /// </example>
        public static int MaxWith(this int self, int max)
        {
            return Mathf.Max(self, max);
        }

        /// <summary>
        ///     将角度值（度）转换为2D方向向量
        /// </summary>
        /// <param name="self">角度值（度），0度表示右方向，逆时针为正方向</param>
        /// <returns>对应的2D方向向量，x分量为水平方向，y分量为垂直方向</returns>
        /// <remarks>
        ///     内部会先将角度转换为弧度，再计算方向向量
        /// </remarks>
        /// <example>
        ///     <code>
        /// float angle = 90f; // 90度（向上）
        /// Vector2 direction = angle.Deg2Direction2D(); // 结果为(0, 1)
        /// </code>
        /// </example>
        public static Vector2 Deg2Direction2D(this float self)
        {
            return new Vector2(Mathf.Cos(self.Deg2Rad()), Mathf.Sin(self.Deg2Rad()));
        }

        /// <summary>
        ///     将角度值（弧度）转换为2D方向向量
        /// </summary>
        /// <param name="self">角度值（弧度），0弧度表示右方向，逆时针为正方向</param>
        /// <returns>对应的2D方向向量，x分量为水平方向，y分量为垂直方向</returns>
        /// <example>
        ///     <code>
        /// float radians = Mathf.PI / 2; // 90度（向上）
        /// Vector2 direction = radians.Rad2Direction2D(); // 结果为(0, 1)
        /// </code>
        /// </example>
        public static Vector2 Rad2Direction2D(this float self)
        {
            return new Vector2(Mathf.Cos(self), Mathf.Sin(self));
        }
    }
}