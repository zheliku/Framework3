// ------------------------------------------------------------
// @file       UI3DKit.cs
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

    public class UI3DKit
    {
        public static AsyncOperationHandle<GameObject> LoadPanelAsync<T>(Action<T> callback = null) where T : IPanel3D
        {
            return LoadPanelAsync(typeof(T).Name, callback);
        }

        public static AsyncOperationHandle<GameObject> LoadPanelAsync<T>(string panelName, Action<T> callback = null) where T : IPanel3D
        {
            return UI3DMgr.Instance.LoadPanelAsync(panelName, callback);
        }

        public static T LoadPanel<T>() where T : IPanel3D
        {
            return LoadPanel<T>(typeof(T).Name);
        }

        public static T LoadPanel<T>(string panelName) where T : IPanel3D
        {
            var handle = LoadPanelAsync<T>(panelName);
            return handle.WaitForCompletion().GetComponent<T>();
        }

        public static T GetPanel<T>(string panelName) where T : IPanel3D
        {
            return UI3DMgr.Instance.GetPanel<T>(panelName);
        }

        public static T GetPanel<T>() where T : IPanel3D
        {
            return UI3DMgr.Instance.GetPanel<T>(typeof(T).Name);
        }

        public static void UnloadPanel<T>(Action callback = null) where T : IPanel3D
        {
            UnloadPanel(typeof(T).Name, callback);
        }

        public static void UnloadPanel<T>(string panelName, Action callback = null) where T : IPanel3D
        {
            UI3DMgr.Instance.UnloadPanel<T>(panelName, callback);
        }

        public static void UnloadPanel(string panelName, Action callback = null)
        {
            UI3DMgr.Instance.UnloadPanel(panelName, callback);
        }

        public static AsyncOperationHandle<GameObject> ShowPanelAsync<T>(Action<T> callback = null) where T : IPanel3D
        {
            return ShowPanelAsync(typeof(T).Name, callback);
        }

        public static AsyncOperationHandle<GameObject> ShowPanelAsync<T>(string panelName, Action<T> callback = null) where T : IPanel3D
        {
            return UI3DMgr.Instance.ShowPanelAsync(panelName, callback);
        }

        public static T ShowPanel<T>() where T : IPanel3D
        {
            return ShowPanel<T>(typeof(T).Name); // 仅在 _panels 中存在时才返回值，否则返回 null
        }

        public static T ShowPanel<T>(string panelName) where T : IPanel3D
        {
            var handle = ShowPanelAsync<T>(panelName);
            return handle.WaitForCompletion().GetComponent<T>();
        }

        public static void HidePanel<T>(Action<T> callback = null) where T : IPanel3D
        {
            HidePanel(typeof(T).Name, callback);
        }

        public static void HidePanel<T>(string panelName, Action<T> callback = null) where T : IPanel3D
        {
            UI3DMgr.Instance.HidePanel(panelName, callback);
        }

        public static void HideAllPanel(Action<IPanel3D> callback = null)
        {
            UI3DMgr.Instance.HideAllPanel(callback);
        }

        public static void UnloadAllPanel(Action callback = null)
        {
            UI3DMgr.Instance.UnloadAllPanel(callback);
        }

        public static bool IsPanelShown<T>(string panelName) where T : IPanel3D
        {
            var panel = GetPanel<T>(panelName);
            return panel != null && panel.State == PanelState.Shown;
        }

        public static bool IsPanelShown<T>() where T : IPanel3D
        {
            return IsPanelShown<T>(typeof(T).Name);
        }
    }
}