// ------------------------------------------------------------
// @file       LoadSceneExample.cs
// @brief
// @author     zheliku
// @Modified   2024-12-10 16:12:50
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.ResKit.Example._2.LoadScene
{
    using FluentAPI;
    using UnityEngine;

    public class LoadSceneExample : MonoBehaviour
    {
        private void Start()
        {
            ResKit.LoadSceneAsync("TestScene", () =>
            {
                ResKit.LoadFromResources<GameObject>("Sphere").Instantiate();
            });
        }
    }
}