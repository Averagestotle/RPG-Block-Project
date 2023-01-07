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
        public bool isStopped;
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

        public void MoveTowardsDestination(Vector3 pos)
        {
            if (characterNavAgent != null)
            {
                characterNavAgent.destination = pos;
                characterNavAgent.isStopped = false;
                isStopped = characterNavAgent.isStopped;                 
            } else
            {
                print("Agent cannot move: No navMesh assigned.");
            }
        }

        public void MovementStopped()
        {
            if (characterNavAgent != null)
            {
                characterNavAgent.isStopped = true;
                isStopped = characterNavAgent.isStopped;
            }
        }
        #endregion

        #region IAction Interface
        public void SwitchAction() 
        {
            if (characterNavAgent != null)
            {
                characterNavAgent.isStopped = true;
                isStopped = characterNavAgent.isStopped;
            }
        }
        #endregion
    }
}

