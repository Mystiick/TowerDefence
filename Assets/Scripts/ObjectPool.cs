using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject[] PoolWarmer;

    // Key: Prefab, Value: Queue of pooled objects
    private Dictionary<GameObject, Queue<GameObject>> _pool;

    #region | Instance |
    private static ObjectPool _instance;
    public static ObjectPool Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<ObjectPool>();
            }

            return _instance;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _pool = new Dictionary<GameObject, Queue<GameObject>>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PreWarm(GameObject prefab, int count)
    {
        // If the dictionary doesn't have a pool for the current prefab, create one
        if (!_pool.ContainsKey(prefab))
        {
            _pool.Add(prefab, new Queue<GameObject>());
        }

        Queue<GameObject> current = _pool[prefab];
        for (int i = 0; i < count; i++)
        {
            var go = Instantiate(prefab);
            go.SetActive(false);

            var poo = go.AddComponent<PooledObject>();
            poo.Prefab = prefab;

            current.Enqueue(go);
        }
    }

    public GameObject GetObject(GameObject prefab)
    {
        if (!_pool.ContainsKey(prefab))
        {
            PreWarm(prefab, 10);
        }

        Queue<GameObject> current = _pool[prefab];

        if (current.Count == 0)
        {
            PreWarm(prefab, 10);
        }

        return _pool[prefab].Dequeue();
    }

    public void ReleaseObject(GameObject go, GameObject prefab = null)
    {
        if (prefab == null)
        {
            prefab = go.GetComponent<PooledObject>().Prefab;
        }

        go.SetActive(false);
        _pool[prefab].Enqueue(go);
    }

    public bool HasPool(GameObject prefab)
    {
        return _pool.ContainsKey(prefab);
    }
}