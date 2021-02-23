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

    /// <summary>
    /// Adds <paramref name="count"/> objects to the pool for the specified prefab.
    /// Creates the object pool if one does not yet exist
    /// </summary>
    public void ExpandPool(GameObject prefab, int count)
    {
        // If the dictionary doesn't have a pool for the current prefab, create one
        if (!HasPool(prefab))
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

    /// <summary>
    /// Gets a single object out of the prefab pool. If none are available, it will build out more and return one of the new objects.
    /// </summary>
    /// <param name="prefab">Prefab type to pull form the pool.</param>
    /// <returns></returns>
    public GameObject GetObject(GameObject prefab)
    {
        if (!_pool.ContainsKey(prefab))
        {
            ExpandPool(prefab, 10);
        }

        Queue<GameObject> current = _pool[prefab];

        if (current.Count == 0)
        {
            ExpandPool(prefab, 10);
        }

        return _pool[prefab].Dequeue();
    }

    /// <summary>
    /// Deactivates and adds the specified object back into the pool to be used again. 
    /// Does not reset prefab in any fashion, and when it is retreived again, it will have the old properties.
    /// </summary>
    /// <param name="go">GameObject to add back into the relevant pool. Must have a PooledObject component attached, and already have a pool to add to</param>
    /// <param name="prefab">If you have the prefab on hand, you can pass it in to prevent it from being looked up again</param>
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