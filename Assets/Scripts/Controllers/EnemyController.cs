using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    NavMeshAgent _agent;
    public GameObject target;
    public EnemyScriptableObject Enemy;
    public Slider HealthBar;

    public int CurrentHealth;
    public bool DestroyInPath;

    /// <summary>
    /// Should be run any time the game object is set to active(true)
    /// </summary>
    public void Init()
    {
        Debug.Assert(target != null);

        // Setup pathfinding
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.SetDestination(target.transform.position);

        HealthBar.gameObject.SetActive(false);
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
        CurrentHealth -= value;

        HealthBar.gameObject.SetActive(true);
        HealthBar.value = ((float)CurrentHealth / (float)Enemy.Health);

        if (CurrentHealth <= 0)
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
