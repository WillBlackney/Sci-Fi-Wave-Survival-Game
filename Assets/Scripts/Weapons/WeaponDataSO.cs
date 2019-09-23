using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New WeaponDataO", menuName = "WeaponDataSO", order = 54)]
public class WeaponDataSO : ScriptableObject
{
    public enum WeaponType { Ranged, Melee };

    public Sprite weaponImage;
    public WeaponType weaponType;
    public string weaponName;    
    public int weaponRange;
    public int weaponMinDamage;
    public int weaponMaxDamage;  

    
}
