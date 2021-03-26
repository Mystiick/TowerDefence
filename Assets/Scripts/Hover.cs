using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component that lerps an object up and down
/// </summary>
public class Hover : MonoBehaviour
{
    public bool MovingDown;
    public float Speed;
    public float Distance;
        
    private float _percent;
    private Vector3 _start;
    private Vector3 _end;

    // Start is called before the first frame update
    void Start()
    {
        MovingDown = true;
        _start = transform.position;
        _end = new Vector3(_start.x, _start.y - Distance, _start.z);
    }

    // Update is called once per frame
    void Update()
    {
        _percent += Time.deltaTime * Speed;

        if (MovingDown)
        {
            transform.position = Vector3.Lerp(_start, _end, _percent);
        }
        else
        {
            transform.position = Vector3.Lerp(_end, _start, _percent);
        }

        // We've made it to the end of the Lerp, turn it around
        if (_percent >= 1)
        {
            _percent = 0;
            MovingDown = !MovingDown;
        }
    }
}
