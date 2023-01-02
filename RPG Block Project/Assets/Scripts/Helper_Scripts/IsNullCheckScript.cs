using System.Collections;
using System.Collections.Generic;
using System.DebugLogs;
using UnityEngine;

public class IsNullCheckScript : MonoBehaviour
{
    public bool IsGameObjectNotEmpty(GameObject gameObject, bool DebugErrorLogEnabled = false)
    {
        if (gameObject != null)
        {
            return true;
        }
        else
        {
            if (DebugErrorLogEnabled)
            {
                Debug.LogError("A null object was passed.");
            }
            return false;
        }
    }

    public bool IsTransformNotEmpty(Transform transformObject, bool DebugErrorLogEnabled = false)
    {
        if (transformObject != null || transformObject)
        {
            return true;
        }
        else
        {
            if (DebugErrorLogEnabled)
            {
                Debug.LogError("A null transform was passed.");
            }
            return false;
        }
    }

    public bool IsLayerMaskNotEmpty(LayerMask layerMask, bool DebugErrorLogEnabled = false)
    {
        if (layerMask.value != 0)
        {
            return true;
        }
        else
        {
            if (DebugErrorLogEnabled)
            {
                Debug.LogError("No Layer Mask was set.");
            }                
            return false;
        }
    }
}
