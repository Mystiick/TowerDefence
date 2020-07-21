using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuPanel : MonoBehaviour
{
    [Header("Buttons")]
    public Button BuildButton;
    public Button CancelButton;
    public Button ArcherTowerButton;
    public Button SellButton;

    [Header("Controllers")]
    [Tooltip("Game object that contains the BuildingController Component")]
    public GameObject BuildingController;

    private BuildState _currentBuildState;
    private BuildingController _bc;

    // Start is called before the first frame update
    void Start()
    {
        SetBuildState(BuildState.None);
        _bc = BuildingController.GetComponent<BuildingController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBuildState(BuildState newState)
    {
        _currentBuildState = newState;

        switch (_currentBuildState)
        {
            case BuildState.None:
                BuildButton.gameObject.SetActive(true);
                CancelButton.gameObject.SetActive(false);
                ArcherTowerButton.gameObject.SetActive(false);
                break;

            case BuildState.Build:
                BuildButton.gameObject.SetActive(false);
                CancelButton.gameObject.SetActive(true);
                ArcherTowerButton.gameObject.SetActive(true);
                break;

            default:
                break;
        }
    }

    public void SetBuildTower(TowerScriptableObject tower)
    {
        Debug.Log("Setting Tower");
        _bc.SetPreview(tower);
    }

}
