using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildStateSetter : MonoBehaviour
{
    public GameObject buildMenuPanel;

    public void SetBuildState(string newState)
    {
        if (Enum.TryParse(newState, out BuildState state))
        {
            buildMenuPanel.GetComponent<BuildMenuPanel>().SetBuildState(state);
        }
    }

    public void SetBuildTower(TowerScriptableObject input)
    {
        buildMenuPanel.GetComponent<BuildMenuPanel>().SetBuildTower(input);
    }
}
