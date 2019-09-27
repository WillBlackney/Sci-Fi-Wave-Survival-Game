using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLogic : Singleton<WeaponLogic>
{
    public void AssignWeaponToEntity(LivingEntity entity, WeaponDataSO weaponData)
    {
        Debug.Log("Assigning weapon '" + weaponData.weaponName + "' to " + entity.name);

        if(weaponData.weaponType == WeaponDataSO.WeaponType.Ranged)
        {
            entity.myRangedWeapon = weaponData;
            if (entity.GetComponent<Defender>())
            {
                if (entity.GetComponent<Defender>().myRangedWeaponSlot != null)
                {
                    entity.GetComponent<Defender>().myRangedWeaponSlot.SetWeapon(weaponData);
                }
                
            }   
        }

        if (weaponData.weaponType == WeaponDataSO.WeaponType.Melee)
        {
            entity.myMeleeWeapon = weaponData;
            if (entity.GetComponent<Defender>())
            {
                if (entity.GetComponent<Defender>().myMeleeWeaponSlot != null)
                {
                    entity.GetComponent<Defender>().myMeleeWeaponSlot.SetWeapon(weaponData);
                }
            }
        }

    }

    public void RunDefenderStartingWeaponSetup(LivingEntity entity)
    {
        Debug.Log("WeaponLogic.RunDefenderWeaponSetup() called...");
        if(entity.myClass == LivingEntity.Class.Rifleman)
        {
            AssignWeaponToEntity(entity, WeaponLibrary.Instance.GetWeaponByName("Assault Rifle"));
        }
        else if (entity.myClass == LivingEntity.Class.Ranger)
        {
            AssignWeaponToEntity(entity, WeaponLibrary.Instance.GetWeaponByName("Shotgun"));
            AssignWeaponToEntity(entity, WeaponLibrary.Instance.GetWeaponByName("Machete"));
        }
        else if (entity.myClass == LivingEntity.Class.MachineGunner)
        {
            AssignWeaponToEntity(entity, WeaponLibrary.Instance.GetWeaponByName("Machine Gun"));            
        }
        else if (entity.myClass == LivingEntity.Class.Marksman)
        {
            AssignWeaponToEntity(entity, WeaponLibrary.Instance.GetWeaponByName("Sniper Rifle"));
        }
    }

    public int CalculateRandomWeaponDamageValue(WeaponDataSO weaponData)
    {
        Debug.Log("WeaponLogic.CalculateRandomWeaponDamageValue() called...");
        int randomAttackDamage = Random.Range(weaponData.weaponMinDamage, weaponData.weaponMaxDamage + 1);
        Debug.Log("Random damage from " + weaponData.weaponName + " between min/max damage values " + weaponData.weaponMinDamage + "/" + weaponData.weaponMaxDamage + " is " + randomAttackDamage.ToString());
        return randomAttackDamage;
    }
}
