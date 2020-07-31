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
        // if (Input.GetMouseButtonDown(1))
        // {
        //     Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        // 
        //     if (Physics.Raycast(r, out RaycastHit hit))
        //     {
        //         _agent.SetDestination(hit.point);
        //     }
        // }
    }
}
