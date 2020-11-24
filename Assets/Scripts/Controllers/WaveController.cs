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
    public int LevelCountdown;
    public LevelScriptableObject[] Levels;

    // Spawning Variables
    private int _spawnedThisLevel;
    private bool _isSpawning;
    private List<GameObject> _enemies;
    private List<GameObject> _cleanup;

    // Level timer Variables
    private float _timeToStartLevel;
    private float _timeSinceLastSpawn;
    private bool _countDown;
    private bool _levelTimerEnabled;

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
        Debug.Assert(LevelCountdown >= 0, $"{nameof(LevelCountdown)} must be greater than zero.");

        _enemies = new List<GameObject>();
        _cleanup = new List<GameObject>();

        ResetCountdown();
    }

    // Update is called once per frame
    void Update()
    {
        if (_levelTimerEnabled)
        {
            _timeToStartLevel += _countDown ? -Time.deltaTime : Time.deltaTime;
            UserInterfaceController.Instance.TimeLabel.text = _timeToStartLevel.SecondsToString();

            if (_isSpawning)
            {
                SpawnWave(Levels[CurrentLevel - 1]);
            }

            if (_countDown && _timeToStartLevel <= 0)
            {
                BeginNextLevel();
            }
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
            _countDown = false;
            _enemies.Clear();
            
            PlayerController.Instance.Level = CurrentLevel;
        }
        else if(_enemies.Count != 0) 
        {
            Debug.Log("Level In Progress");
        }
        else
        {
            Debug.Log("At the end!");
            _levelTimerEnabled = false;
            _timeToStartLevel = 0f;
        }
    }

    public void RemoveFromLevel(GameObject enemy)
    {
        _enemies.Remove(enemy);
        _cleanup.Add(enemy);

        if (_enemies.Count == 0 && !_isSpawning)
        {
            LevelFinished();
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
            go.layer = Layer.Enemy;

            var ec = go.GetComponent<EnemyController>();
            ec.target = this.Target;
            ec.Enemy = wave.EnemyToSpawn;

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

    private void ResetCountdown()
    {
        _timeToStartLevel = LevelCountdown;
        _countDown = true;
        _levelTimerEnabled = true;
    }

    private void LevelFinished()
    {
        Debug.Log("Finished Level, cleaning up");
        ResetCountdown();

        foreach (GameObject go in _cleanup)
        {
            Destroy(go);
        }
        _cleanup.Clear();

        // If there are no more levels to complete, turn off the timer and win the game
        if (CurrentLevel == Levels.Length)
        {
            _levelTimerEnabled = true;
            _timeToStartLevel = 0f;
        }
    }
}
