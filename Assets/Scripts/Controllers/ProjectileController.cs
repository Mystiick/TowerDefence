using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public GameObject Target;
    public float TravelTime;
    public float ExistanceTime;
    public int Damage;
    public Vector3 origin;

    // Update is called once per frame
    void Update()
    {
        Debug.Assert(Target != null);

        if (origin == Vector3.zero) 
        {
            this.Init();
        }

        if (ExistanceTime <= TravelTime)
        {
            ExistanceTime += Time.deltaTime;
            Vector3 target = Vector3.Lerp(origin, Target.transform.position, ExistanceTime / TravelTime);

            transform.rotation = Quaternion.LookRotation(origin - target);
            transform.position = target;
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

    /// <summary>Resets any values on the projectile that should not persist in the </summary>
    public void Init()
    {
        origin = this.transform.position;
    }
}
