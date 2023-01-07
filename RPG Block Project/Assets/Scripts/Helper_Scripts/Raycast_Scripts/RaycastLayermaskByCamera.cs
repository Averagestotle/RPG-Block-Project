using System.Collections;
using System.Collections.Generic;
using System.DebugLogs;
using UnityEngine;

public class RaycastLayermaskByCamera : MonoBehaviour
{
    #region Properties
    private IsNullCheckScript IsNullCheck = new IsNullCheckScript();
    #endregion

    public Vector3 FindByLayermaskCheck(Camera cam, LayerMask layerMask, SceneDebugLogScript DebugEnabled = null)
    {
        if (!IsNullCheck.IsLayerMaskNotEmpty(layerMask, DebugEnabled.debugLayerMaskValues))
        {
            return new Vector3();
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        Vector3 destination = new Vector3();        

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask))
        {
            if (DebugEnabled != null && DebugEnabled.debugColliderObject)
            {
                Debug.Log("Object Detected: " + hitInfo.collider.gameObject.name);
            }
            destination = hitInfo.point;
        }

        return destination;
    }

    public GameObject FindGameObjectByLayerMask(Camera cam, LayerMask layerMask, SceneDebugLogScript DebugEnabled = null)
    {
        if (!IsNullCheck.IsLayerMaskNotEmpty(layerMask, DebugEnabled.debugLayerMaskValues)) {
            return null;
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        GameObject agent = null;

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask))
        {
            if (DebugEnabled != null && DebugEnabled.debugColliderObject)
            {
                Debug.Log("Object Detected: " + hitInfo.collider.gameObject.name);
            }
            agent = hitInfo.collider.gameObject;
        }

        return agent;
    }
}
