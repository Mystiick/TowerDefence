using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class GhostController : MonoBehaviour
{
    // Defaults are set in the script's properties in the editor
    public Material ValidPlacement;
    public Material InvalidPlacement;
    public Material Original;

    private MeshRenderer mr;

    private void Start()
    {
        mr = this.GetComponent<MeshRenderer>();
        Original = mr.material;
    }

    public void SetPlacementInvalid()
    {
        if (mr == null)
        {
            mr = this.GetComponent<MeshRenderer>();
        }

        mr.material = InvalidPlacement;
    }

    public void SetPlacementValid()
    {
        if (mr == null)
        {
            mr = this.GetComponent<MeshRenderer>();
        }

        mr.material = ValidPlacement;
    }

    public void RemoveGhost()
    {
        if (mr == null)
        {
            mr = this.GetComponent<MeshRenderer>();
        }

        mr.material = Original;
    }
    
}
