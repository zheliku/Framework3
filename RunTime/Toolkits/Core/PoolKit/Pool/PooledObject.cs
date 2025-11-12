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
        private readonly T              _toReturn;
        private readonly IObjectPool<T> _objectPool;

        void IDisposable.Dispose()
        {
            _objectPool.Release(_toReturn);
        }

        public PooledObject(T value, IObjectPool<T> objectPool)
        {
            _toReturn   = value;
            _objectPool = objectPool;
        }
    }
}