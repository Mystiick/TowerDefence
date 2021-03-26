using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "level", menuName = "Scriptable Objects/Level")]
public class LevelScriptableObject : ScriptableObject
{
    public float SpawnSpeed;
    public Wave[] Waves;
}

[System.Serializable]
public struct Wave
{
    public EnemyScriptableObject EnemyToSpawn;
    public int NumberOfSpawns;
    public float HealthModifier;
}
