// ------------------------------------------------------------
// @file       2.SystemIOExtension.cs
// @brief
// @author     zheliku
// @Modified   2024-10-17 23:10:39
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.FluentAPI
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    ///     针对 System.IO 提供的链式扩展，主要是文件和文件夹的 IO 操作
    /// </summary>
    public static class SystemIOExtension
    {
        /// <summary>
        ///     确保文件路径中的扩展名正确，不正确或没有扩展名，则修改或添加
        /// </summary>
        /// <example>
        ///     <code>
        /// <![CDATA[
        /// ChangeExtension("C:\mydir\myfile.com.extension", ".old") --> "C:\mydir\myfile.com.old"
        /// ChangeExtension("C:\mydir\myfile.com.extension", "")     --> "C:\mydir\myfile.com."
        /// ChangeExtension("C:\mydir\", ".old")                     --> "C:\mydir\.old"
        /// ]]>
        /// </code>
        /// </example>
        public static string ChangeExtension(this string filePath, string extension)
        {
            return Path.ChangeExtension(filePath, extension);
        }

        /// <summary>
        ///     移除文件路径中的扩展名。
        /// </summary>
        /// <param name="filePath">包含扩展名的文件路径。</param>
        /// <returns>移除扩展名后的文件路径。</returns>
        public static string RemoveExtension(this string filePath)
        {
            var withDot = Path.ChangeExtension(filePath, ""); // 结果末尾会留有 "."
            return withDot[..^1];                             // 去掉末尾的 "."
        }

        /// <summary>
        ///     获取文件夹路径
        /// </summary>
        /// <example>
        ///     <code>
        /// <![CDATA[
        /// var fileName ="/abc/def/b.txt".GetFileNameWithoutExtension();
        /// Debug.Log(fileName); // /abc/def
        ///  
        /// GetDirectoryName("C:\MyDir\MySubDir\myfile.ext") --> "C:\MyDir\MySubDir"
        /// GetDirectoryName("C:\MyDir\MySubDir")            --> "C:\MyDir"
        /// GetDirectoryName("C:\MyDir\")                    --> "C:\MyDir"
        /// GetDirectoryName("C:\MyDir")                     --> "C:\"
        /// GetDirectoryName("C:\")                          --> ""
        /// ]]>
        /// </code>
        /// </example>
        public static string GetDirectoryName(this string path)
        {
            return Path.GetDirectoryName(path);
        }

        /// <summary>
        ///     确保文件路径中的文件夹存在，如果不存在则创建。
        /// </summary>
        /// <param name="path">文件路径或文件夹路径。</param>
        /// <returns>原始路径。</returns>
        public static string EnsureDirectoryExist(this string path)
        {
            var dir = path.HasExtension() ? Path.GetDirectoryName(path) : path;

            // 如果文件夹不存在，则创建
            if (dir != null && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return path;
        }

        /// <summary>
        ///     创建文件夹，如果存在则不操作
        /// </summary>
        /// <example>
        ///     <code>
        /// <![CDATA[
        /// "Assets/TestFolder".CreateDirectory();"
        /// ]]>
        /// </code>
        /// </example>
        public static string CreateDirectory(this string fullPath)
        {
            Directory.CreateDirectory(fullPath);

            return fullPath;
        }

        /// <summary>
        ///     删除文件夹，如果不存在则不操作
        /// </summary>
        /// <example>
        ///     <code>
        /// <![CDATA[
        /// "Assets/TestFolder".DeleteDirectory();"
        /// ]]>
        /// </code>
        /// </example>
        public static bool DeleteDirectory(this string fullPath)
        {
            if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath, true);
                return true;
            }

            return false;
        }

        /// <summary>
        ///     删除文件，如果不存在则不操作
        /// </summary>
        /// <example>
        ///     <code>
        /// <![CDATA[
        /// "Assets/TestFile.txt".DeleteFile();"
        /// ]]>
        /// </code>
        /// </example>
        public static bool DeleteFile(this string fullPath)
        {
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }

            return false;
        }

        /// <summary>
        ///     合并路径
        /// </summary>
        /// <example>
        ///     <code>
        /// <![CDATA[
        /// var path = Application.dataPath.CombinePath("Resources");
        /// Debug.Log(Path) // projectPath/Assets/Resources
        /// ]]>
        /// </code>
        /// </example>
        public static string CombinePath(this string path1, params string[] path2)
        {
            var list = new List<string> { path1 };
            list.AddRange(path2);
            return Path.Combine(list.ToArray());
        }

        /// <summary>
        ///     根据路径获取文件名
        /// </summary>
        /// <example>
        ///     <code>
        /// <![CDATA[
        /// var fileName ="/abc/def/b.txt".GetFileName();
        /// Debug.Log(fileName); // b.txt
        ///  
        /// GetFileName("C:\mydir\myfile.ext")        --> "myfile.ext"
        /// GetFileName("C:\mydir\")                  --> ""
        /// GetFileName("C:\mydir\myfile.ext", false) --> "myfile"
        /// GetFileName("C:\mydir\")                  --> ""
        /// ]]>
        /// </code>
        /// </example>
        public static string GetFileName(this string filePath, bool withExtension = true)
        {
            return withExtension ? Path.GetFileName(filePath) : Path.GetFileNameWithoutExtension(filePath);
        }

        /// <summary>
        ///     根据路径获取文件扩展名
        /// </summary>
        /// <example>
        ///     <code>
        /// <![CDATA[
        /// var fileName ="/abc/def/b.txt".GetExtension();
        /// Debug.Log(fileName); // .txt
        /// ]]>
        /// </code>
        /// </example>
        public static string GetExtension(this string filePath)
        {
            return Path.GetExtension(filePath);
        }

        /// <summary>
        ///     根据路径判断是否含有文件扩展名
        /// </summary>
        /// <example>
        ///     <code>
        /// <![CDATA[
        /// var has ="/abc/def/b.txt".HasExtension();
        /// Debug.Log(has); // true
        /// ]]>
        /// </code>
        /// </example>
        public static bool HasExtension(this string filePath)
        {
            return Path.HasExtension(filePath);
        }

        /// <summary>
        ///     将路径转换为当前环境下的路径（替换分隔符）
        /// </summary>
        /// <example>
        ///     <code>
        /// <![CDATA[
        /// var fileName ="/abc/def/b.txt".ConvertToCurrentEnvironmentPath();
        /// Debug.Log(fileName); // \\abc\\def\\b.txt
        /// ]]>
        /// </code>
        /// </example>
        public static string ConvertToCurrentEnvironmentPath(this string filePath)
        {
            return filePath.Replace("/", Path.DirectorySeparatorChar.ToString())
               .Replace("\\", Path.DirectorySeparatorChar.ToString());
        }

        /// <summary>
        ///     判断指定路径字符串是否为绝对路径
        /// </summary>
        /// <example>
        ///     <code>
        /// <![CDATA[
        /// var rooted ="/abc/def/b.txt".IsPathRooted();
        /// Debug.Log(rooted); // false
        /// ]]>
        /// </code>
        /// </example>
        public static bool IsPathRooted(this string filePath)
        {
            return Path.IsPathRooted(filePath);
        }

        /// <summary>
        ///     判断指定文件是否存在。
        /// </summary>
        /// <param name="filePath">要检查的文件路径。</param>
        /// <returns>如果文件存在则返回 true，否则返回 false。</returns>
        public static bool ExistsFile(this string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        ///     获取文件夹下所有文件，包括子文件夹中的文件。
        /// </summary>
        /// <param name="directoryPath">要搜索的文件夹路径。</param>
        /// <param name="searchPattern">搜索模式（默认为 "*"，匹配所有文件）。</param>
        /// <param name="localPath">是否返回相对于 <paramref name="directoryPath" /> 的本地路径。</param>
        /// <returns>包含所有文件路径的列表。</returns>
        public static List<string> GetAllFiles(this string directoryPath, string searchPattern = "*", bool localPath = false)
        {
            var fileList = new List<string>();

            foreach (var filePath in Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories))
            {
                var path = filePath;
                if (localPath)
                {
                    path = path.TrimStart(directoryPath).TrimStart("/").TrimStart("\\");
                }
                fileList.Add(path);
            }
            return fileList;
        }
    }
}