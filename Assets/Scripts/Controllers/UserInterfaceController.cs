using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceController : MonoBehaviour
{
    [Header("UI Components")]
    public Text GoldLabel;
    public Text LivesLabel;
    public Text LevelLabel;
    public Text TimeLabel;
    public BuildMenuPanel BuildPanel;

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

    /// <summary>
    /// Updates the UI label related to the <paramref name="stat"/>
    /// </summary>
    public void UpdatePlayerStats(StatType stat, int value)
    {
        switch (stat)
        {
            case StatType.Gold:
                GoldLabel.text = value.ToString("N0");
                break;
            case StatType.Level:
                LevelLabel.text = value.ToString("N0");
                break;
            case StatType.Lives:
                LivesLabel.text = value.ToString("N0");
                break;
        }
    }

}
