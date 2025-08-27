// ------------------------------------------------------------
// @file       2.UnityEngineTransformExtension.cs
// @brief
// @author     zheliku
// @Modified   2024-10-18 13:10:33
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.FluentAPI
{
    using System;
    using UnityEngine;
    using Object = UnityEngine.Object;

    /// <summary>
    /// 针对 <see cref="UnityEngine.Transform"/> 提供的链式扩展
    /// </summary>
    public static class UnityEngineTransformExtension
    {
        /// <summary>
        /// <c> <![CDATA[
        /// component.transform.SetParent(parent);
        /// ]]> </c>
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// myScript.Parent(rootGameObj);
        /// ]]>
        /// </code> </example>
        public static T SetParent<T>(this T self, Component parent) where T : Component
        {
            self.transform.SetParent(parent?.transform);
            return self;
        }

        /// <summary>
        /// <c> <![CDATA[
        /// gameObject.transform.SetParent(parent);
        /// ]]> </c>
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// gameObj.Parent(rootGameObj);
        /// ]]>
        /// </code> </example>
        public static GameObject SetParent(this GameObject self, Component parent)
        {
            self.transform.SetParent(parent?.transform);
            return self;
        }
        
        /// <summary>
        /// 设置当前组件的父对象为指定的 GameObject。
        /// </summary>
        /// <typeparam name="T">当前组件的类型。</typeparam>
        /// <param name="self">当前组件实例。</param>
        /// <param name="parent">目标父对象的 GameObject。</param>
        /// <returns>返回当前组件实例。</returns>
        public static T SetParent<T>(this T self, GameObject parent) where T : Component
        {
            self.transform.SetParent(parent?.transform);
            return self;
        }
        
        /// <summary>
        /// 设置当前 GameObject 的父对象为指定的 GameObject。
        /// </summary>
        /// <param name="self">当前 GameObject 实例。</param>
        /// <param name="parent">目标父对象的 GameObject。</param>
        /// <returns>返回当前 GameObject 实例。</returns>
        public static GameObject SetParent(this GameObject self, GameObject parent)
        {
            self.transform.SetParent(parent?.transform);
            return self;
        }
        
        /// <summary>
        /// 获取当前 GameObject 的父对象的 Transform。
        /// </summary>
        /// <param name="self">当前 GameObject 实例。</param>
        /// <returns>返回父对象的 Transform，如果没有父对象则返回 null。</returns>
        public static Transform GetParent(this GameObject self)
        {
            return self.transform.parent;
        }
        
        /// <summary>
        /// 获取当前组件的父对象的 Transform。
        /// </summary>
        /// <param name="self">当前组件实例。</param>
        /// <returns>返回父对象的 Transform，如果没有父对象则返回 null。</returns>
        public static Transform GetParent(this Component self)
        {
            return self.transform.parent;
        }

        /// <summary>
        /// <c> <![CDATA[
        /// component.transform.SetParent(null);
        /// ]]> </c>
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// component.SetRoot();
        /// ]]>
        /// </code> </example>
        public static T SetRoot<T>(this T self) where T : Component
        {
            self.transform.SetParent(null);
            return self;
        }

        /// <summary>
        /// <c> <![CDATA[
        /// gameObject.transform.SetParent(null);
        /// ]]> </c>
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// gameObject.SetRoot();
        /// ]]>
        /// </code> </example>
        public static GameObject SetRoot(this GameObject self)
        {
            self.transform.SetParent(null);
            return self;
        }

        /// <summary>
        /// 设置本地位置为 0、本地角度为 0、本地缩放为 1
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// myScript.SetLocalIdentity();
        /// ]]>
        /// </code> </example>
        public static T SetLocalIdentity<T>(this T self) where T : Component
        {
            self.transform.localPosition = Vector3.zero;
            self.transform.localRotation = Quaternion.identity;
            self.transform.localScale    = Vector3.one;
            return self;
        }

        /// <summary>
        /// 设置本地位置为 0、本地角度为 0、本地缩放为 1
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// gameObject.SetLocalIdentity();
        /// ]]>
        /// </code> </example>
        public static GameObject SetLocalIdentity(this GameObject self)
        {
            self.transform.localPosition = Vector3.zero;
            self.transform.localRotation = Quaternion.identity;
            self.transform.localScale    = Vector3.one;
            return self;
        }

        /// <summary>
        /// 设置世界位置:0 角度:0 缩放:1
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// component.SetIdentity();
        /// ]]>
        /// </code> </example>
        public static T SetIdentity<T>(this T selfComponent) where T : Component
        {
            selfComponent.transform.position   = Vector3.zero;
            selfComponent.transform.rotation   = Quaternion.identity;
            selfComponent.transform.localScale = Vector3.one;
            return selfComponent;
        }

        /// <summary>
        /// 设置世界位置:0 角度:0 缩放:1
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// gameObject.SetIdentity();
        /// ]]>
        /// </code> </example>
        public static GameObject SetIdentity(this GameObject self)
        {
            self.transform.position   = Vector3.zero;
            self.transform.rotation   = Quaternion.identity;
            self.transform.localScale = Vector3.one;
            return self;
        }
        
        /// <summary>
        /// 根据条件 Destroy 所有子 GameObject
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// rootTransform.DestroyChildren();
        ///  
        /// rootTransform.DestroyChildren(child => child != other);
        /// ]]>
        /// </code> </example>
        public static T DestroyChildren<T>(this T selfComponent, Func<Transform, bool> condition = null) where T : Component
        {
            var childCount = selfComponent.transform.childCount;

            for (var i = childCount - 1; i >= 0; i--)
            {
                var child = selfComponent.transform.GetChild(i);
                if (condition == null || condition(child))
                {
                    Object.Destroy(child.gameObject);
                }
            }

            return selfComponent;
        }
        
        /// <summary>
        /// 根据条件 Destroy 所有子 GameObject
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// rootGameObject.DestroyChildrenWithCondition();
        ///  
        /// rootGameObject.DestroyChildrenWithCondition(child => child != other);
        /// ]]>
        /// </code> </example>
        public static GameObject DestroyChildren(this GameObject selfGameObject, Func<Transform, bool> condition = null)
        {
            var childCount = selfGameObject.transform.childCount;

            for (var i = childCount - 1; i >= 0; i--)
            {
                var child = selfGameObject.transform.GetChild(i);
                if (condition == null || condition(child))
                {
                    Object.Destroy(child.gameObject);
                }
            }

            return selfGameObject;
        }
        
        /// <summary>
        /// <c> <![CDATA[
        /// component.transform.SetAsLastSibling();
        /// ]]> </c>
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// myScript.SetAsLastSibling();
        /// ]]>
        /// </code> </example>
        public static T SetAsLastSibling<T>(this T selfComponent) where T : Component
        {
            selfComponent.transform.SetAsLastSibling();
            return selfComponent;
        }
        
        /// <summary>
        /// <c> <![CDATA[
        /// gameObject.transform.SetAsLastSibling();
        /// ]]> </c>
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// gameObject.SetAsLastSibling();
        /// ]]>
        /// </code> </example>
        public static GameObject SetAsLastSibling(this GameObject self)
        {
            self.transform.SetAsLastSibling();
            return self;
        }
        
        /// <summary>
        /// <c> <![CDATA[
        /// component.transform.SetAsFirstSibling();
        /// ]]> </c>
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// myScript.SetAsFirstSibling();
        /// ]]>
        /// </code> </example>
        public static T SetAsFirstSibling<T>(this T selfComponent) where T : Component
        {
            selfComponent.transform.SetAsFirstSibling();
            return selfComponent;
        }
        
        /// <summary>
        /// <c> <![CDATA[
        /// gameObject.transform.SetAsFirstSibling();
        /// ]]> </c>
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// gameObject.SetAsFirstSibling();
        /// ]]>
        /// </code> </example>
        public static GameObject SetAsFirstSibling(this GameObject self)
        {
            self.transform.SetAsFirstSibling();
            return self;
        }
        
        /// <summary>
        /// <c> <![CDATA[
        /// component.transform.SetSiblingIndex(index);
        /// ]]> </c>
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// myScript.SetSiblingIndex(10);
        /// ]]>
        /// </code> </example>
        public static T SetSiblingIndex<T>(this T selfComponent, int index) where T : Component
        {
            selfComponent.transform.SetSiblingIndex(index);
            return selfComponent;
        }
        
        /// <summary>
        /// <c> <![CDATA[
        /// gameObject.transform.SetSiblingIndex(index);
        /// ]]> </c>
        /// </summary>
        /// <example> <code>
        /// <![CDATA[
        /// gameObject.SetSiblingIndex(10);
        /// ]]>
        /// </code> </example>
        public static GameObject SetSiblingIndex(this GameObject selfComponent, int index)
        {
            selfComponent.transform.SetSiblingIndex(index);
            return selfComponent;
        }

        /// <summary>
            /// 设置当前 GameObject 的 Transform 的 right 向量。
            /// </summary>
            /// <param name="self">当前 GameObject 实例。</param>
            /// <param name="right">要设置的 right 向量。</param>
            /// <returns>返回当前 GameObject 实例。</returns>
            public static GameObject SetTransformRight(this GameObject self, Vector3 right)
            {
                self.transform.right = right;
                return self;
            }
            
            /// <summary>
            /// 设置当前组件的 Transform 的 right 向量。
            /// </summary>
            /// <typeparam name="T">当前组件的类型。</typeparam>
            /// <param name="self">当前组件实例。</param>
            /// <param name="right">要设置的 right 向量。</param>
            /// <returns>返回当前组件实例。</returns>
            public static T SetTransformRight<T>(this T self, Vector3 right) where T : Component
            {
                self.transform.right = right;
                return self;
            }
            
            /// <summary>
            /// 设置当前 GameObject 的 Transform 的 up 向量。
            /// </summary>
            /// <param name="self">当前 GameObject 实例。</param>
            /// <param name="up">要设置的 up 向量。</param>
            /// <returns>返回当前 GameObject 实例。</returns>
            public static GameObject SetTransformUp(this GameObject self, Vector3 up)
            {
                self.transform.up = up;
                return self;
            }
            
            /// <summary>
            /// 设置当前组件的 Transform 的 up 向量。
            /// </summary>
            /// <typeparam name="T">当前组件的类型。</typeparam>
            /// <param name="self">当前组件实例。</param>
            /// <param name="up">要设置的 up 向量。</param>
            /// <returns>返回当前组件实例。</returns>
            public static T SetTransformUp<T>(this T self, Vector3 up) where T : Component
            {
                self.transform.up = up;
                return self;
            }
            
            /// <summary>
            /// 设置当前 GameObject 的 Transform 的 forward 向量。
            /// </summary>
            /// <param name="self">当前 GameObject 实例。</param>
            /// <param name="forward">要设置的 forward 向量。</param>
            /// <returns>返回当前 GameObject 实例。</returns>
            public static GameObject SetTransformForward(this GameObject self, Vector3 forward)
            {
                self.transform.forward = forward;
                return self;
            }
            
            /// <summary>
            /// 设置当前组件的 Transform 的 forward 向量。
            /// </summary>
            /// <typeparam name="T">当前组件的类型。</typeparam>
            /// <param name="self">当前组件实例。</param>
            /// <param name="forward">要设置的 forward 向量。</param>
            /// <returns>返回当前组件实例。</returns>
            public static T SetTransformForward<T>(this T self, Vector3 forward) where T : Component
            {
                self.transform.forward = forward;
                return self;
            }
    }
}