using UnityEngine;
using UnityEngine.Pool;

public class AmmoSpawner : MonoBehaviour
{
    // throw an exception if we try to return an existing item, already in the pool
    [SerializeField] private bool collectionCheck = true;

    // extra options to control the pool capacity and maximum size
    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxSize = 100;

    private WeaponSO weaponType = null;
    private Projectile projectileType = null; 
    
    private IObjectPool<Projectile> objectPool;

    public void Create(WeaponSO weaponObject)
    {
        weaponType = weaponObject;
        projectileType = weaponType.projectile;
        
        objectPool = new ObjectPool<Projectile>(CreateProjectile,
            OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
            collectionCheck, defaultCapacity, maxSize);
    }

    // invoked when creating an item to populate the object pool
    private Projectile CreateProjectile()
    {
        Projectile projectileInstance = Instantiate(projectileType);
        projectileInstance.ObjectPool = objectPool;
        return projectileInstance;
    }

    // invoked when returning an item to the object pool
    private void OnReleaseToPool(Projectile pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }

    // invoked when retrieving the next item from the object pool
    private void OnGetFromPool(Projectile pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }

    // invoked when we exceed the maximum number of pooled items (i.e. destroy the pooled object)
    private void OnDestroyPooledObject(Projectile pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }

    public IObjectPool<Projectile> Pool()
    {
        return objectPool;
    }
}
