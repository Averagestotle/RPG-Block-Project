using Asset.Player.Controller;
using System.Collections;
using System.Collections.Generic;
using System.DebugLogs;
using UnityEngine;

public class RaycastAgentByComponent : MonoBehaviour
{
    public Transform FindPlayerByLayerMaskAndComponent(Vector3 position,LayerMask layerMask, float maxDistance, SceneDebugLogScript DebugEnabled = null)
    {
        //NOTE: Function does not work currently, calling this leads to a NullReferenceException. Further research is needed to figure out why.
        RaycastHit[] hits = Physics.SphereCastAll(position, maxDistance / 2, transform.forward, maxDistance, layerMask,QueryTriggerInteraction.UseGlobal);

        if(hits == null || hits.Length > 0) { return null; }

        for (int i = 0; i < hits.Length; i++)
        {
            if (DebugEnabled != null && DebugEnabled.debugColliderObject)
            {
                Debug.Log("Object Detected: " + hits[i].collider.gameObject.name);
            }

            PlayerControllerScript playerController = hits[i].transform.GetComponentInParent<PlayerControllerScript>();

            if(playerController == null) { continue; }
            if(playerController !=null) { return hits[i].transform; }
        }

        return null;

        
    }

    public bool IsAgentInRange(Vector3 origin, Vector3 targetOrigin, float maxDistance, SceneDebugLogScript DebugEnabled = null)
    {
        // Working function :)
        bool canMove = false;
        bool inRange = false; 

        if (origin == new Vector3() || targetOrigin == new Vector3()) { return false; }

        inRange = Vector3.Distance(origin, targetOrigin) <= maxDistance;
        
        if(inRange)
        {
            canMove = true;
        }        

        return canMove;
    }

    public bool IsAgentTooClose(Vector3 origin, Vector3 targetOrigin, float maxDistance, SceneDebugLogScript DebugEnabled = null)
    {
        // Working function :)
        bool canMove = false;
        bool tooClose = false;

        if (origin == new Vector3() || targetOrigin == new Vector3()) { return false; }

        tooClose = Vector3.Distance(origin, targetOrigin) <= maxDistance;

        if (tooClose)
        {
            canMove = false;
        }

        return canMove;
    }
}
