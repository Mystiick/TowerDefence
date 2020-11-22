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
    public Button WallButton;
    public Button SellButton;
    public GameObject ButtonPrefab;
    public float Padding;

    [Header("Controllers")]
    [Tooltip("Game object that contains the BuildingController Component")]
    public GameObject BuildingController;

    public BuildState CurrentBuildState { get; private set; }
    public TowerController Target { get; set; }

    private BuildingController _bc;
    private List<GameObject> _displayButtons;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(ButtonPrefab != null, $"{nameof(ButtonPrefab)} cannot be null on startup.");

        _bc = BuildingController.GetComponent<BuildingController>();
        _displayButtons = new List<GameObject>();

        RemoveAllButtons();
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
        RemoveAllButtons();

        // Determine new buttons to show
        SetDisplayButtons();

        // Actually show them
        for (int i = 0; i < _displayButtons.Count; i++)
        {
            _displayButtons[i].gameObject.SetActive(true);
            _displayButtons[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -Padding * i);
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

            case BuildState.Sell:
                _displayButtons.Add(SellButton.gameObject);
                _displayButtons.Add(CancelButton.gameObject);
                break;

            default:
                break;
        }
    }

    private void RemoveAllButtons()
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

    private void GetBuildableTowerButtons()
    {
        foreach (var tower in PlayerController.Instance.BuildableTowers)
        {
            // Create New Button
            var prefab = Instantiate(ButtonPrefab);
            prefab.GetComponentInChildren<Text>().text = tower.TowerName;
            prefab.transform.SetParent(this.transform, false);
            prefab.name += tower.TowerName;
            prefab.tag = Tags.Generated;

            // Wire up events
            var button = prefab.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                SetBuildTower(tower);
            });

            // Add to _displayButtons
            _displayButtons.Add(prefab);
        }
    }
}
