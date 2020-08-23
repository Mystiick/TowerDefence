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

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(target != null);

        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.SetDestination(target.transform.position);

        Health = Enemy.Health;
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
        if (other.CompareTag(Tags.Finish))
        {
            PlayerController.Instance.Lives -= 1;
            Remove();
        }
    }

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
