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
        private CombatHealthScript combatHealth = new CombatHealthScript();
        private Animator animator;
        
        public float debugDistanceToTarget;
        #endregion

        #region Awake
        private void Awake()
        {
            playerMove = this.GetComponent<CharacterMoveScript>();
            combatHealth = this.GetComponent<CombatHealthScript>();
            actionScheduler = this.GetComponent<ActionSchedulerScript>();
            animator = this.GetComponentInChildren<Animator>();
        }
        #endregion

        #region Update
        private void Update()
        {
            if (combatHealth.CheckIfDead()) { return; }
            canAttack = CanAttack(canAttack);
            IsTargetInRange(targetAgent, canAttack);
        }        
        #endregion


        #region Attacks
        public void AttackCommand(CombatTargetScript combatTarget, GameObject debugGameObject = null, SceneDebugLogScript debugLogEnabled = null)
        {
            //if(targetAgent == null || IsCharacterDead()) { return; }

            if (debugLogEnabled != null && debugLogEnabled.debugCombatLog && debugGameObject != null)
            {
                Debug.Log(debugGameObject.gameObject.name + " : Has attacked.");
            }

            if(actionScheduler != null)
            {
                actionScheduler.StartAction(this, debugLogEnabled);
            }

            targetAgent = combatTarget.transform;
            IsTargetInRange(targetAgent, canAttack);
        }
    
        public bool IsTargetCharacterDead()
        {
            CombatHealthScript combatHealth = null;

            if (targetAgent != null) 
            {
                combatHealth = targetAgent.GetComponentInParent<CombatHealthScript>();
            }

            if (combatHealth != null && combatHealth.CheckIfDead())
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

        #region Attack Checks
        public float GetAttackRange()
        {
            return attackRange;
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
                else if (playerMove != null && inRange && canAttack && !IsTargetCharacterDead())
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

            if (timeSinceLastAttack >= attackDelay)
            {
                canAttack = true;
            }
            else
            {
                canAttack = false;
            }

            return canAttack;
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
            if (!IsTargetCharacterDead())
            {
                if (targetAgent == null) return;
                print("Hit!");                
                targetAgent.GetComponentInParent<CombatHealthScript>().SubractAgentsHealth(attackDamage);
                SwitchAction();
            }         
        }

        private void RunAttackAnimation(Transform targetTransform)
        {
            if(!IsTargetCharacterDead())
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