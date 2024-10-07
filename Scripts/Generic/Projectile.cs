using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private WeaponSO weapon = null;

    private int currentBounces = 0;
    private int weaponId = 0;
    
    private Vector3 moveDirection;
    private Vector3 lastVelocity;
    
    private Rigidbody rb;
    
    private IObjectPool<Projectile> objectPool;
    public IObjectPool<Projectile> ObjectPool { set => objectPool = value; }

    private Action<int> onReleased = null;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Deactivate(true);
    }
    
    public void Spawn(Vector3 position, Vector3 direction, Action<int> released = null, int id = -1)
    {
        rb.velocity = Vector3.zero;
        
        transform.position = position;
        
        moveDirection = direction.normalized;
        rb.velocity = moveDirection * weapon.speed;

        weaponId = id;
        onReleased = released;
    }
    
    private void LateUpdate()
    {
        lastVelocity = rb.velocity;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        AudioManager.Instance.Bullet(weapon.bulletType, this.transform.localScale);
        
        if (collision.gameObject.TryGetComponent(out IDamageable tank))
        {
            tank.Damage();
            Release();
        }
        else
        {
            if (currentBounces >= weapon.bounceAmount)
            {
                Deactivate(false);
            }
            else
            {
                Vector3 direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
                rb.velocity = direction * weapon.speed;
                ++currentBounces;
            }
        }
    }

    public void Deactivate(bool useTimer = false)
    {
        if (useTimer)
            StartCoroutine(DeactivateRoutine(weapon.timeToDestroy));
        else
            Release();
    }

    IEnumerator DeactivateRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        Release();
    }

    private void Release()
    {
        StopAllCoroutines();
        
        currentBounces = 0;
        rb.velocity = Vector3.zero;

        onReleased?.Invoke(weaponId);
        onReleased = null;
        
        // release the projectile back to the pool
        objectPool.Release(this);
    }
}
