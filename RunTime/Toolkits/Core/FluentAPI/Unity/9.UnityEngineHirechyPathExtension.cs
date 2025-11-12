// ------------------------------------------------------------
// @file       9.UnityEngineHirechyPathExtension.cs
// @brief
// @author     zheliku
// @Modified   2024-12-13 13:12:23
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.FluentAPI
{
    using System.Linq;
    using Core;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public static class UnityEngineHierarchyPathExtension
    {
        /// <summary>
        ///     根据层级路径查找指定的 GameObject。
        /// </summary>
        /// <param name="hierarchyPath">层级路径，使用 '/' 分隔每一级名称。</param>
        /// <param name="includeInactive">是否包含未激活的 GameObject。</param>
        /// <param name="throwExceptionIfNotFound">如果未找到，是否抛出异常。</param>
        /// <returns>找到的 GameObject，如果未找到且未抛异常，则返回 null。</returns>
        /// <exception cref="FrameworkException">当未找到且 throwExceptionIfNotFound 为 true 时抛出。</exception>
        public static GameObject GetGameObjectInHierarchy(
            this string hierarchyPath,
            bool        includeInactive          = true,
            bool        throwExceptionIfNotFound = true)
        {
            var objNames = hierarchyPath.Split('/');
            var current = SceneManager.GetActiveScene()
               .GetRootGameObjects()
               .FirstOrDefault(o => o.name == objNames[0]
                                 && (includeInactive || o.activeInHierarchy));

            if (current)
            {
                // 逐级查找子节点
                for (var i = 1; i < objNames.Length; i++)
                {
                    var found       = false;
                    var parentTrans = current.transform;
                    for (var j = 0; j < parentTrans.childCount; j++)
                    {
                        var child = parentTrans.GetChild(j).gameObject;
                        if (child.name == objNames[i] && (includeInactive || child.activeInHierarchy))
                        {
                            current = child;
                            found   = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        current = null;
                        break;
                    }
                }
            }

            else if (throwExceptionIfNotFound)
            {
                throw new FrameworkException($"Can not find GameObject in hierarchy path: {hierarchyPath}");
            }

            return current;
        }

        /// <summary>
        ///     根据层级路径创建 GameObject，并设置父级关系。
        /// </summary>
        /// <param name="hierarchyPath">层级路径，使用 '/' 分隔每一级名称。</param>
        /// <param name="parent">父级 Transform，可选。</param>
        /// <returns>创建的最底层 GameObject。</returns>
        public static GameObject AddGameObjectInHierarchy(this string hierarchyPath, Transform parent = null)
        {
            var        objNames   = hierarchyPath.Split('/');
            GameObject obj        = null;
            var        parentCopy = parent;
            foreach (var name in objNames)
            {
                obj = new GameObject(name);
                obj.transform.SetParent(parentCopy);
                parentCopy = obj.transform;
            }
            return obj;
        }

        /// <summary>
        ///     根据层级路径获取或创建指定的 GameObject。
        /// </summary>
        /// <param name="hierarchyPath">层级路径，使用 '/' 分隔每一级名称。</param>
        /// <param name="includeInactive">是否包含未激活的 GameObject。</param>
        /// <returns>找到或创建的 GameObject。</returns>
        public static GameObject GetOrAddGameObjectInHierarchy(
            this string hierarchyPath,
            bool        includeInactive = true)
        {
            var objNames = hierarchyPath.Split('/');
            var rootObjs = SceneManager.GetActiveScene().GetRootGameObjects();
            var parent = includeInactive
                             ? rootObjs.FirstOrDefault(o => o.name == objNames[0])
                             : rootObjs.FirstOrDefault(o => o.name == objNames[0] && o.activeInHierarchy);
            if (parent == null)
            {
                return hierarchyPath.AddGameObjectInHierarchy();
            }
            return objNames[1..].Join("/").GetOrAddGameObjectInHierarchy(parent.transform, includeInactive);
        }

        /// <summary>
        ///     根据层级路径获取指定类型的组件。
        /// </summary>
        /// <typeparam name="T">组件类型。</typeparam>
        /// <param name="hierarchyPath">层级路径，使用 '/' 分隔每一级名称。</param>
        /// <param name="includeInactive">是否包含未激活的 GameObject。</param>
        /// <param name="throwExceptionIfNotFound">如果未找到，是否抛出异常。</param>
        /// <returns>找到的组件实例。</returns>
        /// <exception cref="FrameworkException">当未找到且 throwExceptionIfNotFound 为 true 时抛出。</exception>
        public static T GetComponentInHierarchy<T>(
            this string hierarchyPath,
            bool        includeInactive          = true,
            bool        throwExceptionIfNotFound = true) where T : Component
        {
            var obj       = hierarchyPath.GetGameObjectInHierarchy(includeInactive, false);
            var component = obj?.GetComponent<T>();

            if (throwExceptionIfNotFound && component == null)
            {
                throw new FrameworkException($"Can not find component {typeof(T)} in hierarchy path: {hierarchyPath}");
            }

            return component;
        }

        /// <summary>
        ///     根据层级路径从指定父级 Transform 获取 GameObject。
        /// </summary>
        /// <param name="hierarchyPath">层级路径，使用 '/' 分隔每一级名称。</param>
        /// <param name="parent">父级 Transform。</param>
        /// <param name="includeInactive">是否包含未激活的 GameObject。</param>
        /// <param name="throwExceptionIfNotFound">如果未找到，是否抛出异常。</param>
        /// <returns>找到的 GameObject。</returns>
        /// <exception cref="FrameworkException">当未找到且 throwExceptionIfNotFound 为 true 时抛出。</exception>
        public static GameObject GetGameObjectInHierarchy(
            this string hierarchyPath,
            Transform   parent,
            bool        includeInactive          = true,
            bool        throwExceptionIfNotFound = true)
        {
            var objNames = hierarchyPath.Split('/');
            var obj      = parent.gameObject;

            // 层次遍历，广度优先
            foreach (var name in objNames)
            {
                // 记录父物体
                var parentTrans = obj.transform;

                for (var i = 0; i < parentTrans.childCount; i++)
                {
                    var child = obj.transform.GetChild(i);
                    if ((includeInactive || child.gameObject.activeInHierarchy) && child.name == name)
                    {
                        // 找到，指向子物体
                        obj = child.gameObject;
                        break;
                    }
                }

                // 如果找完所在层所有子物体后，obj 没有变化，则说明没找到
                if (obj == parentTrans.gameObject)
                {
                    obj = null;
                    break;
                }
            }

            if (throwExceptionIfNotFound && obj == null)
            {
                throw new FrameworkException($"Can not find GameObject from {parent} in hierarchy path: {hierarchyPath}");
            }

            return obj;
        }

        /// <summary>
        ///     根据层级路径从指定父级 Component 获取 GameObject。
        /// </summary>
        /// <param name="hierarchyPath">层级路径，使用 '/' 分隔每一级名称。</param>
        /// <param name="parent">父级 Component。</param>
        /// <param name="includeInactive">是否包含未激活的 GameObject。</param>
        /// <param name="throwExceptionIfNotFound">如果未找到，是否抛出异常。</param>
        /// <returns>找到的 GameObject。</returns>
        /// <exception cref="FrameworkException">当未找到且 throwExceptionIfNotFound 为 true 时抛出。</exception>
        public static GameObject GetGameObjectInHierarchy(
            this string hierarchyPath,
            Component   parent,
            bool        includeInactive          = true,
            bool        throwExceptionIfNotFound = true)
        {
            return hierarchyPath.GetGameObjectInHierarchy(parent.transform, includeInactive, throwExceptionIfNotFound);
        }

        /// <summary>
        ///     根据层级路径从指定父级 GameObject 获取 GameObject。
        /// </summary>
        /// <param name="hierarchyPath">层级路径，使用 '/' 分隔每一级名称。</param>
        /// <param name="parent">父级 GameObject。</param>
        /// <param name="includeInactive">是否包含未激活的 GameObject。</param>
        /// <param name="throwExceptionIfNotFound">如果未找到，是否抛出异常。</param>
        /// <returns>找到的 GameObject。</returns>
        /// <exception cref="FrameworkException">当未找到且 throwExceptionIfNotFound 为 true 时抛出。</exception>
        public static GameObject GetGameObjectInHierarchy(
            this string hierarchyPath,
            GameObject  parent,
            bool        includeInactive          = true,
            bool        throwExceptionIfNotFound = true)
        {
            return hierarchyPath.GetGameObjectInHierarchy(parent.transform, includeInactive, throwExceptionIfNotFound);
        }

        /// <summary>
        ///     根据层级路径从指定父级 Transform 获取指定类型的组件。
        /// </summary>
        /// <typeparam name="T">组件类型。</typeparam>
        /// <param name="hierarchyPath">层级路径，使用 '/' 分隔每一级名称。</param>
        /// <param name="parent">父级 Transform。</param>
        /// <param name="includeInactive">是否包含未激活的 GameObject。</param>
        /// <param name="throwExceptionIfNotFound">如果未找到，是否抛出异常。</param>
        /// <returns>找到的组件实例。</returns>
        /// <exception cref="FrameworkException">当未找到且 throwExceptionIfNotFound 为 true 时抛出。</exception>
        public static T GetComponentInHierarchy<T>(
            this string hierarchyPath,
            Transform   parent,
            bool        includeInactive          = true,
            bool        throwExceptionIfNotFound = true)
            where T : Component
        {
            var obj       = hierarchyPath.GetGameObjectInHierarchy(parent.transform, includeInactive, false);
            var component = obj?.GetComponent<T>();

            if (throwExceptionIfNotFound && component == null)
            {
                throw new FrameworkException($"Can not find component {typeof(T)} from {parent} in hierarchy path: {hierarchyPath}");
            }

            return component;
        }

        /// <summary>
        ///     根据层级路径从指定父级 Component 获取指定类型的组件。
        /// </summary>
        /// <typeparam name="T">组件类型。</typeparam>
        /// <param name="hierarchyPath">层级路径，使用 '/' 分隔每一级名称。</param>
        /// <param name="parent">父级 Component。</param>
        /// <param name="includeInactive">是否包含未激活的 GameObject。</param>
        /// <param name="throwExceptionIfNotFound">如果未找到，是否抛出异常。</param>
        /// <returns>找到的组件实例。</returns>
        /// <exception cref="FrameworkException">当未找到且 throwExceptionIfNotFound 为 true 时抛出。</exception>
        public static T GetComponentInHierarchy<T>(
            this string hierarchyPath,
            Component   parent,
            bool        includeInactive          = true,
            bool        throwExceptionIfNotFound = true)
            where T : Component
        {
            return hierarchyPath.GetComponentInHierarchy<T>(parent.transform, includeInactive, throwExceptionIfNotFound);
        }

        /// <summary>
        ///     根据层级路径从指定父级 GameObject 获取指定类型的组件。
        /// </summary>
        /// <typeparam name="T">组件类型。</typeparam>
        /// <param name="hierarchyPath">层级路径，使用 '/' 分隔每一级名称。</param>
        /// <param name="parent">父级 GameObject。</param>
        /// <param name="includeInactive">是否包含未激活的 GameObject。</param>
        /// <param name="throwExceptionIfNotFound">如果未找到，是否抛出异常。</param>
        /// <returns>找到的组件实例。</returns>
        /// <exception cref="FrameworkException">当未找到且 throwExceptionIfNotFound 为 true 时抛出。</exception>
        public static T GetComponentInHierarchy<T>(
            this string hierarchyPath,
            GameObject  parent,
            bool        includeInactive          = true,
            bool        throwExceptionIfNotFound = true)
            where T : Component
        {
            return hierarchyPath.GetComponentInHierarchy<T>(parent.transform, includeInactive, throwExceptionIfNotFound);
        }

        /// <summary>
        ///     根据层级路径获取或创建指定的 GameObject。
        /// </summary>
        /// <param name="hierarchyPath">层级路径，使用 '/' 分隔每一级名称。</param>
        /// <param name="parent">父级 Transform。</param>
        /// <param name="includeInactive">是否包含未激活的 GameObject。</param>
        /// <returns>找到或创建的 GameObject。</returns>
        public static GameObject GetOrAddGameObjectInHierarchy(
            this string hierarchyPath,
            Transform   parent,
            bool        includeInactive = true)
        {
            var objNames = hierarchyPath.Split('/');
            var obj      = parent.gameObject;

            // 层次遍历，广度优先
            for (var i = 0; i < objNames.Length; i++)
            {
                var name = objNames[i];

                // 记录父物体
                var parentTrans = obj.transform;

                for (var j = 0; j < parentTrans.childCount; j++)
                {
                    var child = obj.transform.GetChild(j);
                    if ((includeInactive || child.gameObject.activeInHierarchy) && child.name == name)
                    {
                        // 找到，指向子物体
                        obj = child.gameObject;
                        break;
                    }
                }

                // 如果找完所在层所有子物体后，obj 没有变化，则说明没找到
                if (obj == parentTrans.gameObject)
                {
                    obj = objNames[i..].Join("/").AddGameObjectInHierarchy(parentTrans);
                    break;
                }
            }

            return obj;
        }

        /// <summary>
        ///     根据层级路径获取或创建指定的 GameObject。
        /// </summary>
        /// <param name="hierarchyPath">层级路径，使用 '/' 分隔每一级名称。</param>
        /// <param name="parent">父级 Component。</param>
        /// <param name="includeInactive">是否包含未激活的 GameObject。</param>
        /// <returns>找到或创建的 GameObject。</returns>
        public static GameObject GetOrAddGameObjectInHierarchy(
            this string hierarchyPath,
            Component   parent,
            bool        includeInactive = true)
        {
            return hierarchyPath.GetOrAddGameObjectInHierarchy(parent.transform, includeInactive);
        }

        /// <summary>
        ///     根据层级路径获取或创建指定的 GameObject。
        /// </summary>
        /// <param name="hierarchyPath">层级路径，使用 '/' 分隔每一级名称。</param>
        /// <param name="parent">父级 GameObject。</param>
        /// <param name="includeInactive">是否包含未激活的 GameObject。</param>
        /// <returns>找到或创建的 GameObject。</returns>
        public static GameObject GetOrAddGameObjectInHierarchy(
            this string hierarchyPath,
            GameObject  parent,
            bool        includeInactive = true)
        {
            return hierarchyPath.GetOrAddGameObjectInHierarchy(parent.transform, includeInactive);
        }

        /// <summary>
        ///     根据层级路径获取或创建指定类型的组件。
        /// </summary>
        /// <typeparam name="T">组件类型。</typeparam>
        /// <param name="hierarchyPath">层级路径，使用 '/' 分隔每一级名称。</param>
        /// <param name="parent">父级 Transform。</param>
        /// <param name="includeInactive">是否包含未激活的 GameObject。</param>
        /// <returns>找到或创建的组件实例。</returns>
        public static T GetOrAddComponentInHierarchy<T>(
            this string hierarchyPath,
            Transform   parent,
            bool        includeInactive = true)
            where T : Component
        {
            var obj       = hierarchyPath.GetOrAddGameObjectInHierarchy(parent.transform, includeInactive);
            var component = obj.GetOrAddComponent<T>();
            return component;
        }

        /// <summary>
        ///     根据层级路径获取或创建指定类型的组件。
        /// </summary>
        /// <typeparam name="T">组件类型。</typeparam>
        /// <param name="hierarchyPath">层级路径，使用 '/' 分隔每一级名称。</param>
        /// <param name="parent">父级 Component。</param>
        /// <param name="includeInactive">是否包含未激活的 GameObject。</param>
        /// <returns>找到或创建的组件实例。</returns>
        public static T GetOrAddComponentInHierarchy<T>(
            this string hierarchyPath,
            Component   parent,
            bool        includeInactive = true)
            where T : Component
        {
            return hierarchyPath.GetOrAddComponentInHierarchy<T>(parent.transform, includeInactive);
        }

        /// <summary>
        ///     根据层级路径获取或创建指定类型的组件。
        /// </summary>
        /// <typeparam name="T">组件类型。</typeparam>
        /// <param name="hierarchyPath">层级路径，使用 '/' 分隔每一级名称。</param>
        /// <param name="parent">父级 GameObject。</param>
        /// <param name="includeInactive">是否包含未激活的 GameObject。</param>
        /// <returns>找到或创建的组件实例。</returns>
        public static T GetOrAddComponentInHierarchy<T>(
            this string hierarchyPath,
            GameObject  parent,
            bool        includeInactive = true)
            where T : Component
        {
            return hierarchyPath.GetOrAddComponentInHierarchy<T>(parent.transform, includeInactive);
        }
    }
}