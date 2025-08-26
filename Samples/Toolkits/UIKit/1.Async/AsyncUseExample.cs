// ------------------------------------------------------------
// @file       BasicExample.cs
// @brief
// @author     zheliku
// @Modified   2024-12-12 21:12:49
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.UIKit.Example._1.Async
{
    using _0.SyncUse;
    using UnityEngine;

    public class AsyncUseExample : MonoBehaviour
    {
        private void OnEnable()
        {
            UI2DKit.LoadPanelAsync<AsyncUseExamplePanel2D>(panel =>
            {
                Debug.Log("AsyncUseExamplePanel2D loaded");
            });
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                UI2DKit.ShowPanelAsync<AsyncUseExamplePanel2D>();
            }
            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                UI2DKit.HidePanel<AsyncUseExamplePanel2D>();
            }
        }

        private void OnDisable()
        {
            UI2DKit.UnloadPanel<AsyncUseExamplePanel2D>();
        }
    }
}
