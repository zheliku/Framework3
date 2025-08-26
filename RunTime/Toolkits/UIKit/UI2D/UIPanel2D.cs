// ------------------------------------------------------------
// @file       UIPanel2D.cs
// @brief
// @author     zheliku
// @Modified   2024-12-12 19:12:14
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

using UnityEngine;

namespace Framework3.Toolkits.UIKit
{
    using System;
    using Core;
    using Sirenix.OdinInspector;

    public abstract class UIPanel2D : AbstractView, IPanel2D
    {
        public Transform Transform { get => transform; }

        [ShowInInspector] [EnumToggleButtons]
        public PanelState State
        {
            get;
            protected set;
        }
        
        public UILevel Level
        {
            get => Enum.Parse<UILevel>(transform.parent?.name); // 父物体名称是该 Panel2D 的 Level
            set
            {
                if (Enum.TryParse<UILevel>(transform.parent?.name, out var level))
                {
                    if (level == value) // 相同的 level 不做处理
                    {
                        return;
                    }
                }
                UI2DRoot.Instance.SetLevelOfPanel(value, this);
            }
        }

        void IPanel2D.Load() // 对自己隐藏
        {
            State = PanelState.Loaded;
            this.BindHierarchyComponent();
            OnLoad();
            gameObject.SetActive(false);
        }

        public void Show()
        {
            State = PanelState.Shown;
            gameObject.SetActive(true);
            OnShow();
        }

        public void Hide()
        {
            State = PanelState.Hidden;
            gameObject.SetActive(false);
            OnHide();
        }

        void IPanel2D.Unload() // 对自己隐藏
        {
            State = PanelState.Unloaded;
            OnUnload();
        }

        protected virtual void OnLoad() { }

        protected virtual void OnShow() { }

        protected virtual void OnHide() { }

        protected virtual void OnUnload() { }
    }
}