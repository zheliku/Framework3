// ------------------------------------------------------------
// @file       SimpleObjectPoolExample.cs
// @brief
// @author     zheliku
// @Modified   2024-10-23 22:10:22
// @Copyright  Copyright (c) 2024, zheliku
// ------------------------------------------------------------

namespace Framework3.Toolkits.PoolKit.Example._0.ObjectPoolExample
{
    using System.Collections;
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class ObjectPoolExample : MonoBehaviour
    {
        [ShowInInspector]
        private ObjectPool<GameObject> _objectPool;

        void Start()
        {
            _objectPool = new ObjectPool<GameObject>(
                () =>
                {
                    var gameObj = new GameObject();
                    gameObj.SetActive(false);
                    return gameObj;
                },
                actionOnRelease: gameObj => { gameObj.SetActive(false); },
                actionOnDestroy: Destroy,
                defaultCapacity: 10);
        }
        
        public void GetGameObjectFromPool()
        {
            var obj = _objectPool.Get();
            obj.SetActive(true);
            StartCoroutine(Recycle(obj));
        }
        
        public void ClearGameObjectPool()
        {
            _objectPool.Clear();
        }

        private IEnumerator Recycle(GameObject obj)
        {
            yield return new WaitForSeconds(1);
            _objectPool.Release(obj);
        }
    }
}