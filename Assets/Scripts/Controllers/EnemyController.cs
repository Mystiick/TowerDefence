using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    NavMeshAgent _agent;
    public GameObject target;
    public EnemyScriptableObject Enemy;

    public int Health;
    public bool DestroyInPath;

    public void Init()
    {
        Debug.Assert(target != null);

        // Setup pathfinding
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.SetDestination(target.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (_agent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            // Rotate 180 degrees to face the right way
            transform.rotation = Quaternion.LookRotation(_agent.velocity.normalized) * Quaternion.AngleAxis(180, transform.up);
        }
        
        if (!_agent.hasPath)
        {
            // TODO: Let this unit attack towers to make a path
            Debug.LogWarning("No path found");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // If the unit has reached the finish, remove a life from the player.
        if (other.CompareTag(Tags.Finish))
        {
            PlayerController.Instance.Lives -= 1;
            // TODO: Lose state
            Remove();
        }
    }

    /// <summary>
    /// Damage the unit for <paramref name="value"/> damage, and removes the unit from the level if they hit 0 hp.
    /// </summary>
    /// <param name="value"></param>
    public void Hit(int value)
    {
        Health -= value;

        if (Health <= 0)
        {
            PlayerController.Instance.Gold += Enemy.GoldValue;
            Remove();
        }
    }

    private void Remove()
    {
        this.gameObject.SetActive(false);
        WaveController.Instance.RemoveFromLevel(this.gameObject);
    }
}
