using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asset.Player.Combat
{
    public class CombatHealthScript : MonoBehaviour
    {
        #region Properties
        [SerializeField] public float combatHealth = 100f;
        private Animator animator;
        public bool isDeadAlready = false;
        #endregion

        #region Awake
        private void Awake()
        {
            animator = this.GetComponentInChildren<Animator>();
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
        }
        #endregion
    }
}

