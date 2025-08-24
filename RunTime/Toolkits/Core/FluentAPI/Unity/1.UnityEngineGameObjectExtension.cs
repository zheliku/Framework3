// ------------------------------------------------------------
// @file       1.UnityEngineGameObjectExtension.cs
// @brief
// @author     zheliku
// @Modified   2024-10-18 12:10:07
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.FluentAPI
{
    using System;
    using EventKit;
    using UnityEngine;
    using Object = UnityEngine.Object;

    /// <summary>
    /// 针对 <see cref="UnityEngine.GameObject"/> 提供的链式扩展
    /// </summary>
    public static class UnityEngineGameObjectExtension
    {
        /// <summary>
        /// <see cref="GameObject.SetActive(bool)"/> 简单链式封装
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// new GameObject().Enable();
        /// ]]> 
        /// </code> </example>
        public static GameObject Enable(this GameObject selfObj)
        {
            selfObj.SetActive(true);
            return selfObj;
        }
        
        /// <summary>
        /// 检查 GameObject 是否在场景中处于激活状态。
        /// </summary>
        /// <param name="selfObj">要检查的 GameObject 实例。</param>
        /// <returns>如果 GameObject 在场景中处于激活状态，则返回 true；否则返回 false。</returns>
        public static bool IsEnabled(this GameObject selfObj)
        {
            return selfObj.activeInHierarchy;
        }
        
        /// <summary>
        /// 检查 GameObject 自身是否处于激活状态（不考虑父对象的激活状态）。
        /// </summary>
        /// <param name="selfObj">要检查的 GameObject 实例。</param>
        /// <returns>如果 GameObject 自身处于激活状态，则返回 true；否则返回 false。</returns>
        public static bool IsEnabledSelf(this GameObject selfObj)
        {
            return selfObj.activeSelf;
        }

        /// <summary>
        /// <c> <![CDATA[
        /// script.gameObject.SetActive(true);
        /// ]]> </c>
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// GetComponent<MyScript>().EnableGameObject();
        /// ]]>
        /// </code> </example>
        public static T EnableGameObject<T>(this T selfComponent) where T : Component
        {
            selfComponent.gameObject.SetActive(true);
            return selfComponent;
        }
        
        public static bool IsGameObjectEnabled<T>(this T selfComponent) where T : Component
        {
            return selfComponent.gameObject.activeInHierarchy;
        }
        
        public static bool IsGameObjectEnabledSelf<T>(this T selfComponent) where T : Component
        {
            return selfComponent.gameObject.activeSelf;
        }

        /// <summary>
        /// <see cref="GameObject.SetActive(bool)"/> 简单链式封装
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// new GameObject().Disable();
        /// ]]>
        /// </code> </example>
        public static GameObject Disable(this GameObject selfObj)
        {
            selfObj.SetActive(false);
            return selfObj;
        }
        
        /// <summary>
        /// 检查 GameObject 是否在场景中未激活状态。
        /// </summary>
        /// <param name="selfObj">要检查的 GameObject 实例。</param>
        /// <returns>如果 GameObject 在场景中未激活状态，则返回 true；否则返回 false。</returns>
        public static bool IsDisabled(this GameObject selfObj)
        {
            return !selfObj.activeInHierarchy;
        }
        
        /// <summary>
        /// 检查 GameObject 自身是否未激活状态（不考虑父对象的激活状态）。
        /// </summary>
        /// <param name="selfObj">要检查的 GameObject 实例。</param>
        /// <returns>如果 GameObject 自身未激活状态，则返回 true；否则返回 false。</returns>
        public static bool IsDisabledSelf(this GameObject selfObj)
        {
            return !selfObj.activeSelf;
        }

        /// <summary>
        /// <c> <![CDATA[
        /// script.gameObject.SetActive(false);
        /// ]]> </c>
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// GetComponent<MyScript>().Disable();
        /// ]]>
        /// </code> </example>
        public static T DisableGameObject<T>(this T selfComponent) where T : Component
        {
            selfComponent.gameObject.SetActive(false);
            return selfComponent;
        }
        
        public static bool IsGameObjectDisabled<T>(this T selfComponent) where T : Component
        {
            return !selfComponent.gameObject.activeInHierarchy;
        }
        
        public static bool IsGameObjectDisabledSelf<T>(this T selfComponent) where T : Component
        {
            return !selfComponent.gameObject.activeSelf;
        }

        /// <summary>
        /// <c> <![CDATA[
        /// Destroy(myScript.gameObject);
        /// ]]> </c>
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// myScript.DestroyGameObject();
        /// ]]>
        /// </code> </example>
        public static void DestroyGameObject<T>(this T selfBehaviour) where T : Component
        {
            Object.Destroy(selfBehaviour.gameObject);
        }

        /// <summary>
        /// <c> <![CDATA[
        /// DestroyGracefully(myScript.gameObject);
        /// ]]> </c>
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// myScript.DestroyGameObjectGracefully();
        /// ]]>
        /// </code> </example>
        public static void DestroyGameObjectGracefully<T>(this T selfBehaviour) where T : Component
        {
            if (selfBehaviour && selfBehaviour.gameObject)
            {
                selfBehaviour.gameObject.DestroyGracefully();
            }
        }

        /// <summary>
        /// <c> <![CDATA[
        /// Object.Destroy(myScript.gameObject, delaySeconds);
        /// ]]> </c>
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// myScript.DestroyGameObject(5);
        /// ]]>
        /// </code> </example>
        public static T DestroyGameObject<T>(this T selfBehaviour, float delayTime) where T : Component
        {
            selfBehaviour.gameObject.Destroy(delayTime);
            return selfBehaviour;
        }

        /// <summary>
        /// <c> <![CDATA[
        /// if (myScript && myScript.gameObject) Object.Destroy(myScript.gameObject, delaySeconds); 
        /// ]]> </c>
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// myScript.DestroyGameObjectGracefully(5);
        /// ]]>
        /// </code> </example>
        public static T DestroyGameObjectGracefully<T>(this T selfBehaviour, float delayTime) where T : Component
        {
            if (selfBehaviour && selfBehaviour.gameObject)
            {
                selfBehaviour.gameObject.DestroyGracefully(delayTime);
            }

            return selfBehaviour;
        }

        /// <summary>
        /// <c> <![CDATA[
        /// gameObject.layer = layer;
        /// ]]> </c>
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// new GameObject().SetLayer(0);
        /// ]]>
        /// </code> </example>
        public static GameObject SetLayer(this GameObject selfObj, int layer)
        {
            selfObj.layer = layer;
            return selfObj;
        }

        /// <summary>
        /// <c> <![CDATA[
        /// component.gameObject.layer = layer;
        /// ]]> </c>
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// rigidbody2D.SetLayer(0);
        /// ]]>
        /// </code> </example>
        public static T SetLayer<T>(this T selfComponent, int layer) where T : Component
        {
            selfComponent.gameObject.layer = layer;
            return selfComponent;
        }

        /// <summary>
        /// <c> <![CDATA[
        /// gameObj.layer = LayerMask.NameToLayer(layerName);
        /// ]]> </c>
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// new GameObject().SetLayer("Default");
        /// ]]>
        /// </code> </example>
        public static GameObject SetLayer(this GameObject selfObj, string layerName)
        {
            selfObj.layer = LayerMask.NameToLayer(layerName);
            return selfObj;
        }

        /// <summary>
        /// <c> <![CDATA[
        /// component.gameObject.layer = LayerMask.NameToLayer(layerName);
        /// ]]> </c>
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// spriteRenderer.SetLayer("Default");
        /// ]]>
        /// </code> </example>
        public static T SetLayer<T>(this T selfComponent, string layerName) where T : Component
        {
            selfComponent.gameObject.layer = LayerMask.NameToLayer(layerName);
            return selfComponent;
        }

        /// <summary>
        /// layerMask 中的层级是否包含 selfObj 所在的层级
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// gameObj.IsInLayerMask(layerMask);
        /// ]]>
        /// </code> </example>
        public static bool IsInLayerMask(this GameObject selfObj, LayerMask layerMask)
        {
            // 根据 Layer 数值进行移位获得用于运算的 Mask 值
            var objLayerMask = 1 << selfObj.layer;
            return (layerMask.value & objLayerMask) == objLayerMask;
        }

        /// <summary>
        /// layerMask 中的层级是否包含 selfComponent.gameObject 所在的层级
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// spriteRenderer.IsInLayerMask(layerMask);
        /// ]]>
        /// </code> </example>
        public static bool IsInLayerMask<T>(this T selfComponent, LayerMask layerMask) where T : Component
        {
            // 根据 Layer 数值进行移位获得用于运算的 Mask 值
            var objLayerMask = 1 << selfComponent.gameObject.layer;
            return (layerMask.value & objLayerMask) == objLayerMask;
        }

        /// <summary>
        /// 获取组件，没有则添加再返回
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// gameObj.GetOrAddComponent<SpriteRenderer>();
        /// ]]>
        /// </code> </example>
        public static T GetOrAddComponent<T>(this GameObject self) where T : Component
        {
            var comp = self.gameObject.GetComponent<T>();
            return comp ? comp : self.gameObject.AddComponent<T>();
        }
        
        /// <summary>
        /// 获取组件，没有则添加再返回
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// component.GetOrAddComponent<SpriteRenderer>();
        /// ]]>
        /// </code> </example>
        public static T GetOrAddComponent<T>(this Component component) where T : Component
        {
            return component.gameObject.GetOrAddComponent<T>();
        }
        
        /// <summary>
        /// 获取组件，没有则添加再返回
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// gameObj.GetOrAddComponent(typeof(SpriteRenderer));
        /// ]]>
        /// </code> </example>
        public static Component GetOrAddComponent(this GameObject self, Type type)
        {
            var component = self.gameObject.GetComponent(type);
            return component ? component : self.gameObject.AddComponent(type);
        }
        
        /// <summary>
        /// 获取组件，没有则添加再返回
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// component.GetOrAddComponent(typeof(SpriteRenderer));
        /// ]]>
        /// </code> </example>
        public static Component GetOrAddComponent(this Component component, Type type)
        {
            var comp = component.gameObject.GetComponent(type);
            return comp ? comp : component.gameObject.AddComponent(type);
        }

        /// <summary>
        /// 当目标 GameObject 被禁用时销毁当前 GameObject。
        /// </summary>
        /// <param name="self">当前 GameObject。</param>
        /// <param name="target">目标 GameObject。</param>
        /// <param name="priority">事件触发的优先级。</param>
        /// <returns>返回当前 GameObject。</returns>
        public static GameObject DestroyWhenGameObjectDisabled(this GameObject self, GameObject target, float priority = 0)
        {
            target.GetOrAddComponent<OnDisableEventTrigger>().OnDisableEvent.Register(() =>
            {
                Object.Destroy(self);
            }, priority);
            return self;
        }
        
        /// <summary>
        /// 当目标 GameObject 被销毁时销毁当前 GameObject。
        /// </summary>
        /// <param name="self">当前 GameObject。</param>
        /// <param name="target">目标 GameObject。</param>
        /// <param name="priority">事件触发的优先级。</param>
        /// <returns>返回当前 GameObject。</returns>
        public static GameObject DestroyWhenGameObjectDestroyed(this GameObject self, GameObject target, float priority = 0)
        {
            target.GetOrAddComponent<OnDestroyEventTrigger>().OnDestroyEvent.Register(() =>
            {
                Object.Destroy(self);
            }, priority);
            return self;
        }
        
        /// <summary>
        /// 当目标 GameObject 被禁用时优雅地销毁当前 GameObject（检查当前 GameObject 是否存在）。 
        /// </summary>
        /// <param name="self">当前 GameObject。</param>
        /// <param name="target">目标 GameObject。</param>
        /// <param name="priority">事件触发的优先级。</param>
        /// <returns>返回当前 GameObject。</returns>
        public static GameObject DestroyGracefullyWhenGameObjectDisabled(this GameObject self, GameObject target, float priority = 0)
        {
            target.GetOrAddComponent<OnDisableEventTrigger>().OnDisableEvent.Register(() =>
            {
                if (self)
                {
                    Object.Destroy(self);
                }
            }, priority);
            return self;
        }
        
        /// <summary>
        /// 当目标 GameObject 被销毁时优雅地销毁当前 GameObject（检查当前 GameObject 是否存在）。
        /// </summary>
        /// <param name="self">当前 GameObject。</param>
        /// <param name="target">目标 GameObject。</param>
        /// <param name="priority">事件触发的优先级。</param>
        /// <returns>返回当前 GameObject。</returns>
        public static GameObject DestroyGracefullyWhenGameObjectDestroyed(this GameObject self, GameObject target, float priority = 0)
        {
            target.GetOrAddComponent<OnDestroyEventTrigger>().OnDestroyEvent.Register(() =>
            {
                if (self)
                {
                    Object.Destroy(self);
                }
            }, priority);
            return self;
        }
        
        /// <summary>
        /// 当目标 GameObject 被禁用时禁用当前 GameObject。
        /// </summary>
        /// <param name="self">当前 GameObject。</param>
        /// <param name="target">目标 GameObject。</param>
        /// <param name="priority">事件触发的优先级。</param>
        /// <returns>返回当前 GameObject。</returns>
        public static GameObject DisableWhenGameObjectDisabled(this GameObject self, GameObject target, float priority = 0)
        {
            target.GetOrAddComponent<OnDisableEventTrigger>().OnDisableEvent.Register(() =>
            {
                self.SetActive(false);
            }, priority);
            return self;
        }
        
        /// <summary>
        /// 当目标 GameObject 被销毁时禁用当前 GameObject。
        /// </summary>
        /// <param name="self">当前 GameObject。</param>
        /// <param name="target">目标 GameObject。</param>
        /// <param name="priority">事件触发的优先级。</param>
        /// <returns>返回当前 GameObject。</returns>
        public static GameObject DisableWhenGameObjectDestroyed(this GameObject self, GameObject target, float priority = 0)
        {
            target.GetOrAddComponent<OnDestroyEventTrigger>().OnDestroyEvent.Register(() =>
            {
                self.SetActive(false);
            }, priority);
            return self;
        }
    }
}