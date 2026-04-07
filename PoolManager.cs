// 2. PoolManager.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    // Maps a specific Prefab to its respective Object Pool
    private Dictionary<GameObject, IObjectPool<GameObject>> pools = new Dictionary<GameObject, IObjectPool<GameObject>>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Spawns an object from the pool. If no pool exists for this prefab, one is created automatically.
    /// </summary>
    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!pools.ContainsKey(prefab))
        {
            CreatePool(prefab);
        }

        GameObject obj = pools[prefab].Get();
        obj.transform.SetPositionAndRotation(position, rotation);
        return obj;
    }

    private void CreatePool(GameObject prefab)
    {
        IObjectPool<GameObject> newPool = null;

        newPool = new ObjectPool<GameObject>(
            createFunc: () => 
            {
                // Instantiate the object and automatically add the PooledObject tracker
                GameObject obj = Instantiate(prefab, transform);
                PooledObject pooledObj = obj.AddComponent<PooledObject>();
                pooledObj.SetPool(newPool);
                return obj;
            },
            actionOnGet: (obj) => obj.SetActive(true),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: false, // Set to true if you want to throw errors on double-releases (costs performance)
            defaultCapacity: 10,
            maxSize: 1000
        );

        pools.Add(prefab, newPool);
    }
}
