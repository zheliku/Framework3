// ------------------------------------------------------------
// @file       GenericPool.cs
// @brief
// @author     zheliku
// @Modified   2025-08-29 23:09:32
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.PoolKit
{
    /// <summary>
    /// 对象必须继承 IPoolable，且必须有无参构造函数
    /// 提供静态的 Get 和 Release 方法
    /// </summary>
    public class GenericPool<T> where T : class, IPoolable, new()
    {
        private static readonly ObjectPool<T> s_pool = new(
            () =>
            {
                var item = new T();
                item.OnCreate();
                return item;
            },
            actionOnGet: item => item.OnGet(),
            actionOnRelease: item => item.OnRelease(),
            actionOnDestroy: item => item.OnDestroy(),
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 100
        );

        public static T Get()
        {
            return s_pool.Get();
        }
        
        public static PooledObject<T> Get(out T value)
        {
            return new PooledObject<T>(value = s_pool.Get(), s_pool);
        }

        public static void Release(T toRelease)
        {
            s_pool.Release(toRelease);
        }
    }
}