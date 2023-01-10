using Asset.Player.Controller;
using System.Collections;
using System.Collections.Generic;
using System.DebugLogs;
using UnityEngine;
using UnityEngine.AI;

namespace Asset.Player.Combat
{
    public class CombatHealthScript : MonoBehaviour
    {
        #region Properties
        [SerializeField] public float combatHealth = 100f;
        private Animator animator;
        private bool isDeadAlready = false;
        private SceneDebugLogScript sceneDebugLog = new SceneDebugLogScript();
        #endregion

        #region Awake
        private void Awake()
        {
            animator = this.GetComponentInChildren<Animator>();
            sceneDebugLog = FindObjectOfType<SceneDebugLogScript>();
        }
        #endregion

        #region Update
        private void Update()
        {
            if (animator != null && combatHealth <= 0 && !isDeadAlready) 
            {
                CharacterDeath();
                isDeadAlready = true;
            }
        }
        #endregion

        #region Combat Functions
        public void SubractAgentsHealth(float damage)
        {

            combatHealth = Mathf.Max(combatHealth - damage, 0);
            print("Health: " + combatHealth);
        }
        #endregion

        #region Death Functions
        private void CharacterDeath()
        {
            animator.SetTrigger("Is Dead");
            this.GetComponentInChildren<BoxCollider>().enabled = false;
            this.GetComponentInChildren<NavMeshAgent>().enabled= false;
            this.GetComponent<ActionSchedulerScript>().CancelAction(sceneDebugLog);
        }

        public bool CheckIfDead() 
        {
            return isDeadAlready;
        }
        #endregion
    }
}

