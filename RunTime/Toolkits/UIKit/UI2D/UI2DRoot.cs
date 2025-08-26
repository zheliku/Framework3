// ------------------------------------------------------------
// @file       UI2DRoot.cs
// @brief
// @author     zheliku
// @Modified   2024-12-12 14:12:32
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.UIKit
{
    using ResKit;
    using Core;
    using SingletonKit;
    using UnityEngine;
    using UnityEngine.UI;

    [MonoSingletonPath(nameof(UI2DRoot))]
    public class UI2DRoot : MonoBehaviour, ISingleton
    {
        private static GameObject s_asset;

        private static UI2DRoot s_instance;

        public static UI2DRoot Instance
        {
            get
            {
                if (!s_instance)
                {
                    s_instance = FindAnyObjectByType<UI2DRoot>(); // 先找场景中的 UI2DRoot
                }

                if (!s_instance)
                {
                    if (s_asset == null) // 找不到 UI2DRoot，则加载 _Asset
                    {
                        s_asset = ResKit.LoadFromResources<GameObject>("UI2DRoot");
                    }

                    s_instance      = Instantiate(s_asset).GetComponent<UI2DRoot>(); // 实例化 UI2DRoot
                    s_instance.name = "UI2DRoot";
                    DontDestroyOnLoad(s_instance);
                }

                return s_instance;
            }
        }

        public Camera           UICamera;
        public Canvas           Canvas;
        public CanvasScaler     CanvasScaler;
        public GraphicRaycaster GraphicRaycaster;

        // Level 层级对应的父物体
        public RectTransform Bg;
        public RectTransform Bottom;
        public RectTransform Common;
        public RectTransform Top;

        /// <summary>
        /// 设置参考分辨率
        /// </summary>
        public Vector2 ReferenceResolution
        {
            get => CanvasScaler.referenceResolution;
            set => CanvasScaler.referenceResolution = value;
        }

        /// <summary>
        /// 设置宽高适配比例
        /// </summary>
        public float MatchWidthOrHeight
        {
            get => CanvasScaler.matchWidthOrHeight;
            set => CanvasScaler.matchWidthOrHeight = value;
        }

        /// <summary>
        /// 设置 ScreenSpaceOverlay 渲染模式
        /// </summary>
        public void ScreenSpaceOverlayRenderMode()
        {
            Canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            UICamera.gameObject.SetActive(false);
        }

        /// <summary>
        /// 设置 ScreenSpaceCamera 渲染模式
        /// </summary>
        public void ScreenSpaceCameraRenderMode()
        {
            Canvas.renderMode = RenderMode.ScreenSpaceCamera;
            UICamera.gameObject.SetActive(true);
            Canvas.worldCamera = UICamera;
        }

        /// <summary>
        /// 设置 Panel2D 层级
        /// </summary>
        /// <param name="level">层级</param>
        /// <param name="panel2D">哪个 Panel2D</param>
        public void SetLevelOfPanel(UILevel level, IPanel2D panel2D)
        {
            switch (level)
            {
                case UILevel.Bg:
                    panel2D.Transform.SetParent(Bg, false);
                    break;
                case UILevel.Bottom:
                    panel2D.Transform.SetParent(Bottom, false);
                    break;
                case UILevel.Common:
                    panel2D.Transform.SetParent(Common, false);
                    break;
                case UILevel.Top:
                    panel2D.Transform.SetParent(Top, false);
                    break;
            }
        }

        public void OnSingletonInit() { }

        private void OnDestroy()
        {
            // _Asset?.Release();
        }
    }
}