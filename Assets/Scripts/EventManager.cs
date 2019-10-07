﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    public TreasureChest activeTreasureChest;
    public WorldEncounter.EncounterType currentEncounterType;

    public bool gameOverEventStarted;

    private void Start()
    {
        StartNewGameSequence();
    }

    public void StartNewReloadGameSceneEvent()
    {
        StartCoroutine(StartNewReloadGameSceneEventCoroutine());
    }
    public IEnumerator StartNewReloadGameSceneEventCoroutine()
    {
        // Start screen fade out
        Action action = BlackScreenManager.Instance.FadeOut(1);
        // wait until screen is completely faded out
        yield return new WaitUntil(() => action.ActionResolved() == true);
        // Reload game scene
        SceneLoader.Instance.LoadGameScene();
    }

    public void StartNewGameOverEvent()
    {
        StartCoroutine(StartNewGameOverEventCoroutine());
    }
    public IEnumerator StartNewGameOverEventCoroutine()
    {
        gameOverEventStarted = true;
        // Stop all coroutines running on enemies
        foreach(Enemy enemy in EnemyManager.Instance.allEnemies)
        {
            enemy.StopAllCoroutines();
        }

        CameraManager.Instance.SetCameraLookAtTarget(LevelManager.Instance.GetWorldCentreTile().gameObject);
        yield return new WaitForSeconds(3f);
        UIManager.Instance.SetGameOverCanvasVisibility(true);
    }
    public void StartNewGameSequence()
    {
        StartCoroutine(StartNewGameSequenceCoroutine());
    }
    public IEnumerator StartNewGameSequenceCoroutine()
    {
        
        // Build a new world
        Action levelCreation = LevelManager.Instance.CreateLevel();
        yield return new WaitUntil(() => levelCreation.ActionResolved() == true);
        // Set up control zone
        LevelManager.Instance.CreateSpaceShipControlZone();
        // Populate Spawn Locations around the map
        EnemySpawner.Instance.PopulateEnemyWaveCentrePoints();
        // Centre the camera in the middle of the world
        //CameraManager.Instance.LookAtTarget(LevelManager.Instance.GetWorldCentreTile().gameObject);
        CameraManager.Instance.SetCameraLookAtTarget(LevelManager.Instance.GetWorldCentreTile().gameObject);
        yield return new WaitUntil(() => CameraManager.Instance.IsCameraWithinRangeOfTarget(LevelManager.Instance.GetWorldCentreTile().gameObject) == true);
        // Fade in scene
        Action fadeIn = BlackScreenManager.Instance.FadeIn(1);
        // wait until the fade in effect is finished
        yield return new WaitUntil(() => fadeIn.ActionResolved() == true);
        //yield return new WaitForSeconds(1.5f);
        // Create Space ship at world centre
        DefenderManager.Instance.CreateSpaceShip();
        yield return new WaitForSeconds(1f);
        // Create Starting Defenders
        DefenderManager.Instance.CreateStartingDefenders();
        yield return new WaitForSeconds(1f);        
        // Instantiate enemies
        Action waveSpawn = EnemySpawner.Instance.SpawnNextWave();
        yield return new WaitUntil(() => waveSpawn.ActionResolved() == true);
        // Move camera back to space ship
        CameraManager.Instance.SetCameraLookAtTarget(LevelManager.Instance.GetWorldCentreTile().gameObject);
        yield return new WaitUntil(() => CameraManager.Instance.IsCameraWithinRangeOfTarget(LevelManager.Instance.GetWorldCentreTile().gameObject) == true);
        // Start the players first turn
        StartCoroutine(TurnManager.Instance.StartPlayerTurn());        
        
    }
    public void StartNewBasicEncounterEvent()
    {
        // Disable player's ability to click on encounter buttons and start new encounters
        WorldMap.Instance.canSelectNewEncounter = false;
        // turn off hexagon highlights
        WorldMap.Instance.UnHighlightAllHexagons();
        // Destroy the previous level and tiles + reset values/properties
        ClearPreviousEncounter();        
        // Create a new level
        LevelManager.Instance.CreateLevel();
        //EnemySpawner.Instance.PopulateEnemySpawnLocations();        
        CharacterRoster.Instance.InstantiateDefenders();
        // DefenderManager.Instance.PlaceDefendersAtStartingLocations();
        // Instantiate enemies
        //EnemySpawner.Instance.SpawnEnemyWave();
        // reset turn manager properties
        TurnManager.Instance.ResetTurnManagerPropertiesOnCombatStart();
        // start player turn 1
        StartCoroutine(TurnManager.Instance.StartPlayerTurn());
        // disable world map view
        UIManager.Instance.DisableWorldMapView();
        currentEncounterType = WorldEncounter.EncounterType.BasicEnemy;
    }
    public void StartNewEliteEncounterEvent()
    {
        // Disable player's ability to click on encounter buttons and start new encounters
        WorldMap.Instance.canSelectNewEncounter = false;
        // turn off hexagon highlights
        WorldMap.Instance.UnHighlightAllHexagons();
        // Destroy the previous level and tiles + reset values/properties
        ClearPreviousEncounter();
        // Create a new level
        LevelManager.Instance.CreateLevel();
        //EnemySpawner.Instance.PopulateEnemySpawnLocations();
        CharacterRoster.Instance.InstantiateDefenders();
        //DefenderManager.Instance.PlaceDefendersAtStartingLocations();
        // Instantiate enemies
        //EnemySpawner.Instance.SpawnEnemyWave("Elite");
        // reset turn manager properties
        TurnManager.Instance.ResetTurnManagerPropertiesOnCombatStart();
        // start player turn 1
        StartCoroutine(TurnManager.Instance.StartPlayerTurn());
        // disable world map view
        UIManager.Instance.DisableWorldMapView();
        currentEncounterType = WorldEncounter.EncounterType.EliteEnemy;
    }
    public void StartNewRestSiteEncounterEvent()
    {
        // Disable player's ability to click on encounter buttons and start new encounters
        WorldMap.Instance.canSelectNewEncounter = false;
        // turn off hexagon highlights
        WorldMap.Instance.UnHighlightAllHexagons();
        // Destroy the previous level and tiles + reset values/properties
        ClearPreviousEncounter();
        CampSiteManager.Instance.EnableCampSiteView();
        // disable world map/char roster/inventory view to avoid clutter
        UIManager.Instance.DisableCharacterRosterView();
        UIManager.Instance.DisableInventoryView();
        UIManager.Instance.DisableWorldMapView();
        CampSiteManager.Instance.ResetEventProperties();
    }
    public void StartShopEncounterEvent()
    {
        // Disable player's ability to click on encounter buttons and start new encounters
        WorldMap.Instance.canSelectNewEncounter = true;
        // turn off hexagon highlights
        WorldMap.Instance.UnHighlightAllHexagons();
        // Destroy the previous level and tiles + reset values/properties
        ClearPreviousEncounter();
        ShopScreenManager.Instance.EnableShopScreenView();
        ShopScreenManager.Instance.LoadShopScreenEntities();
        // disable world map/char roster/inventory view to avoid clutter
        UIManager.Instance.DisableCharacterRosterView();
        UIManager.Instance.DisableInventoryView();
        UIManager.Instance.DisableWorldMapView();
        //CampSiteManager.Instance.ResetEventProperties();
    }
    public void StartNewTreasureRoomEncounterEvent()
    {
        // Disable player's ability to click on encounter buttons and start new encounters
        WorldMap.Instance.canSelectNewEncounter = true;
        // turn off hexagon highlights
        WorldMap.Instance.UnHighlightAllHexagons();
        // Destroy the previous level and tiles + reset values/properties
        ClearPreviousEncounter();
        // Create a new level
        LevelManager.Instance.CreateLevel();
        //EnemySpawner.Instance.PopulateEnemySpawnLocations();        
        CharacterRoster.Instance.InstantiateDefenders();        
        // disable world map view
        UIManager.Instance.DisableWorldMapView();
        // Instantiate treasure chest and populate it with loot
        CreateNewTreasureChest();
    }
    public void StartNewEndBasicEncounterEvent()
    {
        StartCoroutine(StartNewEndBasicEncounterEventCoroutine());        
    }
    public IEnumerator StartNewEndBasicEncounterEventCoroutine()
    {
        Debug.Log("StartNewEndBasicEncounterEvent() coroutine started...");
        // Show combat end visual events before loot reward screen appears
        preLootScreenEventFinished = false;
        StartCoroutine(StartPreLootScreenVisualEvent(20));
        //yield return new WaitForSeconds(3f);
        yield return new WaitUntil(() => PreLootScreenEventFinished() == true);
        // Give characters xp
        CharacterRoster.Instance.RewardAllCharactersXP(20);
        // Unselect defender to hide ability bar UI, prevent null behaviors
        DefenderManager.Instance.ClearSelectedDefender();
        // Hide ability info panel
        SpellInfoBox.Instance.HideInfoBox();
        // re enable world map + get next viable enocunter hexagon tiles
        WorldMap.Instance.SetWorldMapReadyState();
        // Start loot creation/display process
        StartNewLootRewardEvent();
        yield return null;
    }

    public IEnumerator StartPreLootScreenVisualEvent(int xpReward)
    {
        Debug.Log("StartPreLootScreenVisualEvent() coroutine started...");
        preLootScreenEventFinished = false;
        // THIS COROUTINE DOES NOT ACTUALLY MODIFY XP/LEVEL!!!! only displays visual info

        // short yield for seconds to smoothen the transistion
        yield return new WaitForSeconds(1f);


        foreach(CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
        {
            Debug.Log("StartPreLootScreenVisualEvent() creating visual status xp gained effect...");
            StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(character.myDefenderGO.transform.position, "XP + " + xpReward.ToString(), true));
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1f);

        foreach (CharacterData character in CharacterRoster.Instance.allCharacterDataObjects)
        {
            if(character.currentXP + xpReward >= character.currentMaxXP)
            {
                Debug.Log("StartPreLootScreenVisualEvent() creating visual status LEVEL GAINED! effect...");
                StartCoroutine(VisualEffectManager.Instance.CreateStatusEffect(character.myDefenderGO.transform.position, "LEVEL UP!" + xpReward.ToString(), true));
                yield return new WaitForSeconds(0.5f);

            }            
        }
        yield return new WaitForSeconds(1f);

        preLootScreenEventFinished = true;
    }

    public bool preLootScreenEventFinished;
    public bool PreLootScreenEventFinished()
    {
        if(preLootScreenEventFinished == true)
        {
            Debug.Log("PreLootScreenEventFinished() bool returning true...");
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator StartNewEndEliteEncounterEvent()
    {
        // Show combat end visual events before loot reward screen appears
        StartCoroutine(StartPreLootScreenVisualEvent(50));
        yield return new WaitUntil(() => PreLootScreenEventFinished() == true);
        // Give characters xp
        CharacterRoster.Instance.RewardAllCharactersXP(50);
        // Unselect defender to hide ability bar UI, prevent null behaviors
        DefenderManager.Instance.ClearSelectedDefender();
        // Hide ability info panel
        SpellInfoBox.Instance.HideInfoBox();
        // re enable world map + get next viable enocunter hexagon tiles
        WorldMap.Instance.SetWorldMapReadyState();
        // Start loot creation/display process
        StartNewLootRewardEvent(false);
    }

    public void StartNewMysteryEncounterEvent()
    {
        int randomNumber = Random.Range(1, 101);

        if(randomNumber >= 1 && randomNumber <= 50)
        {
            StartNewBasicEncounterEvent();
        }
        else if (randomNumber >= 51 && randomNumber <= 70)
        {
            StartNewTreasureRoomEncounterEvent();
        }
        else if (randomNumber >= 71 && randomNumber <= 90)
        {
            StartShopEncounterEvent();
        }
        else if (randomNumber >= 91 && randomNumber <= 100)
        {
            StartNewEliteEncounterEvent();
        }
    }

    public void ClearPreviousEncounter()
    {
        // Destroy the level data + all tile Go's
        LevelManager.Instance.DestroyCurrentLevel();
        // Destroy defender GO's from previous encounter
        DefenderManager.Instance.DestroyAllDefenders();
        // Hide rest site view screen if open
        CampSiteManager.Instance.DisableCampSiteView();
        // Hide Shop view screen if open
        ShopScreenManager.Instance.DisableShopScreenView();
        // Destroy active treasure chest if it exists
        if(activeTreasureChest != null)
        {
            activeTreasureChest.DestroyChest();
        }
    }

    public void StartNewLootRewardEvent(bool basicEnemy = true)
    {
        Debug.Log("StartNewLootRewardEvent() called...");

        if (basicEnemy)
        {
            UIManager.Instance.EnableRewardScreenView();
            RewardScreen.Instance.CreateGoldRewardButton();
            RewardScreen.Instance.CreateItemRewardButton();            
            RewardScreen.Instance.PopulateItemScreen();
        }
        else
        {
            UIManager.Instance.EnableRewardScreenView();
            RewardScreen.Instance.CreateGoldRewardButton();
            RewardScreen.Instance.CreateItemRewardButton();
            RewardScreen.Instance.CreateArtifactRewardButton();
            RewardScreen.Instance.PopulateItemScreen();
        }
        
    }

    public void CreateNewTreasureChest()
    {
        // create a treasure chest game object
       // GameObject newTreasureChest = Instantiate(PrefabHolder.Instance.TreasureChest);
        //newTreasureChest.GetComponent<TreasureChest>().InitializeSetup();
        RewardScreen.Instance.CreateArtifactRewardButton();
    }

    public void EndNewLootRewardEvent()
    {
        RewardScreen.Instance.ClearRewards();
        UIManager.Instance.DisableRewardScreenView();
        UIManager.Instance.EnableWorldMapView();
        WorldMap.Instance.HighlightNextAvailableEncounters();
    }

    


}
