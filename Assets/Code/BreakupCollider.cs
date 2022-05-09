using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class acts as a controller script for the location trigger to change between the meeting and breaking-up stage
/// </summary>
public class BreakupCollider : MonoBehaviour
{
    private bool used = false;

    void OnTriggerEnter()
    {
        if(!used)
        {
            LabManager.Instance.InitBreakupStage();
            used = true;
        }
    }
    
}
