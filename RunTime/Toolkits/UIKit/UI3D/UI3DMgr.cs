// ------------------------------------------------------------
// @file       UI3DMgr.cs
// @brief
// @author     zheliku
// @Modified   2024-12-12 19:12:30
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

using UnityEngine;

namespace Framework3.Toolkits.UIKit
{
    using System;
    using System.Collections.Generic;
    using ResKit;
    using SingletonKit;
    using Sirenix.OdinInspector;
    using UnityEngine.ResourceManagement.AsyncOperations;

    [HideReferenceObjectPicker]
    public class Panel3DInfo
    {
        [LabelWidth(75)]
        public IPanel3D Panel3D;

        [HideInInspector]
        public AsyncOperationHandle Handle;

        public Panel3DInfo(IPanel3D panel3D, AsyncOperationHandle handle)
        {
            Panel3D  = panel3D;
            Handle = handle;
        }
    }

    [MonoSingletonPath("Framework3/UI3DKit/UI3DMgr")]
    public class UI3DMgr : MonoSingleton<UI3DMgr>, ISingleton
    {
    #region 字段

        [ShowInInspector]
        private Dictionary<string, Panel3DInfo> _panels = new();

    #endregion

    #region 公共方法

        public AsyncOperationHandle<GameObject> LoadPanelAsync<T>(string panelName, Action<T> callback = null) where T : IPanel3D
        {
            if (!_panels.TryGetValue(panelName, out var value))
            {
                var handle = ResKit.InstantiateAsync(panelName);

                value = new Panel3DInfo(null, handle);
                _panels.Add(panelName, value);

                handle.OnCompleted(obj =>
                {
                    obj.name = panelName;
                    var panel = obj.GetComponent<T>();
                    value.Panel3D = panel;
                    callback?.Invoke(panel);
                });
            }

            return value.Handle.Convert<GameObject>();
        }

        public T GetPanel<T>(string panelName) where T : IPanel3D
        {
            if (_panels.TryGetValue(panelName, out var value))
            {
                if (value.Handle.IsDone)
                {
                    return (T) value.Panel3D;
                }
            }
            return default;
        }
        
        public void UnloadPanel<T>(string panelName, Action callback = null) where T : IPanel3D
        {
            if (_panels.TryGetValue(panelName, out var value))
            {
                if (value.Panel3D is not T)
                {
                    Debug.LogError("Panel3D " + panelName + " is not of type " + typeof(T).Name);
                    return;
                }
                
                value.Handle.Release();
                callback?.Invoke();
                _panels.Remove(panelName);
                Destroy(value.Panel3D.Transform.gameObject);
            }
        }

        public void UnloadPanel(string panelName, Action callback = null)
        {
            if (_panels.TryGetValue(panelName, out var value))
            {
                value.Handle.Release();
                callback?.Invoke();
                _panels.Remove(panelName);
                Destroy(value.Panel3D.Transform.gameObject);
            }
        }

        public AsyncOperationHandle<GameObject> ShowPanelAsync<T>(string panelName, Action<T> callback = null) where T : IPanel3D
        {
            var handle = LoadPanelAsync<T>(panelName);
            
            if (!handle.IsDone)
            {
                handle.OnCompleted(obj =>
                {
                    var panel = obj.GetComponent<T>();
                    panel.Show();
                    callback?.Invoke(panel);
                });
            }
            else if (handle.IsValid())
            {
                var panel = handle.Result.GetComponent<T>();
                panel.Show();
                callback?.Invoke(panel);
            }
            else
            {
                Debug.LogError("Panel3D " + panelName + " is not loaded");
            }

            return handle;
        }

        public void HidePanel<T>(string panelName, Action<T> callback = null) where T : IPanel3D
        {
            if (_panels.TryGetValue(panelName, out var value))
            {
                value.Panel3D.Hide();
                callback?.Invoke((T) value.Panel3D);
            }
        }

        public void HideAllPanel(Action<IPanel3D> callback = null)
        {
            foreach (var info in _panels.Values)
            {
                info.Panel3D.Hide();
                callback?.Invoke(info.Panel3D);
            }
        }

        public void UnloadAllPanel(Action callback = null)
        {
            foreach (var panelName in _panels.Keys)
            {
                UnloadPanel(panelName, callback);
            }
        }

    #endregion
    }
}