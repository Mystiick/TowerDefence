using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPanelButtons : MonoBehaviour
{
    public void SetBuildState(string newState)
    {
        if (Enum.TryParse(newState, out BuildState state))
        {
            UserInterfaceController.Instance.BuildPanel.SetBuildState(state);
        }
    }

    public void SetBuildTower(TowerScriptableObject input)
    {
        UserInterfaceController.Instance.BuildPanel.SetBuildTower(input);
    }

    public void SellTower()
    {
        UserInterfaceController.Instance.BuildPanel.SetBuildState(BuildState.None);
        UserInterfaceController.Instance.BuildPanel.Target.Sell();
    }
}
