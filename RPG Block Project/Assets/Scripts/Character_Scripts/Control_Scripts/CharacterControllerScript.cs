using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asset.Player.Controller
{
    using Asset.Player.Combat;
    using Asset.Player.Movement;
    using System.DebugLogs;

    [RequireComponent(typeof(CombatControllerScript))]
    [RequireComponent(typeof(CombatTargetScript))]
    [RequireComponent(typeof(CombatHealthScript))]
    [RequireComponent(typeof(CharacterMoveScript))]
    public class CharacterControllerScript : MonoBehaviour
    {
        #region Properties
        public LayerMask navigationLayerMask;
        public LayerMask combatLayerMask;

        private CharacterMoveScript characterMove = new CharacterMoveScript();
        private CombatControllerScript combatController = new CombatControllerScript();
        private CombatHealthScript combatHealth = new CombatHealthScript();
        private CombatTargetScript characterCombatTarget = new CombatTargetScript();
        private IsNullCheckScript IsNullCheck = new IsNullCheckScript();
        private RaycastAgentByComponent raycastAgent = new RaycastAgentByComponent();
        private SceneDebugLogScript sceneDebugLog = new SceneDebugLogScript();
        private GameObject targetAgent = null;
        #endregion

        #region Start
        // Start is called before the first frame update
        void Start()
        {
            characterMove = this.GetComponent<CharacterMoveScript>();
            combatController = this.GetComponent<CombatControllerScript>();
            combatHealth = this.GetComponent<CombatHealthScript>();
            characterCombatTarget = this.GetComponentInParent<CombatTargetScript>();
            sceneDebugLog = FindObjectOfType<SceneDebugLogScript>();
            targetAgent = GameObject.FindGameObjectWithTag("Player");
        }
        #endregion

        #region Update
        private void Update()
        {
            if (combatHealth.CheckIfDead()) { return; }
            if (MoveToAgent()) { return; }
            if (CombatInteration()) { return; }
        }
        #endregion

        #region LateUpdate
        #endregion

        #region Movement Functions
        private bool MoveToAgent()
        {
            Vector3 destination = new Vector3();
            Vector3 origin = this.gameObject.transform.position;
            bool inRange = false;
            bool tooClose = false;

            //targetTransform = raycastAgent.FindPlayerByLayerMaskAndComponent(this.transform.position, combatLayerMask, 10f, sceneDebugLog);

            if (!IsNullCheck.IsGameObjectNotEmpty(targetAgent, sceneDebugLog.debugNullValues))
            {
                return false;               
            }

            destination = targetAgent.transform.position;
            inRange = characterMove.IsAgentInRange(origin, destination, sceneDebugLog);
            tooClose = characterMove.IsAgentTooClose(origin, destination, combatController.GetAttackRange(), sceneDebugLog);

            if (destination != new Vector3() && inRange && !tooClose)
            {
                characterMove.StartMovementAction(destination, sceneDebugLog);
                return true;         
            }
            else
            {
                characterMove.MovementStopped();
                return false;
            }
        }
        #endregion

        #region Combat Functions
        public bool CombatInteration()
        {
            Vector3 destination = new Vector3();
            Vector3 origin = this.gameObject.transform.position;
            bool inRange = false;

            if (IsNullCheck.IsGameObjectNotEmpty(targetAgent, sceneDebugLog.debugNullValues))
            {
                destination = targetAgent.transform.position;
                inRange = characterMove.IsAgentInRange(origin, destination, sceneDebugLog);
                
                CombatTargetScript combatTarget = targetAgent.GetComponentInParent<CombatTargetScript>();

                if (combatTarget != null && combatTarget != characterCombatTarget && inRange)
                {
                    combatController.AttackCommand(combatTarget, this.gameObject, sceneDebugLog);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Idle Functions
        #endregion
    }

}