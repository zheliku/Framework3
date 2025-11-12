// ------------------------------------------------------------
// @file       TxtHelper.cs
// @brief
// @author     zheliku
// @Modified   2024-12-06 23:12:37
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.DataKit
{
#if ODIN_INSPECTOR
    using Sirenix.Serialization;
    using SerializationUtility = Sirenix.Serialization.SerializationUtility;
#endif
    using System.IO;
    using FluentAPI;
    using UnityEditor;
    using Debug = UnityEngine.Debug;

    // 1. 引用命名空间
#if UNITY_WEBGL && !UNITY_EDITOR
    using System.Runtime.InteropServices;
#endif

    /// <summary>
    ///     WebGL 端数据存储参考：<see href="https://blog.csdn.net/xiaotaiyang_gege/article/details/135862196" />
    /// </summary>
    public class BinaryHelper
    {
        // 2. 加入外部函数声明
    #if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void SyncDB();
    #endif

        public const string EXTENSION = ".bytes";

    #if UNITY_EDITOR
        [MenuItem("Framework3/DataKit/Open Binary Folder")]
        public static void OpenBinarySavePath()
        {
            DataKit.BinarySavePath.EnsureDirectoryExist();
            EditorUtility.RevealInFinder(DataKit.BinarySavePath.ConvertToCurrentEnvironmentPath());
        }
    #endif

        /// <summary>
        ///     处理输入路径，使得路径符合 TxtHelper 下的路径
        /// </summary>
        private static string ProcessPath(string path)
        {
            return DataKit.BinarySavePath.CombinePath(path);
        }

        /// <summary>
        ///     存储数据为 bytes 文件
        /// </summary>
        /// <param name="filePath">文件路径，可以不写后缀</param>
        /// <param name="data">存储数据，如果是自定义结构，则需要添加 [Serializable] 特性</param>
        /// <param name="extension">文件扩展名</param>
        public static void Save<TData>(string filePath, TData data, string extension = EXTENSION)
        {
        #if ODIN_INSPECTOR
            var fullPath = ProcessPath(filePath). // 处理输入路径
                ChangeExtension(extension)        // 确保文件路径扩展名为指定格式
               .EnsureDirectoryExist();           // 确保文件所在目录存在

            var bytes = SerializationUtility.SerializeValue(data, DataFormat.Binary);
            File.WriteAllBytes(fullPath, bytes);

            // 3. 调用外部函数
        #if UNITY_WEBGL && !UNITY_EDITOR
    	    SyncDB();
        #endif

        #if UNITY_EDITOR
            AssetDatabase.Refresh();
        #endif
        #else
            Debug.LogError("BinaryHelper: Save function requires Odin Inspector package.");
        #endif
        }

        /// <summary>
        ///     读取 bytes 文件中的数据
        /// </summary>
        /// <param name="filePath">文件路径，可以不写后缀</param>
        /// <param name="extension">文件扩展名</param>
        public static TData Load<TData>(string filePath, string extension = EXTENSION)
        {
        #if ODIN_INSPECTOR
            var fullPath = ProcessPath(filePath). // 处理输入路径
                ChangeExtension(extension);       // 确保文件路径扩展名为指定格式

            if (!File.Exists(fullPath))
            { // 不存在文件，则警告，并返回默认值
                Debug.LogWarning($"BinaryHelper: Can't find path \"{fullPath}\"");
                return default(TData);
            }

            var bytes = File.ReadAllBytes(fullPath);
            return SerializationUtility.DeserializeValue<TData>(bytes, DataFormat.Binary);
        #else
            Debug.LogError("BinaryHelper: Save function requires Odin Inspector package.");
            return default(TData);
        #endif
        }
    }
}