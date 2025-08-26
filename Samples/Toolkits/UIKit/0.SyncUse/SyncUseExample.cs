// ------------------------------------------------------------
// @file       BasicExample.cs
// @brief
// @author     zheliku
// @Modified   2024-12-12 21:12:49
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.UIKit.Example._0.SyncUse
{
    using ResKit;
    using UnityEngine;

    public class SyncUseExample : MonoBehaviour
    {
        private SyncUseExamplePanel2D _panel2D;
        
        private void OnEnable()
        {
            _panel2D = UI2DKit.LoadPanel<SyncUseExamplePanel2D>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                UI2DKit.ShowPanel<SyncUseExamplePanel2D>();
            }
            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _panel2D.Hide();
            }
        }

        private void OnDisable()
        {
            UI2DKit.UnloadPanel<AsyncUseExamplePanel2D>();
        }
    }
}
