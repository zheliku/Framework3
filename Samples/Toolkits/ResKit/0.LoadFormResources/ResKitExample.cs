// ------------------------------------------------------------
// @file       ResKitExample.cs
// @brief
// @author     zheliku
// @Modified   2024-12-10 11:12:28
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.ResKit.Example._0.Basic
{
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public class ResKitExample : MonoBehaviour
    {
        private GameObject _res;
        
        private ResourceRequest _req;

        private void Start()
        {
            // 同步加载
            _res = ResKit.LoadFromResources<GameObject>("Sphere");
            Instantiate(_res);
            
            // 异步加载
            _req = ResKit.LoadFromResourcesAsync<GameObject>("Sphere", res =>
            {
                var obj = Instantiate(res);
                obj.transform.position = Vector3.up;
            });

            // var asset = ResKit.Instantiate("Cube");
            // Instantiate(asset.Result);
            //
            // // 异步加载
            // _handle = ResKit.LoadAssetAsync<GameObject>("Cube", (res) =>
            // {
            //     var obj = Instantiate(res);
            //     obj.transform.position = Vector3.right;
            // });
            //
            // ResKit.LoadAssetAsync<GameObject>("Cube", (res) =>
            // {
            //     var obj = Instantiate(res);
            //     obj.transform.position = Vector3.left;
            // });
        }

        private void OnDestroy()
        {
            // 加载后的资源需要及时释放
            _res.Unload();
            _req.Unload();
        }
    }
}