using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public BuildingController _buildingController;
    public ScriptableObject[] debugger;
    

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        int selected = -1;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            selected = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            selected = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            selected = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4))
            selected = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5))
            selected = 4;

        if (selected != -1)
        {
            _buildingController.SetPreview((TowerScriptableObject)debugger[selected]);
        }
    }

}
