using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asset.Player.Controller
{
    using Asset.Player.Combat;
    using Asset.Player.Controller.CameraController;
    using Asset.Player.Movement;
    using System.DebugLogs;

    [RequireComponent(typeof(CombatControllerScript))]
    [RequireComponent(typeof(CombatHealthScript))]
    [RequireComponent(typeof(CombatTargetScript))]
    [RequireComponent(typeof(CharacterMoveScript))]
    public class PlayerControllerScript : MonoBehaviour
    {
        #region Properties
        private Camera camera;
        public GameObject camPrefabObject;
        public LayerMask navigationLayerMask;
        public LayerMask combatLayerMask;

        private CharacterMoveScript characterMove = new CharacterMoveScript();
        private CombatTargetScript characterCombatTarget = new CombatTargetScript();
        private CombatHealthScript combatHealth = new CombatHealthScript();
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
            characterMove = this.GetComponent<CharacterMoveScript>();
            characterCombatTarget = this.GetComponentInParent<CombatTargetScript>();
            cameraController = camPrefabObject.GetComponentInChildren<CameraBehavior>();
            combatController = this.GetComponent<CombatControllerScript>();
            combatHealth = this.GetComponent<CombatHealthScript>();
            camera = camPrefabObject.GetComponentInChildren<Camera>();
            sceneDebugLog = FindObjectOfType<SceneDebugLogScript>();
        }
        #endregion

        #region Update
        // Update is called once per frame
        void Update()
        {
            cameraController.HandleRotateScollInput();

            if (combatHealth.CheckIfDead()) { return; }
            if (CombatInteration()) return;
            if(MoveToCursor()) return;
            if(IdleState()) return;
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

            if (destination != new Vector3() && IsNullCheck.IsGameObjectNotEmpty(characterMove.gameObject, sceneDebugLog.debugNullValues))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    characterMove.StartMovementAction(destination, sceneDebugLog);
                }

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

                if (combatTarget != null && combatTarget != characterCombatTarget)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        //if (combatTarget == this.GetComponentInParent<CombatTargetScript>()) return false;

                        combatController.AttackCommand(combatTarget, this.gameObject, sceneDebugLog);
                    }

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

        #region Idle Functions
        public bool IdleState()
        {
            if (sceneDebugLog.debugStateLog)
            {
                Debug.Log("Agent is in idle state.");
            }
            
            return true;
        }
        #endregion
    }

}