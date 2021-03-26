using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

public class BuildingController : MonoBehaviour
{
    public GameObject buildingPreview;
    public GameObject tileCover;
    public float TileSize;

    public Material ValidGhost;
    public Material InvalidGhost;

    private TowerScriptableObject _currentTower;
    private GameObject _tempTower;

    // Start is called before the first frame update
    void Start()
    {
        ResetBuildingController();
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentTower != null)
        {
            UpdatePreview();
        }
    }

    /// <summary>
    /// Updates the position of the preview tower by following the mouse around.
    /// If the mouse button is pressed, spanwn that tower.
    /// </summary>
    private void UpdatePreview()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool rayHit;
        var ghosts = buildingPreview.GetComponentsInChildren<GhostController>();
        buildingPreview.SetActive(true);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.Buildable))
        {
            // Round the position to the nearest 0.5 (TileSize)
            buildingPreview.transform.position = new Vector3(
                hit.point.x - (hit.point.x % TileSize),
                hit.point.y - (hit.point.y % TileSize),
                hit.point.z - (hit.point.z % TileSize)
            );

            rayHit = true;
        }
        else
        {
            buildingPreview.SetActive(false);
            rayHit = false;
        }

        if (IsPlacementValid() && rayHit)
        {
            foreach (var ghost in ghosts)
            {
                ghost.SetPlacementValid();
            }

            // IsPointerOverGameObject tells us if the pointer is over a UI GameObject.
            // Don't spawn a tower if we are trying to click on a button
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                SpawnTower();
            }
        }
        else
        {
            foreach (var ghost in ghosts)
            {
                ghost.SetPlacementInvalid();
            }
        }

    }

    /// <summary>
    /// Sets the preview tower to the specified <see cref="TowerScriptableObject"/>
    /// </summary>
    /// <param name="tower"></param>
    public void SetPreview(TowerScriptableObject tower)
    {
        ResetBuildingController();
        _currentTower = tower;

        // Build out tower to show in preview
        _tempTower = InstantiateTower(true);

        // Add tower to the preview
        _tempTower.transform.parent = buildingPreview.transform;
        _tempTower.transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// Empties the tower preview so no tower is following the cursor around.
    /// </summary>
    public void ClearPreview()
    {
        ResetBuildingController();
        _currentTower = null;
    }

    private GameObject AddChild(GameObject parent, GameObject child, Vector3 scale, string objectName)
    {
        GameObject go = Instantiate(child);
        go.transform.parent = parent.transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = scale;
        go.name = objectName;

        return go;
    }

    /// <summary>
    /// Clears out the building preview's children
    /// </summary>
    private void ResetBuildingController()
    {
        foreach (Transform c in buildingPreview.transform)
        {
            Destroy(c.gameObject);
        }
    }
    
    /// <summary>
    /// Instantiate a real tower if possible, and add all relevant components.
    /// </summary>
    private void SpawnTower()
    {
        if (PlayerController.Instance.Gold >= _currentTower.GoldCost)
        {
            // Subtract .1f from width/height otherwise the colliders will be touching, which is considered overlapping
            Vector3 colliderSize = new Vector3(_currentTower.Width * 5 - .1f, 1, _currentTower.Height * 5 - .1f) * .1f;

            // Mouse has been pressed, copy building preview to a real tower
            var go = InstantiateTower();
            go.transform.position = buildingPreview.transform.position;
            go.name = $"(Clone){_currentTower.TowerName}";

            var bc = go.AddComponent<BoxCollider>();
            bc.size = colliderSize;
            bc.center = Vector3.zero;

            var tc = go.AddComponent<TowerController>();
            tc.Tower = _currentTower;

            var obstacle = go.AddComponent<NavMeshObstacle>();
            obstacle.carving = true;
            obstacle.size = colliderSize;

            var collider = new GameObject()
            {
                layer = Layer.IgnoreRaycast,
                name = GameObjectNames.RangeCollider
            };
            collider.transform.SetParent(go.transform);
            collider.transform.localPosition = Vector3.zero;

            var sc = collider.AddComponent<SphereCollider>();
            sc.center = Vector3.zero;
            sc.radius = _currentTower.Range;
            sc.isTrigger = true;

            PlayerController.Instance.Gold -= _currentTower.GoldCost;
        }
    }

    private GameObject InstantiateTower(bool createGhost = false)
    {
        GameObject go = new GameObject() { name = "Temp Tower Preview" };

        var preview = AddChild(go, _currentTower.PrefabToRender, _currentTower.Scale, GameObjectNames.TowerModel);
        var towerTileCover = AddChild(go, this.tileCover, new Vector3(_currentTower.Width, 1, _currentTower.Height) * TileSize / 10f, "Tile Cover");

        // Bump up the Y axis 0.01, to prevent Y fighting
        towerTileCover.transform.localPosition = new Vector3(0, .01f, 0);

        if (createGhost)
        {
            var ghost = preview.AddComponent<GhostController>();
            ghost.InvalidPlacement = this.InvalidGhost;
            ghost.ValidPlacement = this.ValidGhost;

            ghost = towerTileCover.AddComponent<GhostController>();
            ghost.InvalidPlacement = this.InvalidGhost;
            ghost.ValidPlacement = this.ValidGhost;
        }

        return go;
    }

    private bool IsPlacementValid()
    {
        // Subtract .1f from width/height otherwise the colliders will be touching, which is considered overlapping
        Vector3 colliderSize = new Vector3(_currentTower.Width * 5 - .1f, 1, _currentTower.Height * 5 - .1f) * .1f;

        // Look if we are colliding with anything
        Collider[] overlaps = Physics.OverlapBox(buildingPreview.transform.position, colliderSize / 2, Quaternion.identity, LayerMask.Default);

        return !overlaps.Where(x => !x.isTrigger).Any();

    }
}
