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
        Debug.Assert(Tower != null, "Tower cannot be null upon startup", this);
    }

    // Update is called once per frame
    void Update()
    {
        if (Tower.CanAttack)
        {
            UpdateAttack();
        }
    }

    /// <summary>
    /// Refunds the player partial amount of the tower's gold cost and destroys it.
    /// </summary>
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

    /// <summary>
    /// Upgrades this tower to the specified <paramref name="newTower"/> tower
    /// </summary>
    /// <param name="newTower"></param>
    public void TryUpgrade(TowerScriptableObject newTower)
    {
        if (PlayerController.Instance.Gold >= newTower.GoldCost)
        {
            Debug.Log($"Upgrading tower to: {newTower.TowerName}");

            PlayerController.Instance.Gold -= newTower.GoldCost;

            // Destroy the old tower
            var model = this.transform.Find(GameObjectNames.TowerModel);
            Destroy(model.gameObject);

            // Create the new tower and position it
            GameObject newModel = Instantiate(newTower.PrefabToRender);
            newModel.transform.parent = this.transform;
            newModel.transform.localPosition = Vector3.zero;
            newModel.transform.localScale = newTower.Scale;
            newModel.name = GameObjectNames.TowerModel;

            // Update scriptable object
            this.Tower = newTower;

            // Update range
            var range = this.transform.Find(GameObjectNames.RangeCollider);
            range.GetComponent<SphereCollider>().radius = newTower.Range;
        }

        // Set the build state again to update the buttons based on the new tower
        UserInterfaceController.Instance.BuildPanel.SetBuildState(BuildState.Selected);
    }

    /// <summary>
    /// Finds the closest enemy and throws a projectile at it.
    /// </summary>
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
                GameObject go = ObjectPool.Instance.GetObject(Tower.Projectile);
                go.SetActive(true);
                go.transform.position = this.transform.position;

                if (!go.TryGetComponent<ProjectileController>(out var pc))
                {
                    pc = go.AddComponent<ProjectileController>();
                }

                pc.Init();

                //TODO: Pull this data from the Tower's scriptable object
                pc.Target = closest;
                pc.TravelTime = .5f;
                pc.Damage = Tower.Damage;
                pc.ExistanceTime = 0f;

                // Reset cooldown
                _cooldownRemaining = Tower.AttackCooldown;
            }

        }
    }
}
