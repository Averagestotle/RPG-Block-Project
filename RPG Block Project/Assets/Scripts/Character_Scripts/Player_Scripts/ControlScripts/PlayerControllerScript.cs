using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asset.Player.Controller
{
    using Asset.Player.Controller.CameraController;
    using Asset.Player.Movement;

    public class PlayerControllerScript : MonoBehaviour
    {
        #region Properties
            private Camera camera;
            public GameObject camPrefabObject;
            public LayerMask layerMask;
          
            private PlayerMoveScript playerMove = new PlayerMoveScript();
            private CameraBehavior cameraController = new CameraBehavior();
            private IsNullCheckScript IsNullCheck = new IsNullCheckScript();
            private RaycastLayermaskByCamera raycastLayermaskByCamera = new RaycastLayermaskByCamera();
        #endregion

        #region Start
        // Start is called before the first frame update
        void Start()
            {
                playerMove = GetComponent<PlayerMoveScript>();
                cameraController = camPrefabObject.GetComponentInChildren<CameraBehavior>();
                camera = camPrefabObject.GetComponentInChildren<Camera>();
            }
        #endregion

        #region Update
            // Update is called once per frame
            void Update()
            {
            cameraController.HandleRotateScollInput();
            
            if (Input.GetMouseButton(0))
                {
                    MoveToCursor();
                }
            }
        #endregion

        #region LateUpdate
        void LateUpdate()
        {
            cameraController.HandleCameraMovement(this.transform);
        }
        #endregion

        #region Movement
        public void MoveToCursor()
            {
                Vector3 destination = new Vector3();

                destination = raycastLayermaskByCamera.FindByLayermaskCheck(camera, layerMask);

                if (destination != null && IsNullCheck.IsGameObjectEmpty(playerMove.gameObject))
                {
                    playerMove.MoveTowardsDestination(destination);
                }
            }
        #endregion
    }

}