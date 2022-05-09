using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class acts as a controller script for the location trigger to change between the breaking-up and drifting stage
/// </summary>
public class DriftingCollider : MonoBehaviour
{
    private bool used = false;

    void OnTriggerEnter()
    {
        if (!used)
        {
            LabManager.Instance.InitDriftingStage();
            used = true;
        }
    }
}
