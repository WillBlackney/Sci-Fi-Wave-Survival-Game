using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    [Header("Enemy Wave SO's")]
    public List<EnemyWaveSO> allWaves;

    [Header("Properties")]
    public List<TileScript> waveSpawnCentrePoints;

    // Spawn location related
    #region
    public void PopulateEnemyWaveCentrePoints()
    {
        waveSpawnCentrePoints.AddRange(LevelManager.Instance.GetTilesOnMapEdges());
    }
    public TileScript GetRandomEnemyWaveCentrePoint()
    {
        int randomIndex = Random.Range(0, waveSpawnCentrePoints.Count);
        return waveSpawnCentrePoints[randomIndex];
    }
    public List<TileScript> GetValidSpawnLocationsWithinRangeOfCentrePoint(TileScript centrePoint, int range)
    {
        List<TileScript> validSpawnLocations = LevelManager.Instance.GetValidMoveableTilesWithinRange(range, centrePoint);
        return validSpawnLocations;
    }
    #endregion

    // Spawning + Instantiation
    #region
    public IEnumerator InstantiateEnemiesFromWave(EnemyWaveSO enemyWave, List<TileScript> spawnLocations)
    {
        foreach (EnemyGroup enemyGroup in enemyWave.enemyGroups)
        {
            int randomIndex = Random.Range(0, enemyGroup.enemyList.Count);

            // Choose a random tile from the list of spawnable locations
            TileScript spawnLocation = LevelManager.Instance.GetRandomValidMoveableTileFromList(spawnLocations);

            // Create Portal VFX object
            GameObject portalVFX = Instantiate(PrefabHolder.Instance.PortalPrefab);
            portalVFX.transform.position = spawnLocation.WorldPosition;
            yield return new WaitForSeconds(0.1f);

            // Instantiate enemy GO, get script
            GameObject newEnemyGO = Instantiate(enemyGroup.enemyList[randomIndex]);
            Enemy newEnemy = newEnemyGO.GetComponent<Enemy>();
            
            // Run the enemy's constructor
            newEnemy.InitializeSetup(spawnLocation.GridPosition, spawnLocation);
            yield return new WaitForSeconds(0.5f);
        }
    }
    public IEnumerator SpawnEnemyWave(EnemyWaveSO enemyWave, Action action)
    {
        TileScript spawnCentrePoint = GetRandomEnemyWaveCentrePoint();
        CameraManager.Instance.SetCameraLookAtTarget(spawnCentrePoint.gameObject);
        yield return new WaitForSeconds(2f);
        List<TileScript> possibleSpawnLocations = GetValidSpawnLocationsWithinRangeOfCentrePoint(spawnCentrePoint, 3);
        StartCoroutine(InstantiateEnemiesFromWave(enemyWave, possibleSpawnLocations));
        yield return new WaitForSeconds(3f);
        action.actionResolved = true;

    }
    public EnemyWaveSO GetRandomWave(EnemyWaveSO.WaveType waveType, int level)
    {
        List<EnemyWaveSO> possibleWaves = new List<EnemyWaveSO>();

        foreach (EnemyWaveSO wave in allWaves)
        {
            if (wave.waveType == waveType && wave.level == level)
            {
                possibleWaves.Add(wave);
            }
        }

        int randomIndex = Random.Range(0, possibleWaves.Count);
        return possibleWaves[randomIndex];

    }
    public Action SpawnNextWave()
    {
        Action waveSpawn = new Action();
        List<int> levelOneTurns = new List<int> { 0, 3, 6 };
        List<int> levelTwoTurns = new List<int> { 9, 12, 15 };
        List<int> eliteTurns = new List<int> { 6, 15, 24, 33 };

        int level = 0;
        EnemyWaveSO.WaveType waveType;

        // Set difficulty level based on player current turn count
        if (levelOneTurns.Contains(TurnManager.Instance.playerTurnCount))
        {
            level = 1;
        }
        else if (levelTwoTurns.Contains(TurnManager.Instance.playerTurnCount))
        {
            level = 2;
        }

        // Set wave type based on player current turn count
        if (eliteTurns.Contains(TurnManager.Instance.playerTurnCount))
        {
            // uncomment when elite waves/enemies have been implemented
            //waveType = EnemyWaveSO.WaveType.Elite;
            waveType = EnemyWaveSO.WaveType.Basic;
        }
        else
        {
            waveType = EnemyWaveSO.WaveType.Basic;
        }

        // stop spawning if this not a correct turn for spawning
        if (level != 0)
        {
            StartCoroutine(SpawnEnemyWave(GetRandomWave(waveType, level), waveSpawn));
        }
        else
        {
            waveSpawn.actionResolved = true;
        }
        
        return waveSpawn;
    }
   
    #endregion

}
