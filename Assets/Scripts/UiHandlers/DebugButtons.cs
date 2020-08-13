using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugButtons : MonoBehaviour
{
    public void DEBUG_NextLevel()
    {
        Debug.Log("Next Level");

        WaveController.Instance.BeginNextLevel();
    }
}
