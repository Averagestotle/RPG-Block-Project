using Asset.Player.Controller;
using Asset.Player.Movement;
using UnityEngine;

namespace Asset.Player.Combat
{
    public class CombatControllerScript : MonoBehaviour
    {
        #region Properties
        private Transform targetAgent;
        private float attackRange = 5f;
        private IsNullCheckScript IsNullCheck = new IsNullCheckScript();
        private PlayerMoveScript playerMove;
        private ActionSchedulerScript actionScheduler;
        #endregion

        #region Awake
        private void Awake()
        {
            playerMove = this.GetComponent<PlayerMoveScript>();
            actionScheduler= this.GetComponent<ActionSchedulerScript>();
        }
        #endregion

        #region Update
        private void Update()
        {
            if(IsNullCheck.IsTransformNotEmpty(targetAgent))
            {
                bool inRange = Vector3.Distance(this.transform.position, targetAgent.position) < attackRange;
                //print("Vector A: " + this.transform.position + " / " + "Vector A: " + targetAgent.position);
                if(playerMove != null && !inRange)
                {

                    playerMove.MoveTowardsDestination(targetAgent.position);
                    //playerMove.StartMovementAction(targetAgent.position);
                } else
                {
                    playerMove.MovementStopped();
                }      
            }
        }
        #endregion


        #region Attacks
        public void AttackCommand(CombatTargetScript combatTarget, bool DebugCombatLogEnabled = false)
        {
            
            if (Input.GetMouseButton(0))
            {
                if (DebugCombatLogEnabled)
                {
                    print("Player has attacked.");
                }
                if(actionScheduler != null)
                {
                    actionScheduler.StartAction(this);
                }
                targetAgent = combatTarget.transform;
            }                
        }

        public void CancelTarget()
        {
            targetAgent = null;
        }
        #endregion
    }
}


