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
        public float maxSuspicionTime = Mathf.Infinity;
        public float maxWaypointWaitTime = Mathf.Infinity;

        [SerializeField] PatrolPathScript patrolPath;

        
        private CharacterMoveScript characterMove = new CharacterMoveScript();
        private CombatControllerScript combatController = new CombatControllerScript();
        private CombatHealthScript combatHealth = new CombatHealthScript();
        private CombatTargetScript characterCombatTarget = new CombatTargetScript();
        private IsNullCheckScript IsNullCheck = new IsNullCheckScript();
        private RaycastAgentByComponent raycastAgent = new RaycastAgentByComponent();
        
        private GameObject targetAgent = null;

        private Vector3 characterGuardPosition = new Vector3();
        private Vector3 targetsKnownPosition = new Vector3();

        private int waypointIndex = 0;

        private float suspicionTimeCounted = 0F;

        // Debug variables
        private SceneDebugLogScript sceneDebugLog = new SceneDebugLogScript();
        private Vector3 debugCharactersCurrentDestination = new Vector3();
        public float debugSuspicionTimeCounted = 0F;
        public Vector3 debugNextWaypointPosition = new Vector3();
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
            bool inRange = false;

            if (patrolPath != null)
            {
                if (characterMove.AgentIsInPosition(this.transform.position, patrolPath.GetWaypoint(waypointIndex)))
                {                   
                    waypointIndex = patrolPath.GetNextWaypoint(waypointIndex);
                    debugNextWaypointPosition = patrolPath.GetWaypoint(waypointIndex);
                }
                characterGuardPosition = patrolPath.GetWaypoint(waypointIndex);
            }            

            if (targetsKnownPosition != new Vector3())
            {
                inRange = characterMove.IsAgentInRange(transform.position, targetsKnownPosition, 1f, sceneDebugLog);
            }
            // Note: Not really liking these if statements, but here it is:

            // 1) Check to make sure the charcter is at the last known location.
            // 2) They're not at their guard post.
            // 3) The suspicion time counted is greater than the max amount of time allowed to wait.
            // 4) Go to their guard post.
            if (characterGuardPosition != new Vector3() &&
                (targetsKnownPosition != new Vector3() && inRange) &&
                transform.position != characterGuardPosition &&
                maxSuspicionTime <= suspicionTimeCounted)
            {
                // Reset timer and last known location.
                suspicionTimeCounted = 0;
                debugSuspicionTimeCounted = suspicionTimeCounted;

                targetsKnownPosition = new Vector3();

                characterMove.StartMovementAction(characterGuardPosition, sceneDebugLog);
                debugCharactersCurrentDestination = characterGuardPosition;
                return true;
            }
            else if (characterGuardPosition != new Vector3() &&
                transform.position != characterGuardPosition &&
                targetsKnownPosition == new Vector3())
            {
                suspicionTimeCounted = 0;
                debugSuspicionTimeCounted = suspicionTimeCounted;

                characterMove.StartMovementAction(characterGuardPosition, sceneDebugLog);
                debugCharactersCurrentDestination = characterGuardPosition;
                return true;
            }
            else if (targetsKnownPosition != new Vector3() && (inRange))
            {
                suspicionTimeCounted += Time.deltaTime;
                debugSuspicionTimeCounted = suspicionTimeCounted;
                return false;
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