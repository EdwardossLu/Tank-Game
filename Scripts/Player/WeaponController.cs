using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private WeaponSO[] availableWeapons = null;
    
    private int selectedWeapon = 0;
    
    private float nextFireTime = 0f;

    public int ID => selectedWeapon;
    public AmmoData ammo => data[selectedWeapon];
    [ReadOnly] public List<AmmoData> data = new List<AmmoData>();
    
    public Action<WeaponSO, AmmoData> onWeaponChanged = null;
    
    private void Awake()
    {
        for (int index = 0; index < availableWeapons.Length; index++)
        {
            WeaponSO item = availableWeapons[index];
            data.Add(new AmmoData(item, index));
        }
    }

    private void Start()
    {
        OnChanged();
    }

    private void Update()
    {
        if (!(Time.time >= nextFireTime)) return;
        
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            Forward();
            nextFireTime = Time.time + fireRate;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            Backward();
            nextFireTime = Time.time + fireRate;
        }
    }

    public Projectile GetAmmo()
    {
        return WeaponManager.Instance.Ammo(availableWeapons[selectedWeapon]);
    }
    
    private void Forward()
    {
        selectedWeapon++;
        if (selectedWeapon >= availableWeapons.Length)
            selectedWeapon = 0;
        
        OnChanged();
    }

    private void Backward()
    {
        selectedWeapon--;
        if (selectedWeapon < 0)
            selectedWeapon = availableWeapons.Length - 1;

        OnChanged();
    }

    private void OnChanged()
    {
        onWeaponChanged?.Invoke(availableWeapons[selectedWeapon], data[selectedWeapon]);
    }
}
