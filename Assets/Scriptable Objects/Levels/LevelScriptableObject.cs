using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "level", menuName = "Scriptable Objects/Level")]
public class LevelScriptableObject : ScriptableObject
{
    public EnemyScriptableObject EnemyToSpawn;
    public int NumberOfSpawns;
}