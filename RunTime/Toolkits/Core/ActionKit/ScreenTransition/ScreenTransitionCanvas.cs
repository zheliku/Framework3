// ------------------------------------------------------------
// @file       ScreenTransitionCanvas.cs
// @brief
// @author     zheliku
// @Modified   2024-10-31 15:10:24
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.ActionKit
{
    using SingletonKit;
    using UnityEngine;
    using UnityEngine.UI;

    [MonoSingletonPath("Framework3/ActionKit/ScreenTransitionCanvas")]
    internal class ScreenTransitionCanvas : MonoBehaviour, ISingleton
    {
        public Image ColorImage;

        internal static ScreenTransitionCanvas Instance
        {
            get => PrefabSingletonProperty<ScreenTransitionCanvas>.InstanceWithLoader(Resources.Load<GameObject>);
        }

        public void OnSingletonInit()
        { }
    }
}