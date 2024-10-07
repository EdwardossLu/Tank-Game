using UnityEngine;
using Sirenix.OdinInspector;

public abstract class Shooter : MonoBehaviour
{
    [Header("Setup")]
    [SceneObjectsOnly, SerializeField] protected WeaponController weapons;
    [SceneObjectsOnly, SerializeField] protected Transform spawnPoint;
    
    [Header("Checks")]
    [Range(0f, 1f), SerializeField] protected float collisionDistanceCheck = 0.5f;
    [Range(1f, 2f), SerializeField] protected float backCheck = 0.5f;
    [SerializeField] protected LayerMask targetLayers = 0;

    protected AmmoData ammo => weapons.ammo;
    
    protected bool CanShoot()
    {
        return ammo.IsAvailable() && IsClearToShoot();
    }
    
    private bool IsClearToShoot()
    {
        Vector3 sp = spawnPoint.position;
        Vector3 fwd = spawnPoint.TransformDirection(Vector3.back);
        
        return !Physics.CheckSphere(sp, collisionDistanceCheck, targetLayers) &&    // Check the surrounding area
               !Physics.Raycast(sp, fwd, backCheck, targetLayers);      // Check if anything is behind the spawn point
    }
    
    protected abstract void SpawnObject();
    
    protected abstract void Reload(int id);

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        bool isClear = IsClearToShoot();
        Vector3 sp = spawnPoint.position;
        
        Gizmos.color = isClear ? Color.green : Color.red;
        Gizmos.DrawSphere(sp, collisionDistanceCheck);
        Gizmos.DrawLine(sp, sp + (-spawnPoint.forward * backCheck));
    }
#endif
}
