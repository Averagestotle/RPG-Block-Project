using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    //NOTE: ATTACH THIS SCRIPT ON THE CAMERA PREFAB ITSELF, NOT THE CHILD CAMERA

    // Attach to Parent Camera prefab
    public Transform cameraTransform;
    public Camera cameraObject;
    // Attach to Parent Player prefab, 
    // or anything you'd like it to follow
    public Transform targetTransform;

    // Determines how "smooth" the camera will be. 
    // Higher value means less time for it scroll/rotate.
    // Lower values gives a drifting effect.
    public float cameraTime;

    // How far with the camera rotate per frame.
    public float rotationAmount;
    // How far with the camera scroll in/out per frame.
    public float scrollSpeed;   
    
    public float minZoomAllowed;
    public float maxZoomAllowed;
    public int initZoomLength;
    public float currentZoomLevel;

    // The starting position of where the camera will be,
    // modify the z axis if you want the camera to be further
    public Quaternion newRotation;
    
    private Vector3 normalizedCameraPosition;
    private Vector3 newPosition;

    private void Awake()
    {
        SetupPerspectiveCameraOrbit(cameraObject, new Vector3(initZoomLength, initZoomLength, 0f), initZoomLength);
    }

    // Start is called before the first frame update
    void Start()
    {
        newPosition = targetTransform.position;
        newRotation = transform.rotation;
    }

    public void SetupPerspectiveCameraOrbit(Camera cam, Vector3 offset, float initialZoomLength)
    {
        normalizedCameraPosition = new Vector3(0f, Mathf.Abs(offset.y), -Mathf.Abs(offset.x)).normalized;
        currentZoomLevel = initialZoomLength;
        PositionCamera(cam);
    }

    private void PositionCamera(Camera cam)
    {
        cam.transform.localPosition = normalizedCameraPosition * currentZoomLevel;
    }

    private void ZoomIn(Camera cam, float delta, float nearZoomLimit)
    {
        if (currentZoomLevel <= nearZoomLimit) return;

        currentZoomLevel = currentZoomLevel - delta;

        if (currentZoomLevel <= nearZoomLimit) 
        {
            currentZoomLevel = nearZoomLimit;
        }
        
        PositionCamera(cam);
    }

    private void ZoomOut(Camera cam, float delta, float farZoomLimit)
    {
        if (currentZoomLevel >= farZoomLimit) return;

        currentZoomLevel = currentZoomLevel + delta;

        if (currentZoomLevel >= farZoomLimit) 
        {
            currentZoomLevel = farZoomLimit;
        }

        PositionCamera(cam);
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
        if (Input.GetMouseButton(1))
        {
            newRotation *= Quaternion.Euler(Vector3.up * Input.GetAxis("Mouse X") * rotationAmount);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
        }

        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
        }

        if (Input.mouseScrollDelta.y > 0)
        {
                ZoomIn(cameraObject, scrollSpeed, minZoomAllowed);
        }

        if (Input.mouseScrollDelta.y < 0)
        {
                ZoomOut(cameraObject, scrollSpeed, maxZoomAllowed);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, (Time.deltaTime * cameraTime));
    }
}
