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
    public void InstantiateEnemiesFromWave(EnemyWaveSO enemyWave, List<TileScript> spawnLocations)
    {
        foreach (EnemyGroup enemyGroup in enemyWave.enemyGroups)
        {
            int randomIndex = Random.Range(0, enemyGroup.enemyList.Count);

            GameObject newEnemyGO = Instantiate(enemyGroup.enemyList[randomIndex]);

            Enemy newEnemy = newEnemyGO.GetComponent<Enemy>();
            // Choose a random tile from the list of spawnable locations
            TileScript spawnLocation = LevelManager.Instance.GetRandomValidMoveableTileFromList(spawnLocations);
            // Run the enemy's constructor
            newEnemy.InitializeSetup(spawnLocation.GridPosition, spawnLocation);
        }
    }
    public void SpawnEnemyWave(EnemyWaveSO enemyWave)
    {
        TileScript spawnCentrePoint = GetRandomEnemyWaveCentrePoint();
        List<TileScript> possibleSpawnLocations = GetValidSpawnLocationsWithinRangeOfCentrePoint(spawnCentrePoint, 5);
        InstantiateEnemiesFromWave(enemyWave, possibleSpawnLocations);

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
    public void SpawnNextWave()
    {
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
            waveType = EnemyWaveSO.WaveType.Elite;
        }
        else
        {
            waveType = EnemyWaveSO.WaveType.Basic;
        }

        // stop spawning if this not a correct turn for spawning
        if(level == 0)
        {
            return;
        }

        SpawnEnemyWave(GetRandomWave(waveType, level));

    }
    #endregion

}
