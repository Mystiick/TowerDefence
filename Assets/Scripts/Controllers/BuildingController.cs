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

    private void UpdatePreview()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.Buildable))
        {
            // Round the position to the nearest 0.5 (TileSize)
            buildingPreview.transform.position = new Vector3(
                hit.point.x - (hit.point.x % TileSize),
                hit.point.y - (hit.point.y % TileSize),
                hit.point.z - (hit.point.z % TileSize)
            );

            // IsPointerOverGameObject tells us if the pointer is over a UI GameObject.
            // Don't spawn a tower if we are trying to click on a button
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {                
                SpawnTower();
            }
        }
    }

    public void SetPreview(TowerScriptableObject tower)
    {
        ResetBuildingController();
        _currentTower = tower;

        // Build out tower to show in preview
        _tempTower = new GameObject() { name = "Temp Tower Preview" };
        AddChild(_tempTower, _currentTower.PrefabToRender, _currentTower.Scale);
        var tc = AddChild(_tempTower, this.tileCover, new Vector3(_currentTower.Width, 1, _currentTower.Height) * TileSize / 10f);

        // Bump up the Y axis 0.01, to prevent Y fighting
        tc.transform.localPosition = new Vector3(0, .01f, 0);

        // Add tower to the preview
        _tempTower.transform.parent = buildingPreview.transform;
        _tempTower.transform.localPosition = Vector3.zero;
    }

    public void ClearPreview()
    {
        ResetBuildingController();
        _currentTower = null;
    }

    private GameObject AddChild(GameObject parent, GameObject child, Vector3 scale)
    {
        GameObject go = Instantiate(child);
        go.transform.parent = parent.transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = scale;

        return go;
    }

    private void ResetBuildingController()
    {
        foreach (Transform c in buildingPreview.transform)
        {
            Destroy(c.gameObject);
        }
    }

    private void SpawnTower()
    {        
        Vector3 colliderSize = new Vector3(_currentTower.Width * 5 - .1f, 1, _currentTower.Height * 5 - .1f) * .1f;

        // Look if we are colliding with anything
        Collider[] overlaps = Physics.OverlapBox(buildingPreview.transform.position, colliderSize / 2, Quaternion.identity, LayerMask.Default);

        // Ignore any triggers
        overlaps = overlaps.Where(x => !x.isTrigger).ToArray();

        if (overlaps.Length == 0 && PlayerController.Instance.Gold >= _currentTower.GoldCost)
        {
            // Mouse has been pressed, copy building preview to a real tower
            var go = Instantiate(_tempTower);
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
                name = "Range Collider"
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
}
