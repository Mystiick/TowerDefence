using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class BuildMenuPanel : MonoBehaviour
{
    [Header("Buttons")]
    public Button BuildButton;
    public Button CancelButton;
    public Button SellButton;
    public GameObject ButtonPrefab;
    public float Padding;

    [Header("Controllers")]
    [Tooltip("Game object that contains the BuildingController Component")]
    public GameObject BuildingController;

    public BuildState CurrentBuildState { get; private set; }
    public TowerController Target { get; set; }

    private BuildingController _bc;
    
    /// <summary>
    /// List that contains all active buttons. 
    /// Buttons added to the list will be reparented to this.transform, which allows them to display in the GridLayout
    /// </summary>
    private List<GameObject> _displayButtons;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(ButtonPrefab != null, $"{nameof(ButtonPrefab)} cannot be null on startup.");

        _bc = BuildingController.GetComponent<BuildingController>();
        _displayButtons = new List<GameObject>();

        DestroyGeneratedButtons();
        SetBuildState(BuildState.None);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetBuildState(BuildState newState)
    {
        // Reset current state
        CurrentBuildState = newState;
        DestroyGeneratedButtons();

        // Determine new buttons to show
        SetDisplayButtons();

        // Actually show them
        for (int i = 0; i < _displayButtons.Count; i++)
        {
            _displayButtons[i].transform.SetParent(this.transform, false);
            _displayButtons[i].gameObject.SetActive(true);
        }
    }

    public void SetBuildTower(TowerScriptableObject tower)
    {
        _bc.SetPreview(tower);
    }


    private void SetDisplayButtons()
    {
        _displayButtons.Clear();

        switch (CurrentBuildState)
        {
            case BuildState.None:
                _displayButtons.Add(BuildButton.gameObject);
                _bc.ClearPreview();
                break;

            case BuildState.Build:
                GetBuildableTowerButtons();
                _displayButtons.Add(CancelButton.gameObject);
                break;

            case BuildState.Selected:
                GetUpgradeButtons();
                _displayButtons.Add(SellButton.gameObject);
                _displayButtons.Add(CancelButton.gameObject);
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// Cleans up generated buttons by destroying the gameobject
    /// </summary>
    private void DestroyGeneratedButtons()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            GameObject go = this.transform.GetChild(i).gameObject;
            go.SetActive(false);

            if (go.CompareTag(Tags.Generated))
            {
                Destroy(go);
            }
        }
    }

    /// <summary>
    /// Loops through all the buildable base towers and generates buttons for each.
    /// </summary>
    private void GetBuildableTowerButtons()
    {
        foreach (var tower in PlayerController.Instance.BuildableTowers)
        {
            GenerateButton(tower.TowerName, () => SetBuildTower(tower));
        }
    }

    private void GetUpgradeButtons()
    {
        var output = new List<GameObject>();
        var selectedTower = UserInterfaceController.Instance.BuildPanel.Target;

        if (UserInterfaceController.Instance.BuildPanel.Target.Tower.Upgrades.Length > 0)
        {
            foreach (TowerScriptableObject upgrade in selectedTower.Tower.Upgrades)
            {
                GenerateButton($"{upgrade.GoldCost}G : {upgrade.TowerName}", () => selectedTower.TryUpgrade(upgrade));
            }
        }
    }

    /// <summary>
    /// Generates a button to display on the UI and adds them to the <see cref="_displayButtons"/> list
    /// Tags the returned GameObject with 'Generated' so it can be cleaned up when no longer needed.
    /// </summary>
    /// <param name="text">Text to display on the button</param>
    /// <param name="onClick">OnClick Action to trigger when the button is pressed</param>
    private GameObject GenerateButton(string text, UnityEngine.Events.UnityAction onClick)
    {
        var prefab = Instantiate(ButtonPrefab);
        prefab.GetComponentInChildren<Text>().text = text;
        prefab.name += text;
        prefab.tag = Tags.Generated;

        // Wire up events
        var button = prefab.GetComponent<Button>();
        button.onClick.AddListener(onClick);

        _displayButtons.Add(prefab);

        return prefab;
    }
}
