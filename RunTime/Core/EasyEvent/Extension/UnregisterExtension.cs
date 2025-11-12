// ------------------------------------------------------------
// @file       UnregisterExtension.cs
// @brief
// @author     zheliku
// @Modified   2024-10-04 16:10:22
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Core
{
    using UnityEngine;

    /// <summary>
    ///     IUnregister 扩展
    /// </summary>
    public static class UnregisterExtension
    {
        /// <summary>
        ///     获取 GameObject 上的组件，不存在则添加
        /// </summary>
        /// <param name="gameObject">GameObject 实例</param>
        /// <typeparam name="TComponent">组件类型</typeparam>
        /// <returns>获取的组件实例</returns>
        private static TComponent GetOrAddComponent<TComponent>(GameObject gameObject) where TComponent : Component
        {
            var trigger = gameObject.GetComponent<TComponent>();

            if (!trigger)
            {
                trigger = gameObject.AddComponent<TComponent>();
            }

            return trigger;
        }

        /// <summary>
        ///     当 GameObject 销毁时注销
        /// </summary>
        /// <param name="self">注销器</param>
        /// <param name="gameObject">绑定的 GameObject</param>
        /// <returns>注销器</returns>
        public static IUnregister UnregisterWhenGameObjectDestroyed(this IUnregister self, GameObject gameObject)
        {
            return GetOrAddComponent<UnregisterOnDestroyTrigger>(gameObject).AddUnregister(self); // 添加到 UnregisterOnDestroyTrigger 中
        }

        /// <summary>
        ///     当 GameObject 禁用时注销
        /// </summary>
        /// <param name="self">注销器</param>
        /// <param name="gameObject">绑定的 GameObject</param>
        /// <returns>注销器</returns>
        public static IUnregister UnregisterWhenGameObjectDisabled(this IUnregister self, GameObject gameObject)
        {
            return GetOrAddComponent<UnregisterOnDisableTrigger>(gameObject).AddUnregister(self); // 添加到 UnregisterOnDisableTrigger 中
        }

        /// <summary>
        ///     当组件挂载的 GameObject 销毁时注销
        /// </summary>
        /// <param name="self">注销器</param>
        /// <param name="component">绑定的组件</param>
        /// <returns>注销器</returns>
        public static IUnregister UnregisterWhenGameObjectDestroyed<TComponent>(this IUnregister self, TComponent component) where TComponent : Component
        {
            return GetOrAddComponent<UnregisterOnDestroyTrigger>(component.gameObject).AddUnregister(self); // 添加到 UnregisterOnDestroyTrigger 中
        }

        /// <summary>
        ///     当组件挂载的 GameObject 禁用时注销
        /// </summary>
        /// <param name="self">注销器</param>
        /// <param name="component">绑定的组件</param>
        /// <returns>注销器</returns>
        public static IUnregister UnregisterWhenGameObjectDisabled<TComponent>(this IUnregister self, TComponent component) where TComponent : Component
        {
            return GetOrAddComponent<UnregisterOnDisableTrigger>(component.gameObject).AddUnregister(self); // 添加到 UnregisterOnDisableTrigger 中
        }


        /// <summary>
        ///     当前场景卸载时注销
        /// </summary>
        /// <param name="self">注销器</param>
        /// <returns>注销器</returns>
        public static IUnregister UnregisterWhenCurrentSceneUnloaded(this IUnregister self)
        {
            return UnregisterCurrentSceneUnloadedTrigger.Default.AddUnregister(self); // 添加到 UnregisterCurrentSceneUnloadedTrigger 中
        }
    }
}