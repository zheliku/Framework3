// ------------------------------------------------------------
// @file       PooledObject.cs
// @brief
// @author     zheliku
// @Modified   2025-08-29 23:28:09
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.PoolKit
{
    using System;

    public readonly struct PooledObject<T> : IDisposable where T : class
    {
        private readonly T        _toReturn;
        private readonly IPool<T> _pool;

        void IDisposable.Dispose() => _pool.Release(_toReturn);

        public PooledObject(T value, IPool<T> pool)
        {
            _toReturn = value;
            _pool = pool;
        }
    }
}