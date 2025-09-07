// ------------------------------------------------------------
// @file       UI2DMgr.cs
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
    public class Panel2DInfo
    {
        [LabelWidth(75)]
        public IPanel2D Panel2D;

        [HideInInspector]
        public AsyncOperationHandle Handle;

        public Panel2DInfo(IPanel2D panel2D, AsyncOperationHandle handle)
        {
            Panel2D  = panel2D;
            Handle = handle;
        }
    }

    public class UI2DMgr : MonoBehaviour, ISingleton
    {
    #region Static

        private static UI2DMgr s_instance;

        public static UI2DMgr Instance
        {
            get => UI2DRoot.Instance.GetComponentInChildren<UI2DMgr>();
        }

    #endregion

    #region 字段

        [ShowInInspector]
        private Dictionary<string, Panel2DInfo> _panels = new Dictionary<string, Panel2DInfo>();

    #endregion

    #region 公共方法

        public AsyncOperationHandle<GameObject> LoadPanelAsync<T>(string panelName, Action<T> callback = null, UILevel level = UILevel.Common) where T : IPanel2D
        {
            if (!_panels.TryGetValue(panelName, out var value))
            {
                var handle = ResKit.InstantiateAsync(panelName);

                value = new Panel2DInfo(null, handle);
                _panels.Add(panelName, value);

                handle.OnCompleted(obj =>
                {
                    obj.name = panelName;
                    var panel = obj.GetComponent<T>();
                    panel.Level = level;
                    value.Panel2D = panel;
                    callback?.Invoke(panel);
                });
            }

            return value.Handle.Convert<GameObject>();
        }

        public T GetPanel<T>(string panelName) where T : IPanel2D
        {
            if (_panels.TryGetValue(panelName, out var value))
            {
                if (value.Handle.IsDone)
                {
                    return (T) value.Panel2D;
                }
            }
            return default;
        }

        public void UnloadPanel<T>(string panelName, Action callback = null) where T : IPanel2D
        {
            if (_panels.TryGetValue(panelName, out var value))
            {
                if (value.Panel2D is not T)
                {
                    Debug.LogError("Panel2D " + panelName + " is not of type " + typeof(T).Name);
                    return;
                }
                
                value.Handle.Release();
                callback?.Invoke();
                _panels.Remove(panelName);
                Destroy(value.Panel2D.Transform.gameObject);
            }
        }
        
        public void UnloadPanel(string panelName, Action callback = null)
        {
            if (_panels.TryGetValue(panelName, out var value))
            {
                value.Handle.Release();
                callback?.Invoke();
                _panels.Remove(panelName);
                Destroy(value.Panel2D.Transform.gameObject);
            }
        }

        public AsyncOperationHandle<GameObject> ShowPanelAsync<T>(string panelName, Action<T> callback = null, UILevel level = UILevel.Common) where T : IPanel2D
        {
            var handle = LoadPanelAsync<T>(panelName);
            
            if (!handle.IsDone)
            {
                handle.OnCompleted(obj =>
                {
                    var panel = obj.GetComponent<T>();
                    panel.Level = level;
                    panel.Show();
                    callback?.Invoke(panel);
                });
            }
            else if (handle.IsValid())
            {
                var panel = handle.Result.GetComponent<T>();
                panel.Level = level;
                panel.Show();
                callback?.Invoke(panel);
            }
            else
            {
                Debug.LogError("Panel2D " + panelName + " is not loaded");
            }

            return handle;
        }

        public void HidePanel<T>(string panelName, Action<T> callback = null) where T : IPanel2D
        {
            if (_panels.TryGetValue(panelName, out var value))
            {
                value.Panel2D.Hide();
                callback?.Invoke((T) value.Panel2D);
            }
        }

        public void HideAllPanel(Action<IPanel2D> callback = null)
        {
            foreach (var info in _panels.Values)
            {
                info.Panel2D.Hide();
                callback?.Invoke(info.Panel2D);
            }
        }

        public void UnloadAllPanel(Action callback = null)
        {
            foreach (var panelName in _panels.Keys)
            {
                UnloadPanel(panelName, callback);
            }
        }

        public void OnSingletonInit() { }

    #endregion

    #region Unity 事件

        private void Update()
        {
#if UNITY_EDITOR

            // 强制刷新 Inspector GUI
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

    #endregion
    }
}