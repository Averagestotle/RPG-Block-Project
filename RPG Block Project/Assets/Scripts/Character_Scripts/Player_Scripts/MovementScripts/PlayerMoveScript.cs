//using Asset.Player.Combat;
using Asset.Player.Controller;
using System.DebugLogs;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;

namespace Asset.Player.Movement
{
    public class PlayerMoveScript : MonoBehaviour, IActionScript
    {
        #region Properties
        private GameObject player;
        private NavMeshAgent playerNavAgent;
        //private CombatControllerScript combatController;
        private ActionSchedulerScript actionScheduler;
        private IsNullCheckScript IsNullCheck = new IsNullCheckScript();
        //private IActionScript actionScript;
        public bool isStopped;
        #endregion

        #region Start
        // Start is called before the first frame update
        void Start()
            {
                player = this.gameObject;
                if (IsNullCheck.IsGameObjectNotEmpty(player))
                {
                    playerNavAgent = player.GetComponent<NavMeshAgent>();
                    //combatController = player.GetComponent<CombatControllerScript>();
                    actionScheduler = this.GetComponent<ActionSchedulerScript>();
                    //actionScript = (IActionScript)actionScheduler;
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
            //combatController.CancelTarget();
            MoveTowardsDestination(pos);
        }

        public void MoveTowardsDestination(Vector3 pos)
        {
            if (playerNavAgent != null)
            {
                playerNavAgent.destination = pos;
                playerNavAgent.isStopped = false;
                isStopped = playerNavAgent.isStopped;                 
            } else
            {
                print("Agent cannot move: No navMesh assigned.");
            }
        }

        public void MovementStopped()
        {
            if (playerNavAgent != null)
            {
                playerNavAgent.isStopped = true;
                isStopped = playerNavAgent.isStopped;
            }
        }
        #endregion

        #region IAction Interface
        public void SwitchAction() 
        {
            if (playerNavAgent != null)
            {
                playerNavAgent.isStopped = true;
                isStopped = playerNavAgent.isStopped;
            }
        }
        #endregion
    }
}

