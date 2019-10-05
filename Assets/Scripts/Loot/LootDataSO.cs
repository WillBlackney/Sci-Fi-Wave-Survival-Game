using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LootDataSO", menuName = "LootDataSO", order = 55)]
public class LootDataSO : ScriptableObject
{
    public Sprite lootImage;
    public string lootName;
    public string lootDescription;
}
