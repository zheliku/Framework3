// ------------------------------------------------------------
// @file       ObjectPool.cs
// @brief      实现一个通用的对象池，用于管理对象的创建、获取、释放和销毁。
// @author     zheliku
// @Modified   2024-10-23 21:10:03
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.PoolKit
{
    using System;
    using System.Collections.Generic;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    /// <summary>
    ///     通用对象池类，用于管理对象的生命周期，减少频繁创建和销毁对象的开销。
    /// </summary>
    /// <typeparam name="T">对象池中管理的对象类型。</typeparam>
#if ODIN_INSPECTOR
    [HideReferenceObjectPicker]
#endif
    public class ObjectPool<T> : IObjectPool<T>
    {
        /// <summary>
        ///     销毁对象时执行的操作。
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        private readonly Action<T> _actionOnDestroy;

        /// <summary>
        ///     获取对象时执行的操作。
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        private readonly Action<T> _actionOnGet;

        /// <summary>
        ///     释放对象时执行的操作。
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        private readonly Action<T> _actionOnRelease;

        /// <summary>
        ///     是否启用重复释放检查。
        /// </summary>
        protected readonly bool _collectionCheck;
        /// <summary>
        ///     用于存储对象的栈。
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        protected Stack<T> _cacheStack;

        /// <summary>
        ///     对象池中所有对象的总数。
        /// </summary>
        protected int _countAll;

        /// <summary>
        ///     对象工厂，用于创建新对象。
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        protected IObjectFactory<T> _factory;

        /// <summary>
        ///     对象池的最大容量，默认值为 100。
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        protected int _maxSize;

        /// <summary>
        ///     初始化对象池。
        /// </summary>
        /// <param name="createFunc">用于创建新对象的委托。</param>
        /// <param name="actionOnGet">获取对象时执行的操作。</param>
        /// <param name="actionOnRelease">释放对象时执行的操作。</param>
        /// <param name="actionOnDestroy">销毁对象时执行的操作。</param>
        /// <param name="collectionCheck">是否启用重复释放检查。</param>
        /// <param name="defaultCapacity">对象池的初始容量。</param>
        /// <param name="maxSize">对象池的最大容量。</param>
        /// <param name="preCreate">是否在初始化时预创建对象。</param>
        public ObjectPool(
            Func<T>   createFunc,
            Action<T> actionOnGet     = null,
            Action<T> actionOnRelease = null,
            Action<T> actionOnDestroy = null,
            bool      collectionCheck = false,
            int       defaultCapacity = 10,
            int       maxSize         = 100,
            bool      preCreate       = false)
        {
            _factory         = new CustomObjectFactory<T>(createFunc);
            _actionOnGet     = actionOnGet;
            _actionOnRelease = actionOnRelease;
            _actionOnDestroy = actionOnDestroy;
            _collectionCheck = collectionCheck;
            _maxSize         = maxSize;
            _cacheStack      = new Stack<T>(defaultCapacity);

            if (preCreate)
            {
                for (var i = 0; i < defaultCapacity; i++)
                {
                    _cacheStack.Push(_factory.Create());
                }
                _countAll = defaultCapacity;
            }
        }

        /// <summary>
        ///     获取对象池中所有对象的总数。
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public int CountAll { get => _countAll; }

        /// <summary>
        ///     获取当前未使用的对象数量。
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public int CountInactive { get => _cacheStack.Count; }

        /// <summary>
        ///     获取当前正在使用的对象数量。
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public int CountActive { get => _countAll - _cacheStack.Count; }

        /// <summary>
        ///     从对象池中获取一个对象。
        /// </summary>
        /// <returns>获取的对象。</returns>
        public T Get()
        {
            T item;
            if (_cacheStack.Count > 0)
            {
                item = _cacheStack.Pop();
            }
            else
            {
                item = _factory.Create();
                ++_countAll;
            }
            _actionOnGet?.Invoke(item);
            return item;
        }

        /// <summary>
        ///     将对象释放回对象池。
        /// </summary>
        /// <param name="obj">要释放的对象。</param>
        /// <returns>如果对象成功释放到池中返回 true，否则返回 false。</returns>
        /// <exception cref="InvalidOperationException">当启用重复释放检查时，尝试释放已存在于池中的对象会抛出此异常。</exception>
        public bool Release(T obj)
        {
            if (_collectionCheck && _cacheStack.Contains(obj))
            {
                throw new InvalidOperationException("Trying to release an object that has already been released to the objectPool.");
            }

            _actionOnRelease?.Invoke(obj);

            if (CountInactive < _maxSize)
            {
                _cacheStack.Push(obj);
                return true;
            }
            --_countAll;
            _actionOnDestroy?.Invoke(obj);
            return false;
        }

        /// <summary>
        ///     清空对象池，并可选地对每个对象执行清理操作。
        /// </summary>
        /// <param name="onClear">清理时对每个对象执行的操作。</param>
        public void Clear(Action<T> onClear = null)
        {
            if (onClear != null)
            {
                foreach (var t in _cacheStack)
                {
                    onClear.Invoke(t);
                }
            }

            if (_actionOnDestroy != null)
            {
                foreach (var t in _cacheStack)
                {
                    _actionOnDestroy.Invoke(t);
                }
            }

            _cacheStack.Clear();
            _countAll = 0;
        }

        /// <summary>
        ///     设置对象工厂。
        /// </summary>
        /// <param name="factory">实现 IObjectFactory 接口的对象工厂实例。</param>
        public void SetObjectFactory(IObjectFactory<T> factory)
        {
            _factory = factory;
        }
    }
}