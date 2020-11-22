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


        if (Target.activeInHierarchy)
        {
            if (ExistanceTime <= TravelTime)
            {
                ExistanceTime += Time.deltaTime;
                this.transform.position = Vector3.Lerp(this.transform.position, Target.transform.position, ExistanceTime / TravelTime);
            }
            else
            {
                Target.GetComponent<EnemyController>().Hit(Damage);
                Destroy(this.gameObject);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
