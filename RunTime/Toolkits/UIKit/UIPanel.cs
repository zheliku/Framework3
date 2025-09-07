// ------------------------------------------------------------
// @file       UIPanel.cs
// @brief
// @author     zheliku
// @Modified   2025-09-08 00:30:42
// @Copyright  Copyright (c) 2025, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.UIKit
{
    using Core;
    using FluentAPI;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public abstract class UIPanel : AbstractView, IPanel
    {
        public Transform Transform { get => transform; }
        
        [ShowInInspector] [EnumToggleButtons]
        public PanelState State
        {
            get;
            protected set;
        }

        public void Show()
        {
            this.EnableGameObject();
        }

        public void Hide()
        {
            this.DisableGameObject();
        }

        protected override void Awake()
        {
            base.Awake();
            State = PanelState.Loaded;
        }

        protected virtual void OnEnable()
        {
            State = PanelState.Shown;
        }

        protected virtual void OnDisable()
        {
            State = PanelState.Hidden;
        }

        protected virtual void OnDestroy()
        {
            State = PanelState.Unloaded;
        }
    }
}