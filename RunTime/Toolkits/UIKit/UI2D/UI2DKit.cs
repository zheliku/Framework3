// ------------------------------------------------------------
// @file       UI2DKit.cs
// @brief
// @author     zheliku
// @Modified   2024-12-14 11:12:49
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.UIKit
{
    using System;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public class UI2DKit
    {
        public static AsyncOperationHandle<GameObject> LoadPanelAsync<T>(Action<T> callback = null, UILevel level = UILevel.Common) where T : IPanel2D
        {
            return LoadPanelAsync<T>(typeof(T).Name, callback, level); 
        }

        public static AsyncOperationHandle<GameObject> LoadPanelAsync<T>(string panelName, Action<T> callback = null, UILevel level = UILevel.Common) where T : IPanel2D
        {
            return UI2DMgr.Instance.LoadPanelAsync<T>(panelName, callback, level);
        }
        
        public static T LoadPanel<T>(UILevel level = UILevel.Common) where T : IPanel2D
        {
            return LoadPanel<T>(typeof(T).Name, level);
        }
        
        public static T LoadPanel<T>(string panelName, UILevel level = UILevel.Common) where T : IPanel2D
        {
            var handle = LoadPanelAsync<T>(panelName, null, level);
            return handle.WaitForCompletion().GetComponent<T>();
        }
        
        public static T GetPanel<T>(string panelName) where T : IPanel2D
        {
            return UI2DMgr.Instance.GetPanel<T>(panelName);
        }
        
        public static T GetPanel<T>() where T : IPanel2D
        {
            return UI2DMgr.Instance.GetPanel<T>(typeof(T).Name);
        }

        public static void UnloadPanel<T>(Action callback = null) where T : IPanel2D
        {
            UnloadPanel(typeof(T).Name, callback);
        }

        public static void UnloadPanel(string panelName, Action callback = null)
        {
             UI2DMgr.Instance.UnloadPanel(panelName, callback);
        }

        public static AsyncOperationHandle<GameObject> ShowPanelAsync<T>(Action<T> callback = null, UILevel level = UILevel.Common) where T : IPanel2D
        {
            return ShowPanelAsync<T>(typeof(T).Name, callback, level);
        }

        public static AsyncOperationHandle<GameObject> ShowPanelAsync<T>(string panelName, Action<T> callback = null, UILevel level = UILevel.Common) where T : IPanel2D
        {
            return UI2DMgr.Instance.ShowPanelAsync<T>(panelName, callback, level);
        }
        
        public static T ShowPanel<T>(UILevel level = UILevel.Common) where T : IPanel2D
        {
            return ShowPanel<T>(typeof(T).Name, level); // 仅在 _panels 中存在时才返回值，否则返回 null
        }
        
        public static T ShowPanel<T>(string panelName, UILevel level = UILevel.Common) where T : IPanel2D
        {
            var handle = ShowPanelAsync<T>(panelName, null, level);
            return handle.WaitForCompletion().GetComponent<T>();
        }
        
        public static void HidePanel<T>(Action<T> callback = null) where T : IPanel2D
        {
            HidePanel<T>(typeof(T).Name, callback);
        }

        public static void HidePanel<T>(string panelName, Action<T> callback = null) where T : IPanel2D
        {
            UI2DMgr.Instance.HidePanel<T>(panelName, callback);
        }

        public static void HideAllPanel(Action<IPanel2D> callback = null)
        {
            UI2DMgr.Instance.HideAllPanel(callback);
        }

        public static void UnloadAllPanel(Action callback = null)
        {
            UI2DMgr.Instance.UnloadAllPanel(callback);
        }

        public static bool IsPanelShown<T>(string panelName) where T : IPanel2D
        {
            var panel = GetPanel<T>(panelName);
            return panel != null && panel.State == PanelState.Shown;
        }
        
        public static bool IsPanelShown<T>() where T : IPanel2D
        {
            return IsPanelShown<T>(typeof(T).Name);
        }
        
        public static void TogglePanelAsync<T>(string panelName, Action<T, bool> callback = null, UILevel level = UILevel.Common) where T : IPanel2D
        {
            if (IsPanelShown<T>(panelName))
            {
                HidePanel<T>(panelName, panel =>
                {
                    callback?.Invoke(panel, false);
                });
            }
            else
            {
                ShowPanelAsync<T>(panelName, panel =>
                {
                    callback?.Invoke(panel, true);
                }, level);
            }
        }
        
        public static void TogglePanelAsync<T>(Action<T, bool> callback = null, UILevel level = UILevel.Common) where T : IPanel2D
        {
            TogglePanelAsync<T>(typeof(T).Name, callback, level);
        }
        
        public static void TogglePanel<T>(string panelName, UILevel level = UILevel.Common) where T : IPanel2D
        {
            if (IsPanelShown<T>(panelName))
            {
                HidePanel<T>(panelName);
            }
            else
            {
                ShowPanel<T>(panelName, level);
            }
        }
        
        public static void TogglePanel<T>(UILevel level = UILevel.Common) where T : IPanel2D
        {
            TogglePanel<T>(typeof(T).Name, level);
        }
    }
}