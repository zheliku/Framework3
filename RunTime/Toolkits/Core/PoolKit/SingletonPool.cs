// ------------------------------------------------------------
// @file       SingletonPool.cs
// @brief
// @author     zheliku
// @Modified   2024-10-23 22:10:42
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.PoolKit
{
    using System;
    using Core;

    /// <summary>
    /// 对象必须继承 IPoolable
    /// </summary>
    public class SingletonPool<T> where T : class, IPoolable, new()
    {
        public static readonly ObjectPool<T> Pool = new(
            () =>
            {
                var item = new T();
                item.OnCreate();
                item.IsInPool = true;
                return item;
            },
            actionOnGet: item => item.OnGet(),
            actionOnRelease: item => item.OnRelease(),
            actionOnDestroy: item => item.OnDestroy(),
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 100,
            preCreate: false
        );

        public static T Get()
        {
            var item = Pool.Get();
            item.IsInPool = false;
            return item;
        }

        public static PooledObject<T> Get(out T value)
        {
            value = Pool.Get();
            value.IsInPool = false;
            return new PooledObject<T>(value, Pool);
        }

        public static bool Release(T toRelease)
        {
            if (toRelease.IsInPool)
            {
                throw new FrameworkException("SingletonPool: The object is already in the pool.");
            }
            
            if (Pool.Release(toRelease))
            {
                toRelease.IsInPool = true;
                return true;
            }
            
            return false;
        }

        public static void Clear(Action<T> onClear = null)
        {
            Pool.Clear(onClear);
        }
    }
}