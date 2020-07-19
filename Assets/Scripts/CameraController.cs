using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    const float cameraSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        Vector3 newPos = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Time.deltaTime * cameraSpeed;

        if (Input.GetButton("Sprint"))
        {
            newPos *= 2;
        }

        transform.Translate(newPos, Space.World);
    }
}
