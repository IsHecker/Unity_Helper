using System.Collections.Generic;
using UnityEngine;

namespace UnityHelper
{
    public class ObjectPool
    {
        [System.Serializable]
        public struct Pool
        {
            public string poolName;
            public GameObject prefab;
            public int size;
        }

        private readonly Pool[] pools;

        private readonly Dictionary<string, Queue<GameObject>> poolDictionary;

        private readonly Transform objectPool;

        private readonly int poolSize;

        public ObjectPool(GameObject[] data, int poolSize)
        {
            pools = new Pool[data.Length];

            var result = GameObject.Find("Object Pool");

            objectPool = result != null ? result.transform : new GameObject("Object Pool").transform;
            poolDictionary = new Dictionary<string, Queue<GameObject>>();
            this.poolSize = poolSize;

            CreatePool(data);

            // Fill the pools with GameObject instances.
            foreach (var pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    var obj = UnityEngine.Object.Instantiate(pool.prefab, this.objectPool);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }
                poolDictionary.Add(pool.poolName, objectPool);
            }
        }

        public GameObject GetFromPool(string poolName, Vector3 position, Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(poolName))
                return null;

            var objectToSpawn = poolDictionary[poolName].Dequeue();
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.SetPositionAndRotation(position, rotation);

            poolDictionary[poolName].Enqueue(objectToSpawn);

            return objectToSpawn;
        }

        private void CreatePool(GameObject[] data)
        {
            //GameObjectUtility
            for (int i = 0; i < data.Length; i++)
            {
                pools[i] = new Pool
                {
                    poolName = data[i].name,
                    prefab = data[i],
                    size = poolSize
                };
            }

        }
    }
}
