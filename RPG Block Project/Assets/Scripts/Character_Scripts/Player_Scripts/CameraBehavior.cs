using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    //NOTE: ATTACH THIS SCRIPT ON THE CAMERA PREFAB ITSELF, NOT THE CHILD CAMERA

    // Attach to Parent Camera prefab
    public Transform cameraTransform;
    // Attach to Parent Player prefab, or anything you'd like it to follow
    public Transform targetTransform;

    // Determines how "smooth" the camera will be. 
    // Higher value means less time for it scroll/rotate.
    // Lower values gives a drifting effect.
    public float cameraTime;

    // How far with the camera rotate per frame.
    public float rotationAmount;
    // How far with the camera scroll in/out per frame.
    public float scrollSpeed;

    // The starting position of where the camera will be,
    // modify the z axis if you want the camera to be further
    // away from the target.
    public Vector3 newPosition;
    public Vector3 zoomAmount;
    public Vector3 newZoom;
    public float minZoomAllowed;
    public float maxZoomAllowed;
    public Quaternion newRotation;

    // Start is called before the first frame update
    void Start()
    {
        newPosition = targetTransform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        HandleRotateScollInput();       
    }

    void LateUpdate()
    {
        HandleCameraMovement(targetTransform);        
    }

    private void HandleCameraMovement(Transform targetObj) 
    {
        if (targetObj != null)
        {
            newPosition = targetTransform.position;
            transform.position = Vector3.Lerp(targetObj.position, newPosition, (Time.deltaTime * cameraTime));
        }        
    }

    private void HandleRotateScollInput()
    {
        Vector3 zoomCalcValue = new Vector3();

        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }
        
        newZoom += new Vector3(zoomAmount.x, (zoomAmount.y * scrollSpeed) * Input.mouseScrollDelta.y, (zoomAmount.z * scrollSpeed) * Input.mouseScrollDelta.y);
        // newZoom += new Vector3(Mathf.Clamp(zoomCalcValue.x, minZoomAllowed, maxZoomAllowed), 
        //                       (Mathf.Clamp(zoomCalcValue.y, minZoomAllowed, maxZoomAllowed) * scrollSpeed) * Input.mouseScrollDelta.y, (
        //                       Mathf.Clamp(zoomCalcValue.z, minZoomAllowed, maxZoomAllowed) * scrollSpeed) * Input.mouseScrollDelta.y);

        /* newZoom.x = Mathf.Clamp(zoomCalcValue.x, minZoomAllowed, maxZoomAllowed);
        newZoom.y = Mathf.Clamp(zoomCalcValue.y, minZoomAllowed, maxZoomAllowed);
        newZoom.z = Mathf.Clamp(zoomCalcValue.z, minZoomAllowed, maxZoomAllowed); */

        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, (Time.deltaTime * cameraTime));
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, (Time.deltaTime * cameraTime));
    }
}
