// 1. PooledObject.cs
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Attached automatically to spawned objects to keep track of which pool they belong to.
/// </summary>
public class PooledObject : MonoBehaviour
{
    private IObjectPool<GameObject> pool;

    public void SetPool(IObjectPool<GameObject> poolReference)
    {
        pool = poolReference;
    }

    public void Release()
    {
        if (pool != null)
        {
            pool.Release(gameObject);
        }
        else
        {
            Destroy(gameObject); // Fallback just in case it wasn't spawned via the manager
        }
    }
}
