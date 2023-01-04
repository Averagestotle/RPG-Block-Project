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
        [SerializeField] float attackRange = 4.5f;
        [SerializeField] float attackDelay = 1.5f;
        private float timeSinceLastAttack = 0f;
        public bool canAttack = false;
        private IsNullCheckScript IsNullCheck = new IsNullCheckScript();
        private PlayerMoveScript playerMove;
        private ActionSchedulerScript actionScheduler;
        private Animator animator;
        
        public float debugDistanceToTarget;
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
            canAttack = CanAttack(canAttack);
            targetAgent = IsTargetInRange(targetAgent, canAttack);
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

            IsTargetInRange(targetAgent, canAttack);
        }

        private Transform IsTargetInRange(Transform targetTransform, bool canAttack)
        {
            if (IsNullCheck.IsTransformNotEmpty(targetTransform))
            {
                bool inRange = Vector3.Distance(this.transform.position, targetTransform.position) < attackRange;
                debugDistanceToTarget = DebugCalculateDistance(this.transform, targetTransform);
                if (playerMove != null && !inRange)
                {
                    playerMove.MoveTowardsDestination(targetTransform.position);
                } 
                else if (playerMove != null && inRange && canAttack)
                {
                    
                    RunAttackAnimation();
                    timeSinceLastAttack = 0f;
                    playerMove.Cancel();
                    //targetTransform = null;
                    debugDistanceToTarget = 0;
                }
                else
                {
                    playerMove.Cancel();                    
                    //targetTransform = null;
                    debugDistanceToTarget = 0;   
                }

                
            }
            return targetTransform;
        }
        
        private bool CanAttack(bool canAttack)
        {

            timeSinceLastAttack += Time.deltaTime;

            if(timeSinceLastAttack >= attackDelay)
            {
                canAttack = true;                
            } else
            {
                canAttack = false;
            }

            return canAttack;
        }

        public void Cancel()
        {
            targetAgent = null;
        }
        #endregion

        #region DebugFunctions
        private float DebugCalculateDistance(Transform transformA, Transform transformB)
        {
            float distance = 0;
            return distance = Vector3.Distance(transformA.position, transformB.position);          
        }
        #endregion

        #region Animation Functions
        public void Hit()
        {
            if (targetAgent == null) return;
            print("Hit!");
            CombatHealthScript combatHealth = targetAgent.GetComponentInParent<CombatHealthScript>();
            if (combatHealth != null)
            {
                combatHealth.SubractAgentsHealth(10f);
            }
        }

        private void RunAttackAnimation()
        {
            animator.SetTrigger("Attack Trigger");
            
            //Hit is also called when running additional code.
        }
        #endregion
    }
}