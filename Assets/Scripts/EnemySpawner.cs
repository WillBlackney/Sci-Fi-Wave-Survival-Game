using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    [Header("Enemy Encounter Lists")]
    public List<EnemyWaveSO> basicEnemyWaves;
    public List<EnemyWaveSO> eliteEnemyWaves;
    public List<EnemyWaveSO> bossEnemyWaves;    
    public List<TileScript> spawnLocations;

    public List<TileScript> waveSpawnCentrePoints;
    
    
    public EnemyWaveSO GetRandomWaveSO(List<EnemyWaveSO> enemyWaves)
    {
        int randomIndex = Random.Range(0, enemyWaves.Count);
        return enemyWaves[randomIndex];
    }
    public void SpawnEnemyWave(string enemyType = "Basic")
    {
        TileScript spawnCentrePoint = GetRandomEnemyWaveCentrePoint();
        List<TileScript> possibleSpawnLocations = GetValidSpawnLocationsWithinRangeOfCentrePoint(spawnCentrePoint, 5);
        InstantiateEnemiesFromWave(GetRandomWaveSO(basicEnemyWaves), possibleSpawnLocations);
               
    }    

    // Spawn location related
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

}
