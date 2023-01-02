using Asset.Player.Controller;
using Asset.Player.Movement;
using System.DebugLogs;
using UnityEngine;

namespace Asset.Player.Combat
{
    public class CombatControllerScript : MonoBehaviour, IActionScript
    {
        #region Properties
        private Transform targetAgent;
        private float attackRange = 5f;
        private IsNullCheckScript IsNullCheck = new IsNullCheckScript();
        private PlayerMoveScript playerMove;
        private ActionSchedulerScript actionScheduler;
        private Animator animator;
        #endregion

        #region Awake
        private void Awake()
        {
            playerMove = this.GetComponent<PlayerMoveScript>();
            actionScheduler = this.GetComponent<ActionSchedulerScript>();
            animator = this.GetComponentInChildren<Animator>();
        }
        #endregion

        #region Update
        private void Update()
        {
            if (IsNullCheck.IsTransformNotEmpty(targetAgent))
            {
                bool inRange = Vector3.Distance(this.transform.position, targetAgent.position) < attackRange;
                //print("Vector A: " + this.transform.position + " / " + "Vector A: " + targetAgent.position);
                if (playerMove != null && !inRange)
                {
                    playerMove.MoveTowardsDestination(targetAgent.position);
                }                
                else
                {
                    playerMove.Cancel();
                }

                
            }
        }
        #endregion


        #region Attacks
        public void AttackCommand(CombatTargetScript combatTarget, SceneDebugLogScript debugLogEnabled = null)
        {

            if (debugLogEnabled != null && debugLogEnabled.debugCombatLog)
            {
                print("Player has attacked.");
            }

            if(actionScheduler != null)
            {
                actionScheduler.StartAction(this, debugLogEnabled);
            }

            targetAgent = combatTarget.transform;

            bool inRange = Vector3.Distance(this.transform.position, targetAgent.position) < attackRange;
            if (playerMove != null && inRange)
            {
                animator.SetTrigger("Attack Trigger");
            }
        }                

        public void Cancel()
        {
            targetAgent = null;
        }
        #endregion
    }
}


