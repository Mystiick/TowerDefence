using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UserInterfaceController), typeof(BuildingController))]
public class PlayerController : MonoBehaviour
{
    public BuildingController _buildingController;
    public TowerScriptableObject[] debugger;

    // Start is called before the first frame update
    void Start()
    {
        // UserInterfaceController.Instance.BuildableTowers = new[] { debugger[0], debugger[4] };
    }

    // Update is called once per frame
    void Update()
    {

    }

}
