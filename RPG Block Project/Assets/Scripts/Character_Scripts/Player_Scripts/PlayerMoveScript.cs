using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMoveScript : MonoBehaviour
{
    
    public GameObject target;
    public Camera camera;
    public LayerMask layerMask;
    private GameObject player;
    private NavMeshAgent playerNavAgent;
    private Vector3 destinationTarget;
    private Transform pointTransform;

    // Start is called before the first frame update
    void Start()
    {
        player = this.gameObject;
        if (IfGameObjectIsEmpty(player)){
            playerNavAgent = player.GetComponent<NavMeshAgent>();
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (playerNavAgent != null)
        {
            TriggerNavigation();
        }
    }

    

    private void TriggerNavigation()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;            

            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask))
            {
                print(hitInfo.collider.gameObject.name);
                destinationTarget = hitInfo.point;

                // pointTransform = hitInfo.point; TODO: Figure out how to get this damn point to point towards where the mouse clicked not the object it hit's tranform.

                //objectToPlace.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);                
            }
        }

        if (destinationTarget != null)
        { 
            MoveTowardsDestination(playerNavAgent, destinationTarget);
        }    
    }

    private void MoveTowardsDestination(NavMeshAgent agentNavMesh, Vector3 pos)
    {
        agentNavMesh.destination = pos;    
    }

    bool IfGameObjectIsEmpty(GameObject gameObject){
        if (gameObject != null)
        {
            return true;
        }
        else{
            Debug.LogError("A null object was passed.");
            return false;
        }
    }
}
