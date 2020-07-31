using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceController : MonoBehaviour
{
    [Header("UI Components")]
    public Text GoldLabel;
    public BuildMenuPanel BuildPanel;

    //private BuildMenuPanel
    #region | Instance |
    private static UserInterfaceController _instance;
    public static UserInterfaceController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<UserInterfaceController>();
            }

            return _instance;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

}
