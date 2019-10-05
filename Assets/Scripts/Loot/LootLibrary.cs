using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootLibrary : Singleton<LootLibrary>
{
    public List<LootDataSO> allLootRewards;
    public LootDataSO GetRandomLootData()
    {
        int randomIndex = Random.Range(0, allLootRewards.Count);
        return allLootRewards[randomIndex];
    }
    public LootDataSO GetLootRewardByName(string name)
    {
        LootDataSO lootReturned = null;

        foreach(LootDataSO loot in allLootRewards)
        {
            if(loot.lootName == name)
            {
                lootReturned = loot;
                break;
            }
        }

        return lootReturned;
    }
}
