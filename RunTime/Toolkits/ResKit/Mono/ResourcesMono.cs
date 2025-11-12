// ------------------------------------------------------------
// @file       ResourcesMono.cs
// @brief
// @author     zheliku
// @Modified   2024-12-27 20:12:15
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.ResKit
{
    using System.Collections.Generic;
    using UnityEngine;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    /// <summary>
    ///     用于在 Inspector 中显示 Resources 资源的 Mono 对象
    /// </summary>
    public class ResourcesMono : MonoBehaviour
    {
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public Dictionary<string, ResourcesMonoInfo> ResMap = new();

        public void BindRes(Object res)
        {
            ResMap.TryAdd(res.AssetPath(), new ResourcesMonoInfo(res.AssetPath(), res));
            AddRef(res);
        }

        public void AddRef(Object res)
        {
            ResMap[res.AssetPath()].RefCount++;
        }

        public void SubRef(Object res)
        {
            if (ResMap.TryGetValue(res.AssetPath(), out var info))
            {
                info.RefCount--;
                if (info.RefCount <= 0) // 资源引用计数为 0，则清空记录
                {
                    ResMap.Remove(res.AssetPath());
                    ResMgr.ResourceAssetPathMap.Remove(res);
                }
            }
        }
    }

    /// <summary>
    ///     显示在 Inspector 中的 Resources Info
    /// </summary>
#if ODIN_INSPECTOR
    [HideReferenceObjectPicker]
#endif
    public class ResourcesMonoInfo
    {
        public ResourcesMonoInfo(string assetPath, Object asset)
        {
            AssetPath = assetPath;
            Asset     = asset;
        }

        /// <summary>
        ///     显示 Resources 的资源加载路径
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector] [LabelWidth(75)]
    #endif
        public string AssetPath { get; private set; }

        /// <summary>
        ///     显示 Resources 的资源
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector] [LabelWidth(75)]
    #endif
        public Object Asset { get; private set; }

        /// <summary>
        ///     显示 Resources 的引用计数
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector] [LabelWidth(75)]
    #endif
        public int RefCount { get; set; }
    }
}