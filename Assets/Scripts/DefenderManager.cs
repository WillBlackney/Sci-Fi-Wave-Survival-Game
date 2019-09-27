using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderManager : Singleton<DefenderManager>
{
    //public GameObject spaceShipPrefab;
    public List<Defender> allDefenders = new List<Defender>();
    public List<TileScript> spawnLocations = new List<TileScript>();
    public GameObject defendersParent;

    public Defender selectedDefender;    

    public void SetSelectedDefender(Defender defender)
    {
        // if we click on a different defender while the selectedDefender is awaiting an order, return
        if(selectedDefender != null &&
           (
           selectedDefender.awaitingMoveOrder == true ||
           selectedDefender.awaitingShootOrder == true 
           //selectedDefender.awaitingThrowGrenadeOrder
           )
           )
        {
            return;
        }

        // if we have already have a defender selected when we click on another defender, unselect that defender, then select the new defender
        if(selectedDefender != defender && selectedDefender != null)
        {
            Debug.Log("Clearing selected defender");
            selectedDefender.UnselectDefender();
            LevelManager.Instance.UnhighlightAllTiles();
        }
        selectedDefender = defender;
        CameraManager.Instance.SetCameraLookAtTarget(selectedDefender.gameObject);
        Debug.Log("Selected defender: " + selectedDefender.gameObject.name);
    }

    public void ClearSelectedDefender()
    {
        if(selectedDefender != null)
        {
            selectedDefender.UnselectDefender();
            selectedDefender = null;
        }
        CameraManager.Instance.ClearCameraLookAtTarget();
        LevelManager.Instance.UnhighlightAllTiles();
    }    

    public void DestroyAllDefenders()
    {
        List<Defender> allDefs = new List<Defender>();
        allDefs.AddRange(allDefenders);

        foreach(Defender defender in allDefenders)
        {
            LivingEntityManager.Instance.allLivingEntities.Remove(defender);
        }       

        foreach(Defender defender in allDefs)
        {
            if (allDefenders.Contains(defender))
            {
                allDefenders.Remove(defender);
                Destroy(defender.gameObject);
            }
        }

        allDefenders.Clear();
    }

    public void CreateSpaceShip()
    {
        TileScript spawnLocation = LevelManager.Instance.GetWorldCentreTile();

        GameObject spaceShip = Instantiate(PrefabHolder.Instance.spaceShipPrefab, defendersParent.transform);
        spaceShip.GetComponent<Defender>().InitializeSetup(spawnLocation.GridPosition, spawnLocation);
    }

    public void CreateStartingDefenders()
    {
        List<TileScript> spawnLocations = LevelManager.Instance.GetDefenderSpawnLocations();

        foreach(TileScript tile in spawnLocations)
        {
            GameObject newRifleman = Instantiate(PrefabHolder.Instance.riflemanPrefab, defendersParent.transform);
            newRifleman.GetComponent<Defender>().InitializeSetup(tile.GridPosition, tile);
        }
    }

   


    
}
