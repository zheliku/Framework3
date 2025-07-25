﻿// ------------------------------------------------------------
// @file       ReplaceableMonoSingletonExample.cs
// @brief
// @author     zheliku
// @Modified   2024-10-20 19:10:56
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.SingletonKit.Example._6.ReplaceableMonoSingletonExample
{
    using System.Collections;
    using UnityEngine;
    using Framework3.Core;

    public class ReplaceableMonoSingletonExample : MonoBehaviour
    {
        // Use this for initialization
        IEnumerator Start()
        {
            // 创建一个单例
            var instance = GameManager.Instance;

            // 强制创建一个实例
            new GameObject().AddComponent<GameManager>();

            // 等一帧，等待第二个 GameManager 把自己删除
            yield return new WaitForEndOfFrame();

            // 结果为 1 
            Debug.Log(FindObjectsByType<GameManager>(FindObjectsSortMode.None).Length);

            // 最先创建的实例已经被删除
            Debug.Log(instance != FindFirstObjectByType<GameManager>());
        }
    }

    internal class GameManager : ReplaceableMonoSingleton<GameManager>
    { }
}