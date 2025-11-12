// ------------------------------------------------------------
// @file       CollectionPool.cs
// @brief
// @author     zheliku
// @Modified   2025-05-15 22:57:53
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.PoolKit
{
    using System;
    using System.Collections.Generic;

    public class CollectionPool<TCollection, TItem> where TCollection : class, ICollection<TItem>, new()
    {
        public static readonly ObjectPool<TCollection> Pool = new(
            () => new TCollection(),
            null,
            l => l.Clear(),
            null,
            defaultCapacity: 5,
            maxSize: 20
        );

        public static TCollection Get()
        {
            return Pool.Get();
        }

        public static void Release(TCollection toRelease)
        {
            Pool.Release(toRelease);
        }

        public static void Clear(Action<TCollection> onClear = null)
        {
            Pool.Clear(onClear);
        }
    }
}