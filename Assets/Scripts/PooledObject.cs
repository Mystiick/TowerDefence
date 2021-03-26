using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component type that contains the parent prefab. Used to create/release objects from the pool
/// </summary>
public class PooledObject : MonoBehaviour
{
    public GameObject Prefab;
}
