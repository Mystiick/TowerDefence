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
    private bool _isSpawning;
    private Queue<GameObject> _enemyQueue;
    private List<GameObject> _aliveEnemies;
    private List<GameObject> _cleanup;

    // Level timer Variables
    public float _timeToStartLevel;
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

        _aliveEnemies = new List<GameObject>();
        _cleanup = new List<GameObject>();
        _enemyQueue = new Queue<GameObject>();

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
                TrySpawnNextEnemy();
            }

            if (_countDown && _timeToStartLevel <= 0)
            {
                BeginNextLevel();
            }
        }
    }

    /// <summary>
    /// Cleans up the current level, and builds out the queue of enemies that will filter into the map for the next level.
    /// </summary>
    public void BeginNextLevel()
    {
        Debug.Assert(Levels.Length > 0, $"{MethodBase.GetCurrentMethod().Name} should not be called without '{nameof(Levels)}' being initialized first.");

        if (CurrentLevel < Levels.Length && _aliveEnemies.Count == 0)
        {
            CurrentLevel++;
            _isSpawning = true;
            _countDown = false;
            _aliveEnemies.Clear();
            LoadEnemyQueue(Levels[CurrentLevel - 1]);
            
            PlayerController.Instance.Level = CurrentLevel;
        }
        else if(_aliveEnemies.Count != 0) 
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

    /// <summary>
    /// Iterates over the current level's waves and adds enemies to the queue
    /// </summary>
    /// <param name="level"></param>
    private void LoadEnemyQueue(LevelScriptableObject level)
    {
        _enemyQueue.Clear();

        foreach (Wave w in level.Waves)
        {
            Debug.Assert(w.HealthModifier != 0);

            for (int i = 0; i < w.NumberOfSpawns; i++)
            {
                var go = ObjectPool.Instance.GetObject(w.EnemyToSpawn.PrefabToRender);

                go.transform.position = Spawner.transform.position;
                go.layer = Layer.Enemy;

                var ec = go.GetComponent<EnemyController>();
                ec.target = this.Target;
                ec.Enemy = w.EnemyToSpawn;
                ec.Health = (int)(w.EnemyToSpawn.Health * w.HealthModifier);

                go.SetActive(false);

                _enemyQueue.Enqueue(go);
            }
        }
    }

    /// <summary>
    /// Spawns the next enemy in the queue if the spawner's timer has elapsed.
    /// Sets isSpawning to false if there are no more enemies in the queue.
    /// </summary>
    private void TrySpawnNextEnemy()
    {
        var wave = Levels[CurrentLevel - 1];
        _timeSinceLastSpawn += Time.deltaTime;

        if (_timeSinceLastSpawn >= wave.SpawnSpeed)
        {
            // Spawn enemy
            var go = _enemyQueue.Dequeue();
            go.SetActive(true);
            var ec = go.GetComponent<EnemyController>();
            ec.Init();

            _timeSinceLastSpawn = 0f;
            _aliveEnemies.Add(go);
        }

        if (_enemyQueue.Count == 0)
        {
            _isSpawning = false;
        }
    }

    /// <summary>
    /// Flags an enemy for removal from the current level upon cleanup.
    /// If there are no more enemies alive, it marks the level as finished to begin the transition to the next level.
    /// </summary>
    /// <param name="enemy"></param>
    public void RemoveFromLevel(GameObject enemy)
    {
        _aliveEnemies.Remove(enemy);
        _cleanup.Add(enemy);

        if (_aliveEnemies.Count == 0 && !_isSpawning)
        {
            LevelFinished();
        }
    }

    private void ResetCountdown()
    {
        _timeToStartLevel = LevelCountdown;
        _countDown = true;
        _levelTimerEnabled = true;
    }

    /// <summary>
    /// Cleans up the current level, and releases all pooled enemies back into the pool
    /// </summary>
    private void LevelFinished()
    {
        Debug.Log("Finished Level, cleaning up");
        ResetCountdown();

        foreach (GameObject go in _cleanup)
        {
            ObjectPool.Instance.ReleaseObject(go);
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
