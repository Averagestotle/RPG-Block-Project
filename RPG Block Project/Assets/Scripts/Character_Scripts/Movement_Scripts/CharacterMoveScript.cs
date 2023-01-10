using Asset.Player.Controller;
using System.DebugLogs;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;

namespace Asset.Player.Movement
{
    public class CharacterMoveScript : MonoBehaviour, IActionScript
    {
        #region Properties
        private GameObject character;
        private NavMeshAgent characterNavAgent;
        private ActionSchedulerScript actionScheduler;
        private IsNullCheckScript IsNullCheck = new IsNullCheckScript();
        public float movementSpeed;
        public bool isStopped;
        public float chaseRange;
        public float targetDistance;
        
        #endregion

        #region Start
        // Start is called before the first frame update
        void Start()
            {
                character = this.gameObject;
                if (IsNullCheck.IsGameObjectNotEmpty(character))
                {
                    characterNavAgent = character.GetComponent<NavMeshAgent>();
                    actionScheduler = this.GetComponent<ActionSchedulerScript>();
                }
            }
        #endregion

        #region Movement
        public void StartMovementAction(Vector3 pos, SceneDebugLogScript sceneDebugLog = null)
        {
            

            if (actionScheduler != null)
            {
                actionScheduler.StartAction(this, sceneDebugLog);
            }
            MoveTowardsDestination(pos);
        }

        public void MoveTowardsDestination(Vector3 pos, SceneDebugLogScript sceneDebugLog = null)
        {
            if (characterNavAgent != null && characterNavAgent.enabled)
            {
                characterNavAgent.destination = pos;
                characterNavAgent.speed = movementSpeed;
                characterNavAgent.isStopped = false;
                isStopped = characterNavAgent.isStopped;                 
            } else
            {
                if (sceneDebugLog != null && sceneDebugLog.debugCharacterMovementLog)
                {
                    print("Agent cannot move: No navMesh assigned.");
                }                
            }
        }

        public void MovementStopped()
        {
            if (characterNavAgent != null && characterNavAgent.enabled)
            {
                characterNavAgent.isStopped = true;
                isStopped = characterNavAgent.isStopped;
            }
        }

        public bool IsAgentInRange(Vector3 origin, Vector3 targetOrigin, float maxDistance, SceneDebugLogScript DebugEnabled = null)
        {
            bool inRange = false;
            if (chaseRange == 0)
            {
                if (DebugEnabled.debugCharacterMovementLog)
                {
                    Debug.LogError("Chanse distance was not set.");
                }
                return false;
            }

            if (origin == new Vector3() || targetOrigin == new Vector3()) { return false; }

            inRange = Vector3.Distance(origin, targetOrigin) <= maxDistance;
            targetDistance = Vector3.Distance(origin, targetOrigin);

            if (inRange)
            {
                return true;
            }

            return false;
        }

        public bool IsAgentTooClose(Vector3 origin, Vector3 targetOrigin, float maxDistance, SceneDebugLogScript DebugEnabled = null)
        {
            bool tooClose = false;

            if (origin == new Vector3() || targetOrigin == new Vector3()) { return false; }

            tooClose = Vector3.Distance(origin, targetOrigin) <= maxDistance;
            targetDistance = Vector3.Distance(origin, targetOrigin);

            if (tooClose)
            {
                return true;
            }

            return false;
        }

        public bool AgentIsInPosition(Vector3 a, Vector3 b)
        {
            if (a == new Vector3() || b == new Vector3()) { return false; }

            if (a.x == b.x && a.z == b.z)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region IAction Interface
        public void SwitchAction() 
        {
            if (characterNavAgent != null && characterNavAgent.enabled)
            {
                characterNavAgent.isStopped = true;
                isStopped = characterNavAgent.isStopped;
            }
        }
        #endregion
    }
}

