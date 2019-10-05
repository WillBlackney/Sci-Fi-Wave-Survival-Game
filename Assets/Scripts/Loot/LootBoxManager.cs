using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBoxManager : Singleton<LootBoxManager>
{
    [Header("Component References")]
    public GameObject lootRewardScreenVisualParent;
    public LootCard lootCardOne;
    public LootCard lootCardTwo;
    public LootCard lootCardThree;

    [Header("Properties")]
    public List<LootBox> activeLootBoxes;
    public List<int> lootSpawnTurns;

    // Instantiation + Loot box related
    #region
    public Action StartNewLootBoxCreatedEvent()
    {
        Action action = new Action();
        StartCoroutine(StartNewLootBoxCreatedEventCoroutine(action));
        return action;

    }
    public IEnumerator StartNewLootBoxCreatedEventCoroutine(Action action)
    {
        // Determine spawn location for new Loot Box
        TileScript spawnLocation = GetRandomValidSpawnLocation();

        // Move camera to look at location
        CameraManager.Instance.SetCameraLookAtTarget(spawnLocation.gameObject);
        yield return new WaitForSeconds(2f);

        // Create Portal VFX object
        GameObject portalVFX = Instantiate(PrefabHolder.Instance.PortalPrefab);
        portalVFX.transform.position = spawnLocation.WorldPosition;
        yield return new WaitForSeconds(0.1f);

        // Instantiate Loot Box GO
        CreateLootBox(spawnLocation);

        action.actionResolved = true;
    }
    public void CreateLootBox(TileScript spawnLocation)
    {
        GameObject newLootBox = Instantiate(PrefabHolder.Instance.LootBox);
        activeLootBoxes.Add(newLootBox.GetComponent<LootBox>());
        PlaceLootBoxOnTile(newLootBox.GetComponent<LootBox>(), spawnLocation);
        newLootBox.GetComponent<LootBox>().ModifyCountDown(3);
    }
    public void PlaceLootBoxOnTile(LootBox lootBox, TileScript location)
    {
        lootBox.transform.position = location.WorldPosition;
        lootBox.myTile = location;
        location.SetTileAsOccupiedByObject(lootBox);
    }
    public void RemoveLootBoxFromTile(LootBox lootBox)
    {
        TileScript location = lootBox.myTile;       
        location.SetTileAsUnoccupiedByObject();
    }
    public void DestroyLootBox(LootBox lootBox)
    {
        activeLootBoxes.Remove(lootBox.GetComponent<LootBox>());
        RemoveLootBoxFromTile(lootBox);
        Destroy(lootBox.gameObject);
    }
    public Action ReduceAllLootBoxCountdowns()
    {
        Action action = new Action();
        StartCoroutine(ReduceAllLootBoxCountdownsCoroutine(action));
        return action;
    }
    public IEnumerator ReduceAllLootBoxCountdownsCoroutine(Action action)
    {
        List<LootBox> lootBoxesDestroyed = new List<LootBox>();

        foreach(LootBox lootBox in activeLootBoxes)
        {
            lootBox.ModifyCountDown(-1);
            if(lootBox.countDown == 0)
            {
                lootBoxesDestroyed.Add(lootBox);
                /*
                CameraManager.Instance.SetCameraLookAtTarget(lootBox.gameObject);
                yield return new WaitForSeconds(2f);
                DestroyLootBox(lootBox);
                */
            }
        }

        foreach(LootBox lootB in lootBoxesDestroyed)
        {
            CameraManager.Instance.SetCameraLookAtTarget(lootB.gameObject);
            yield return new WaitForSeconds(3f);
            DestroyLootBox(lootB);
        }

        action.actionResolved = true;
    }
    #endregion

    // Loot screen related
    #region
    public Action StartNewLootScreenEvent()
    {
        Action action = new Action();
        StartCoroutine(StartNewLootScreenEventCoroutine());
        return action;
    }
    public IEnumerator StartNewLootScreenEventCoroutine()
    {
        PopulateLootRewardScreen();
        Action fadeIn = FadeInLootScreen();
        yield return new WaitUntil(() => fadeIn.ActionResolved() == true);
    }
    public void PopulateLootRewardScreen()
    {
        List<LootDataSO> lootRewards = GetThreeRandomDifferentLootRewards();
        lootCardOne.RunSetupFromLootDataSO(lootRewards[0]);
        lootCardTwo.RunSetupFromLootDataSO(lootRewards[1]);
        lootCardThree.RunSetupFromLootDataSO(lootRewards[2]);
    }
    public Action FadeInLootScreen()
    {
        Action action = new Action();
        StartCoroutine(FadeInLootScreenCoroutine(action));
        return action;

    }
    public IEnumerator FadeInLootScreenCoroutine(Action action)
    {
        CanvasGroup cg = lootRewardScreenVisualParent.GetComponent<CanvasGroup>();
        lootRewardScreenVisualParent.SetActive(true);
        cg.alpha = 0;

        while (cg.alpha < 1)
        {
            cg.alpha += 0.2f;
            yield return new WaitForEndOfFrame();
        }

        action.actionResolved = true;
    }
    public Action FadeOutLootScreen()
    {
        Action action = new Action();
        StartCoroutine(FadeOutLootScreenCoroutine(action));
        return action;

    }
    public IEnumerator FadeOutLootScreenCoroutine(Action action)
    {
        CanvasGroup cg = lootRewardScreenVisualParent.GetComponent<CanvasGroup>();
        lootRewardScreenVisualParent.SetActive(true);
        cg.alpha = 1;

        while (cg.alpha > 0)
        {
            cg.alpha -= 0.2f;
            yield return new WaitForEndOfFrame();
        }

        lootRewardScreenVisualParent.SetActive(false);
        action.actionResolved = true;
    }
    public List<LootDataSO> GetThreeRandomDifferentLootRewards()
    {
        List<LootDataSO> lootRewardsReturned = new List<LootDataSO>();
        LootDataSO one = null;
        LootDataSO two = null;
        LootDataSO three = null;

        one = LootLibrary.Instance.GetRandomLootData();
        while (two == null || two == one)
        {
            two = LootLibrary.Instance.GetRandomLootData();
        }
        while (three == null || three == two || three == one)
        {
            three = LootLibrary.Instance.GetRandomLootData();
        }

        lootRewardsReturned.Add(one);
        lootRewardsReturned.Add(two);
        lootRewardsReturned.Add(three);
        return lootRewardsReturned;
    }
    public void RewardLootFromLootCard(LootDataSO lootData)
    {      
        // Gold Supplies
        if(lootData.lootName == "Gold Supplies")
        {
            PlayerDataManager.Instance.ModifyGold(5);
        }

        // Medical Supplies
        else if (lootData.lootName == "Medical Supplies")
        {
            foreach(Defender defender in DefenderManager.Instance.allDefenders)
            {
                defender.ModifyCurrentHealth(5);
            }
        }

        // Food Supplies
        else if (lootData.lootName == "Food Supplies")
        {
            PlayerDataManager.Instance.ModifyCurrentMaxTroopCount(2);
        }
    }
    #endregion

    // Get and return data
    #region
    public TileScript GetRandomValidSpawnLocation()
    {
        List<TileScript> validLocations = new List<TileScript>();

        foreach(TileScript tile in LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary())
        {
            // only select valid tiles that are more then 10 tiles away from the space ship
            if(tile.CanBeMovedThrough() &&
               tile.CanBeOccupied() &&
               !LevelManager.Instance.GetTilesWithinRange(10, LevelManager.Instance.GetWorldCentreTile()).Contains(tile))
            {
                validLocations.Add(tile);
            } 
        }

        int randomIndex = Random.Range(0, validLocations.Count);
        return validLocations[randomIndex];
    }
    #endregion
    
    
}
