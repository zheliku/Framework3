// ------------------------------------------------------------
// @file       JsonExample.cs
// @brief
// @author     zheliku
// @Modified   2024-12-07 00:12:57
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.Core.DataKit.Example._1Json
{
    using Toolkits.DataKit;
    using UnityEngine;

    public class JsonData
    {
        public int    Age;
        public string Name;

        public override string ToString()
        {
            return $"Name: {Name}, Age: {Age}";
        }
    }

    public class JsonExample : MonoBehaviour
    {
        public void Save()
        {
            DataKit.SaveJson("example", new JsonData
            {
                Name = "hello",
                Age  = 18
            });
        }

        public void Load()
        {
            var data = DataKit.LoadJson<JsonData>("example");
            Debug.Log(data);
        }

        public void OpenFolder()
        {
        #if UNITY_EDITOR
            JsonHelper.OpenJsonSavePath();
        #endif
        }
    }
}