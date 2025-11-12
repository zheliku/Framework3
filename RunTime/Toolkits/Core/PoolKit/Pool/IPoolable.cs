// ------------------------------------------------------------
// @file       IPoolable.cs
// @brief
// @author     zheliku
// @Modified   2024-10-23 21:10:45
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.PoolKit
{
    /// <summary>
    ///     可放进 ObjectPool 的对象接口.
    /// </summary>
    public interface IPoolable
    {
        bool IsInPool   { get; set; }
        void OnCreate() { }

        void OnGet();

        void OnRelease();

        void OnDestroy();
    }

    public static class IPoolableExtension
    {
        public static void Release2Pool<T>(this T poolable) where T : class, IPoolable, new()
        {
            SingletonPool<T>.Release(poolable);
        }
    }
}