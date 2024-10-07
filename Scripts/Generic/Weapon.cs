using UnityEngine;

public enum BulletType { Rocket, FastRocket }

[System.Serializable]
public class AmmoData
{
    // setup
    public string weaponName;           // used for inspector visualization through odin inspector
    public int current;
    public int max;
    
    private WeaponSO data = null;
    
    public AmmoData(WeaponSO type, int id)
    {
        data = type;
        
        weaponName = data.name;
        max = data.minAmmo;
        current = max;
    }

    public void Consume()
    {
        --current;
        current = Mathf.Clamp(current, 0, max);
    }

    public void Reload()
    {
        ++current;
        current = Mathf.Clamp(current, 0, max);
    }

    public bool IsAvailable()
    {
        return current > 0;
    }

    public void SetNewMaxAmmo(int newMax)
    {
        max = newMax;
        current = max;
    }
}