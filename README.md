# Unity Object Pooling System

A highly performant, automated Object Pooling System for Unity. Built as a wrapper around Unity's native "UnityEngine.Pool" API, this system eliminates frame drops and Garbage Collection (GC) spikes caused by rapidly instantiating and destroying objects like bullets, enemies, or particle effects.

---

## ✨ Features

* **Zero Setup Required:** You do not need to pre-configure pools in the inspector. Pass any prefab into the "Spawn" method, and the Manager will automatically generate and track a pool for it.
* **Native Performance:** Leverages the modern "ObjectPool<T>" struct introduced in Unity 2021, ensuring maximum memory efficiency.
* **Smart Tracking:** Spawned prefabs are automatically given a "PooledObject" component, allowing them to release themselves back into the correct pool without needing to talk to the Manager.
* **Auto-Despawner:** Includes a handy helper script to automatically recycle objects after a set duration (perfect for muzzle flashes, hit sparks, and projectiles).

---

## 🧠 Design Notes

Older pooling systems often required developers to create large lists in the inspector, assigning pre-warmed amounts for every single object in the game. This was tedious and bloated.

This system takes a **Dictionary-driven, Just-In-Time (JIT)** approach. The "PoolManager" acts as a Singleton factory. When you request a "BulletPrefab", it checks its Dictionary. If a pool for that prefab doesn't exist, it creates one on the fly. By dynamically attaching a "PooledObject" component to the clones, the clones become self-aware of their parent pool, making despawning completely decoupled and error-free.

---

## 📂 Included Scripts

* "PoolManager.cs" - The core Singleton that handles creating pools, tracking prefabs, and dispensing GameObjects.
* "PooledObject.cs" - Automatically attached to spawned items. Holds a reference to its specific pool so it can quickly recycle itself.
* "AutoDespawn.cs" - An optional helper component. Add this to your prefabs to have them automatically return to the pool after $X$ seconds.

---

## 🧩 How To Use

1. **Setup the Manager:** Create an empty GameObject in your scene named "PoolManager" and attach the "PoolManager.cs" script.
2. **Spawning Objects:** Instead of using Unity's native "Instantiate", call the pool manager from any script:

public GameObject bulletPrefab;

void Shoot()
{
    PoolManager.Instance.Spawn(bulletPrefab, transform.position, transform.rotation);
}
"
3. **Despawning Objects (Manually):** If you want an enemy to die or a bullet to disappear on impact, just get the "PooledObject" component and call release:

void OnCollisionEnter(Collision col)
{
    GetComponent<PooledObject>().Release();
}
"
4. **Despawning Objects (Automatically):** If you are spawning an explosion particle, just add the "AutoDespawn.cs" script to the explosion prefab and set the delay to 2 seconds. It will handle the rest!

---

## 🚀 Possible Extensions

* **Pre-warming Pools:** Add a method to "PoolManager" that accepts an Array of Prefabs and an integer, looping through them on "Start()" to generate the pools before the gameplay begins to avoid mid-game JIT spikes.
* **Pool Limits/Stealing:** If a pool reaches its max capacity, write logic to find the oldest active object in the scene, disable it, and repurpose it for the new spawn request.
* **Interface Implementations:** Create an "IPoolable" interface with an "OnSpawn" and "OnDespawn" method so complex enemies can reset their health and AI states automatically when pulled from the pool.

---

## 🛠 Unity Version

* Requires **Unity 2021 LTS** or higher (relies on the native "UnityEngine.Pool" namespace).
* Supports all Render Pipelines (Built-in, URP, HDRP).

---

## 📜 License

MIT
