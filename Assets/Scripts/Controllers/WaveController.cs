using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;

public class WaveController : MonoBehaviour
{

    public GameObject Spawner;
    public GameObject Target;
    public int CurrentLevel = 0;
    public LevelScriptableObject[] Levels;

    private int _spawnedThisLevel;
    private float _timeSinceLastSpawn;
    private bool _isSpawning;
    private List<GameObject> _enemies;

    #region | Instance |
    private static WaveController _instance;
    public static WaveController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<WaveController>();
            }

            return _instance;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _enemies = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isSpawning)
        {
            SpawnWave(Levels[CurrentLevel - 1]);
        }
    }

    public void BeginNextLevel()
    {
        Debug.Assert(Levels.Length > 0, $"{MethodBase.GetCurrentMethod().Name} should not be called without '{nameof(Levels)}' being initialized first.");

        if (CurrentLevel < Levels.Length && _enemies.Count == 0)
        {
            CurrentLevel++;
            _spawnedThisLevel = 0;
            _isSpawning = true;
            _enemies.Clear();
        }
        else if(_enemies.Count != 0) 
        {
            Debug.Log("Level In Progress");
        }
        else
        {
            Debug.Log("At the end!");
        }
    }

    public void RemoveFromLevel(GameObject enemy)
    {
        _enemies.Remove(enemy);

        if (_enemies.Count == 0 && !_isSpawning)
        {
            Debug.Log("Finished Level!");
        }
    }

    private void SpawnWave(LevelScriptableObject wave)
    {
        _timeSinceLastSpawn += Time.deltaTime;

        if (_timeSinceLastSpawn >= wave.SpawnSpeed)
        {
            // Spawn enemy
            var go = Instantiate(wave.EnemyToSpawn.PrefabToRender);
            go.transform.position = Spawner.transform.position;
            go.GetComponent<EnemyController>().target = this.Target;

            // Reset spawn time and count up
            _timeSinceLastSpawn = 0f;
            _spawnedThisLevel++;
            _enemies.Add(go);
        }

        if (_spawnedThisLevel >= wave.NumberOfSpawns)
        {
            _isSpawning = false;
        }
    }
}
