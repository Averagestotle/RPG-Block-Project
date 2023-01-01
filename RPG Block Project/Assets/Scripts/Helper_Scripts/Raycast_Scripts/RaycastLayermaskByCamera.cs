using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastLayermaskByCamera : MonoBehaviour
{
    public Vector3 FindByLayermaskCheck(Camera cam, LayerMask layerMask)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        Vector3 destination = new Vector3();

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask))
        {
            print(hitInfo.collider.gameObject.name);
            destination = hitInfo.point;
        }

        return destination;
    }
}
