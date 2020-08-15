using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    NavMeshAgent _agent;
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;

        if (target != null)
        {
            _agent.SetDestination(target.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_agent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            // Rotate 180 degrees to face the right way
            transform.rotation = Quaternion.LookRotation(_agent.velocity.normalized) * Quaternion.AngleAxis(180, transform.up);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.Finish))
        {
            WaveController.Instance.RemoveFromLevel(this.gameObject);
            PlayerController.Instance.Lives -= 1;
            Destroy(this.gameObject);
        }
    }
}
