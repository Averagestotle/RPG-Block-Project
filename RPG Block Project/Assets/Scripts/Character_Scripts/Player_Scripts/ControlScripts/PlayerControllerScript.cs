using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asset.Player.Controller
{
    using Asset.Player.Combat;
    using Asset.Player.Controller.CameraController;
    using Asset.Player.Movement;
    using System.DebugLogs;

    public class PlayerControllerScript : MonoBehaviour
    {
        #region Properties
        private Camera camera;
        public GameObject camPrefabObject;
        public LayerMask navigationLayerMask;
        public LayerMask combatLayerMask;

        private PlayerMoveScript playerMove = new PlayerMoveScript();
        private CameraBehavior cameraController = new CameraBehavior();
        private IsNullCheckScript IsNullCheck = new IsNullCheckScript();
        private CombatControllerScript combatController= new CombatControllerScript();
        private RaycastLayermaskByCamera raycastLayermaskByCamera = new RaycastLayermaskByCamera();
        private SceneDebugLogScript sceneDebugLog = new SceneDebugLogScript();
        #endregion

        #region Start
        // Start is called before the first frame update
        void Start()
        {
            playerMove = GetComponent<PlayerMoveScript>();
            cameraController = camPrefabObject.GetComponentInChildren<CameraBehavior>();
            combatController = this.GetComponent<CombatControllerScript>();
            camera = camPrefabObject.GetComponentInChildren<Camera>();
            sceneDebugLog = FindObjectOfType<SceneDebugLogScript>();
        }
        #endregion

        #region Update
        // Update is called once per frame
        void Update()
        {
            cameraController.HandleRotateScollInput();
            
            if(CombatInteration()) return;
            if(MoveToCursor()) return;
            print("Idle State.");
        }
        #endregion

        #region LateUpdate
        void LateUpdate()
        {
            cameraController.HandleCameraMovement(this.transform);
        }
        #endregion

        #region Movement Functions
        public bool MoveToCursor()
        {
            Vector3 destination = new Vector3();

            destination = raycastLayermaskByCamera.FindByLayermaskCheck(camera, navigationLayerMask, sceneDebugLog);

            if (destination != new Vector3() && IsNullCheck.IsGameObjectNotEmpty(playerMove.gameObject, sceneDebugLog.debugNullValues))
            {
                if (Input.GetMouseButton(0))
                {
                    playerMove.StartMovementAction(destination);
                }
                //playerMove.StartMovementAction(destination);
                //playerMove.MoveTowardsDestination(destination);
                return true;
            } else
            {
                return false;
            }            
        }
        #endregion

        #region Combat Functions
        public bool CombatInteration()
        {
            GameObject agent;
            agent = raycastLayermaskByCamera.FindGameObjectByLayerMask(camera, combatLayerMask, sceneDebugLog);

            if (IsNullCheck.IsGameObjectNotEmpty(agent, sceneDebugLog.debugNullValues))
            {
                CombatTargetScript combatTarget = agent.GetComponentInParent<CombatTargetScript>();

                if (combatTarget != null)
                {
                    if (Input.GetMouseButton(0))
                    {
                        combatController.AttackCommand(combatTarget, sceneDebugLog.debugCombatLog);
                    }
                    //combatController.AttackCommand(combatTarget, sceneDebugLog.debugCombatLog);
                    return true;
                } else
                {
                    return false;
                }
            } else
            {
                return false;
            }       
        }
        #endregion
    }

}