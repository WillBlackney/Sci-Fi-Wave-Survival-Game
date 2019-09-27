using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyWaveSO", menuName = "EnemyWaveSO", order = 51)]
public class EnemyWaveSO : ScriptableObject
{
    public enum WaveType { None, Basic, Elite, Boss}
    public int level;
    public WaveType waveType;
    public List<EnemyGroup> enemyGroups;
}

[System.Serializable]
public class EnemyGroup
{
    public List<GameObject> enemyList;
}
