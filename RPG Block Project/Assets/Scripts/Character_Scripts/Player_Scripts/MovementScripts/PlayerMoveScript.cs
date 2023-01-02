using Asset.Player.Combat;
using Asset.Player.Controller;
using UnityEngine;
using UnityEngine.AI;

namespace Asset.Player.Movement
{
    public class PlayerMoveScript : MonoBehaviour
    {
        #region Properties
        private GameObject player;
        private NavMeshAgent playerNavAgent;
        private CombatControllerScript combatController;
        private ActionSchedulerScript actionScheduler;
        private IsNullCheckScript IsNullCheck = new IsNullCheckScript();
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
                    combatController = player.GetComponent<CombatControllerScript>();
                    actionScheduler = this.GetComponent<ActionSchedulerScript>();
                }
            }
        #endregion

        #region Movement
        public void StartMovementAction(Vector3 pos)
        {
            

            if (actionScheduler != null)
            {
                actionScheduler.StartAction(this);
            }
            combatController.CancelTarget();
            MoveTowardsDestination(pos);
        }

        public void MoveTowardsDestination(Vector3 pos)
        {
            if (playerNavAgent != null)
            {
                //if (Input.GetMouseButton(0))
                {
                    //if (actionScheduler != null)
                    //{
                    //    actionScheduler.StartAction(this);
                    //}
                    playerNavAgent.destination = pos;
                    playerNavAgent.isStopped = false;
                    isStopped = playerNavAgent.isStopped;
                }                    
            }
            else
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
    }
}

