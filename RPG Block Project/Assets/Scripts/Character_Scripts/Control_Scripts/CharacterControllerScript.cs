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
        public float chaseRange;

        private CharacterMoveScript characterMove = new CharacterMoveScript();
        private CombatControllerScript combatController = new CombatControllerScript();
        private CombatHealthScript combatHealth = new CombatHealthScript();
        private CombatTargetScript characterCombatTarget = new CombatTargetScript();
        private IsNullCheckScript IsNullCheck = new IsNullCheckScript();
        private RaycastAgentByComponent raycastAgent = new RaycastAgentByComponent();
        private SceneDebugLogScript sceneDebugLog = new SceneDebugLogScript();
        private GameObject targetAgent = null;
        private Vector3 characterGuardPosition = new Vector3();
        private Vector3 targetsKnownPosition = new Vector3();

        // Debug Gizmos variables
        private Vector3 debugCharactersCurrentDestination = new Vector3();
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
            characterGuardPosition = transform.position;
        }
        #endregion

        #region Update
        private void Update()
        {
            if (combatHealth.CheckIfDead()) 
            {
                debugCharactersCurrentDestination = new Vector3();
                return; 
            }

            if (MoveToAgent()) { return; }
            if (CombatInteration()) { return; }
            if (MoveToGaurdPost()) { return; }
        }
        #endregion

        #region LateUpdate
        #endregion

        #region Movement Functions
        private bool MoveToAgent()
        {
            Vector3 targetAgentPosition = new Vector3();
            Vector3 origin = this.gameObject.transform.position;
            bool inRange = false;
            bool tooClose = false;

            if (!IsNullCheck.IsGameObjectNotEmpty(targetAgent, sceneDebugLog.debugNullValues))
            {
                return false;               
            }

            targetAgentPosition = targetAgent.transform.position;
            inRange = characterMove.IsAgentInRange(origin, targetAgentPosition, chaseRange, sceneDebugLog);
            tooClose = characterMove.IsAgentTooClose(origin, targetAgentPosition, combatController.GetAttackRange(), sceneDebugLog);

            if (targetAgentPosition != new Vector3() && inRange && !tooClose)
            {
                targetsKnownPosition = targetAgentPosition; // We want the agent to move to their target's last known position if they're outside of range.
                debugCharactersCurrentDestination = targetsKnownPosition;
                characterMove.StartMovementAction(targetsKnownPosition, sceneDebugLog);
                return true;         
            }
            else
            {
                // characterMove.MovementStopped();
                return false;
            }
        }

        private bool MoveToGaurdPost()
        {
            if(characterGuardPosition != new Vector3() &&
                (targetsKnownPosition != new Vector3() && transform.position == targetsKnownPosition) &&
                transform.position != characterGuardPosition)
            {
                characterMove.StartMovementAction(characterGuardPosition, sceneDebugLog);
                debugCharactersCurrentDestination = characterGuardPosition;
                return true;
            }
            else
            {
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
                inRange = characterMove.IsAgentInRange(origin, destination, chaseRange, sceneDebugLog);
                
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

        #region Debug Functions
        private void OnDrawGizmosSelected()
        {
            sceneDebugLog.DebugDrawAgentsChaseRange(transform.position, chaseRange, Color.cyan);
            sceneDebugLog.DebugDrawAgentsAttackRange(transform.position, combatController.GetAttackRange(), Color.red);
            sceneDebugLog.DebugFindAgentsDestination(transform.position, debugCharactersCurrentDestination, Color.cyan);          
        }
        #endregion
    }

}