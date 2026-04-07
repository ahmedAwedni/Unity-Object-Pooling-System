// 3. AutoDespawn.cs
using UnityEngine;
using System.Collections;

/// <summary>
/// A helper script to automatically return objects to the pool after a set time.
/// Perfect for particle effects or bullets.
/// </summary>
[RequireComponent(typeof(PooledObject))]
public class AutoDespawn : MonoBehaviour
{
    [Tooltip("Time in seconds before the object returns to the pool.")]
    public float delay = 2f;
    private PooledObject pooledObject;

    private void Awake()
    {
        // Because PoolManager adds PooledObject on creation, we can safely grab it here
        pooledObject = GetComponent<PooledObject>();
    }

    private void OnEnable()
    {
        StartCoroutine(DespawnRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator DespawnRoutine()
    {
        yield return new WaitForSeconds(delay);
        pooledObject.Release();
    }
}
