using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [System.Serializable]
    struct WeaponContainer
    {
        public WeaponSO weapon;
        public AmmoSpawner spawner;
    }

    public static WeaponManager Instance = null;
    
    [SerializeField] private WeaponContainer[] weaponTypes = null;

    private void Awake()
    {
        //NOTE: this might cause issues because it's a singleton. fix this if issues occured
        Instance = this;
        
        foreach (WeaponContainer item in weaponTypes)
            item.spawner.Create(item.weapon);
    }

    public Projectile Ammo(WeaponSO type)
    {
        for (int i = 0; i < weaponTypes.Length; i++)
        {
            if (weaponTypes[i].weapon != type) continue;
            return weaponTypes[i].spawner.Pool().Get();
        }

        return null;
    }
}
