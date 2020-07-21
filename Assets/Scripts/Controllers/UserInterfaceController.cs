using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceController : MonoBehaviour
{

    public BuildState _currentBuildState;

    #region | Instance |
    private static UserInterfaceController _instance;
    public static UserInterfaceController Instance()
    {
        if (_instance == null)
        {
            _instance = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<UserInterfaceController>();
        }

        return _instance;
    }
    #endregion

    public void SetBuildState(BuildState newState)
    {
        _currentBuildState = newState;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
