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

        if (target != null)
        {
            _agent.SetDestination(target.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag.Finish))
        {
            WaveController.Instance.RemoveFromLevel(this.gameObject);
            Destroy(this.gameObject);
        }
    }
}
