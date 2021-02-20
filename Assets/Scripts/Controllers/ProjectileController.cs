using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public GameObject Target;
    public float TravelTime;
    public float ExistanceTime;
    public int Damage;

    // Update is called once per frame
    void Update()
    {
        Debug.Assert(Target != null);

        if (ExistanceTime <= TravelTime)
        {
            ExistanceTime += Time.deltaTime;
            this.transform.position = Vector3.Lerp(this.transform.position, Target.transform.position, ExistanceTime / TravelTime);
        }
        else
        {
            if (Target.activeInHierarchy)
            {
                Target.GetComponent<EnemyController>().Hit(Damage);
            }

            var poo = this.gameObject.GetComponent<PooledObject>();
            if (poo != null)
            {
                // If this is a pooled object, release it
                ObjectPool.Instance.ReleaseObject(this.gameObject, poo.Prefab);
            }
            else
            {
                // If this isn't a pooled object, destroy it
                Destroy(this.gameObject);
            }
        }
    }
}
