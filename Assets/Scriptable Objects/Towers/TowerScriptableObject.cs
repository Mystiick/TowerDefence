using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "tower", menuName = "Scriptable Objects/Tower")]
public class TowerScriptableObject : ScriptableObject
{
    public string TowerName;
    public int Width = 2;
    public int Height = 2;
    public float Range = 10f;
    public int GoldCost = 1;
    public GameObject Projectile;
    public GameObject PrefabToRender;
    public int Damage;
    public bool HasCollider = true;
    public Vector3 Scale;
    /// <summary>Time in seconds for how long this tower takes to be built/upgraded to</summary>
    public float ConstructionTime;
    public TowerScriptableObject[] Upgrades;
}
