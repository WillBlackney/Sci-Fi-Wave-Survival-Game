using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLibrary : Singleton<WeaponLibrary>
{
    public List<WeaponDataSO> allWeapons;

    public WeaponDataSO GetWeaponByName(string name)
    {
        WeaponDataSO weaponReturned = null;

        foreach(WeaponDataSO weapon in allWeapons)
        {
            if(name == weapon.weaponName)
            {
                weaponReturned = weapon;
            }
        }

        if(weaponReturned == null)
        {
            Debug.Log("WeaponLibrary.GetWeaponByName() failed to find a weapon with the name: " + name + ", returning null....");
        }

        return weaponReturned;
            
    }

}
