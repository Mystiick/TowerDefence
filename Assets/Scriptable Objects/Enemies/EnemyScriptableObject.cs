using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "enemy", menuName = "Scriptable Objects/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    public string EnemyName;    
    public float Speed = 1f;
    public int GoldValue = 10;
    public int Health = 100;
    public GameObject PrefabToRender;
    public Vector3 Scale;

}
