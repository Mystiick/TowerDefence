using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    public TowerScriptableObject Tower;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
