// ------------------------------------------------------------
// @file       ResMgr.cs
// @brief
// @author     zheliku
// @Modified   2024-12-09 20:12:36
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.ResKit
{
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif
    using System;
    using System.Collections.Generic;
    using FluentAPI;
    using SingletonKit;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using Object = UnityEngine.Object;

    [MonoSingletonPath("Framework3/ResKit")]
    public class ResMgr : MonoSingleton<ResMgr>
    {
    #region Static

        /// <summary>
        ///     记录 AsyncOperationHandle 的附属信息：资源名称
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public static Dictionary<AsyncOperationHandle, string> HandleAssetNameMap = new();

        /// <summary>
        ///     记录 AsyncOperationHandle 的附属信息：资源类型
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public static Dictionary<AsyncOperationHandle, Type> HandleAssetTypeMap = new();

        /// <summary>
        ///     记录 Resources 的附属信息：资源加载路径
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public static Dictionary<Object, string> ResourceAssetPathMap = new();

    #endregion

    #region 字段

        public Transform ResourcesMonoParent;   // 用于挂载 ResourcesMono 的父节点
        public Transform AddressableMonoParent; // 用于挂载 AddressableMono 的父节点

    #endregion

    #region 公共方法

        public override void OnSingletonInit()
        {
            ResourcesMonoParent = new GameObject("Resources").transform;
            ResourcesMonoParent.SetParent(transform);

            AddressableMonoParent = new GameObject("Addressable").transform;
            AddressableMonoParent.SetParent(transform);
        }

        public AddressableMono GetAddressableMono(AsyncOperationHandle handle)
        {
            return $"{handle.AssetType().Name}".GetOrAddComponentInHierarchy<AddressableMono>(AddressableMonoParent);
        }

        public ResourcesMono GetResourcesMono(Object res)
        {
            return $"{res.GetType().Name}".GetOrAddComponentInHierarchy<ResourcesMono>(ResourcesMonoParent);
        }

    #endregion
    }
}