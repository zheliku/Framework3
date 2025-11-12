// ------------------------------------------------------------
// @file       ResourceMono.cs
// @brief
// @author     zheliku
// @Modified   2024-12-27 15:12:16
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.ResKit
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;
#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif

    /// <summary>
    ///     用于在 Inspector 中显示 Addressables 资源的 Mono 对象
    /// </summary>
    public class AddressableMono : MonoBehaviour
    {
    #if ODIN_INSPECTOR
        [ShowInInspector]
    #endif
        public Dictionary<string, AddressableMonoInfo> ResMap = new();

        private void Update()
        {
        #if UNITY_EDITOR

            // 强制刷新 Inspector GUI
            EditorUtility.SetDirty(this);
        #endif
        }

        public void BindHandle(AsyncOperationHandle handle)
        {
            ResMap.TryAdd(handle.AssetName(), new AddressableMonoInfo(handle));
            ResMap[handle.AssetName()].RefCount++;
        }

        public void UnbindHandle(AsyncOperationHandle handle)
        {
            if (ResMap.TryGetValue(handle.AssetName(), out var info))
            {
                info.RefCount--;
                if (info.RefCount <= 0) // 如果引用计数为 0，则从字典中移除
                {
                    ResMap.Remove(handle.AssetName());
                    ResMgr.HandleAssetNameMap.Remove(handle);
                    ResMgr.HandleAssetTypeMap.Remove(handle);
                }
            }
        }

        public void AddOnCompletedAction(AsyncOperationHandle handle, Delegate action)
        {
            ResMap[handle.AssetName()].OnCompletedActions.Add(action);
        }

        public void RemoveOnCompletedAction(AsyncOperationHandle handle, Delegate action)
        {
            ResMap[handle.AssetName()].OnCompletedActions.Remove(action);
        }
    }

    /// <summary>
    ///     显示在 Inspector 中的 Addressable Info
    /// </summary>
#if ODIN_INSPECTOR
    [HideReferenceObjectPicker]
#endif
    public class AddressableMonoInfo
    {
        private AsyncOperationHandle _handle;

        public AddressableMonoInfo(AsyncOperationHandle handle)
        {
            _handle = handle;
        }

    #if ODIN_INSPECTOR
        [ShowInInspector] [LabelWidth(75)]
    #endif
        public string Name { get => _handle.AssetName(); }

        /// <summary>
        ///     显示 Addressable 当前状态
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector] [LabelWidth(75)] [EnumToggleButtons]
    #endif
        public AsyncOperationStatus Status
        {
            get => _handle.Status;
        }

        /// <summary>
        ///     显示 Addressable 加载完成的所有回调方法
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector] [LabelWidth(75)]
    #endif
        public List<Delegate> OnCompletedActions { get; private set; } = new();

        /// <summary>
        ///     显示 Addressable 的引用计数
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector] [LabelWidth(75)]
    #endif
        public int RefCount { get; set; }

        /// <summary>
        ///     显示 Addressable 对应的资源
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector] [LabelWidth(75)]
    #endif
        public object Res
        {
            get => _handle.Result;
        }

        /// <summary>
        ///     显示 Addressable 是否加载完成
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector] [LabelWidth(75)]
    #endif
        public bool IsDone
        {
            get => _handle.IsDone;
        }

        /// <summary>
        ///     显示 Addressable 的加载进度
        /// </summary>
    #if ODIN_INSPECTOR
        [ShowInInspector] [LabelWidth(75)]
    #endif
        public float Progress
        {
            get => _handle.PercentComplete;
        }
    }
}