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
        [SerializeField] float attackDamage = 10f;
        [SerializeField] float attackRange = 4.5f;
        [SerializeField] float attackDelay = 1.5f;
        public float timeSinceLastAttack = 0f;
        public bool canAttack = false;
        private IsNullCheckScript IsNullCheck = new IsNullCheckScript();
        private CharacterMoveScript playerMove;
        private ActionSchedulerScript actionScheduler;
        private Animator animator;
        
        public float debugDistanceToTarget;
        #endregion

        #region Awake
        private void Awake()
        {
            playerMove = this.GetComponent<CharacterMoveScript>();
            actionScheduler = this.GetComponent<ActionSchedulerScript>();
            animator = this.GetComponentInChildren<Animator>();
        }
        #endregion

        #region Update
        private void Update()
        {
            canAttack = CanAttack(canAttack);
            IsTargetInRange(targetAgent, canAttack);
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

        private void IsTargetInRange(Transform targetTransform, bool canAttack)
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
                    RunAttackAnimation(targetTransform);
                    playerMove.SwitchAction();
                    ResetAttack();
                }
                else
                {
                    debugDistanceToTarget = 0;
                    playerMove.SwitchAction();
                }            
            }
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

        private bool IsCharacterDead()
        {
            if (targetAgent == null) return false;

            CombatHealthScript combatHealth = targetAgent.GetComponentInParent<CombatHealthScript>();

            if (combatHealth != null && combatHealth.isDeadAlready)
            {
                return true;
            }

                return false;
        }

        private void ResetAttack()
        {           
            timeSinceLastAttack = 0;
            canAttack = false;
        }        
        #endregion

        #region DebugFunctions
        private float DebugCalculateDistance(Transform transformA, Transform transformB)
        {
            float distance = 0;
            return distance = Mathf.Max(Vector3.Distance(transformA.position, transformB.position), 0);          
        }
        #endregion

        #region Animation Functions
        public void Hit()
        {
            if (!IsCharacterDead())
            {
                print("Hit!");
                targetAgent.GetComponentInParent<CombatHealthScript>().SubractAgentsHealth(attackDamage);
                SwitchAction();
            }         
        }

        private void RunAttackAnimation(Transform targetTransform)
        {
            if(!IsCharacterDead())
            {
                this.transform.LookAt(targetTransform.position); //Look at the target's position
                animator.SetTrigger("Attack Trigger");
            }
            //Hit is also called when running additional code.
        }
        #endregion

        #region Action
        public void SwitchAction()
        {
            targetAgent = null;
        }
        #endregion
    }
}