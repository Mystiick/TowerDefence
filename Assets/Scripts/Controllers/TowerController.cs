using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class TowerController : MonoBehaviour
{
    public TowerScriptableObject Tower;

    private float _cooldownRemaining = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateAttack();
    }

    public void Sell()
    {
        // Only sell active towers
        if (this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(false);

            // Refund 75% rounded down
            PlayerController.Instance.Gold += (int)Math.Floor(Tower.GoldCost * .75);
            Destroy(this.gameObject);
        }
    }

    private void UpdateAttack()
    {
        _cooldownRemaining -= Time.deltaTime;

        // Check ability/attack cooldown
        if (_cooldownRemaining <= 0)
        {
            // If tower can attack, find the closest enemy within range
            GameObject closest = null;
            float closestDistance = float.MaxValue;
            GameObject[] inRange = Physics.OverlapSphere(this.transform.position, Tower.Range, LayerMask.Enemy).Select(x => x.gameObject).ToArray();

            for (int i = 0; i < inRange.Length; i++)
            {
                float thisDistance = Vector3.Distance(this.transform.position, inRange[i].transform.position);

                if (thisDistance < closestDistance)
                {
                    closest = inRange[i];
                    closestDistance = thisDistance;
                }
            }

            if (closest != null)
            {
                Debug.Log("Found Target, attacking");
                // Fire projectile/spell
                GameObject go = Instantiate(Tower.Projectile);
                go.transform.position = this.transform.position;

                var pc = go.AddComponent<ProjectileController>();
                pc.Target = closest;
                pc.TravelTime = .5f;
                pc.Damage = Tower.Damage;

                // Reset cooldown
                _cooldownRemaining = Tower.AttackCooldown;
            }

        }
    }
}
