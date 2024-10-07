using UnityEngine;

public class PlayerShooter : Shooter
{
    [Header("Player Settings")]
    [SerializeField] private float fireRate = 1f;
    
    private float nextFireTime = 0f;
    
    private void Update()
    {
        if (!Input.GetMouseButtonDown(0) || !CanShoot() || !(Time.time >= nextFireTime)) return;
        
        SpawnObject();
        nextFireTime = Time.time + fireRate;
    }

    protected override void SpawnObject()
    {
        Projectile bullet = weapons.GetAmmo();
        bullet.Spawn(spawnPoint.position, spawnPoint.forward, Reload, weapons.ID);

        ammo.Consume();
    }

    protected override void Reload(int id)
    {
        if (id <= -1)
            ammo.Reload();
        else
        {
            // reload to the assigned weapon
            weapons.data[id].Reload();            
        }
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 880, 150, 50), $"Lots of ammo on \n {ammo.weaponName}"))
        {
            ammo.SetNewMaxAmmo(1000);
        }
    }
#endif
}