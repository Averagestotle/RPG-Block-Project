using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asset.Player.Combat
{
    public class CombatHealthScript : MonoBehaviour
    {
        #region Properties
        [SerializeField] public float combatHealth = 100f;
        #endregion

        #region Combat Functions
        public void SubractAgentsHealth(float damage)
        {

            combatHealth = Mathf.Max(combatHealth - damage, 0);
            print("Health: " + combatHealth);
        }
        #endregion
    }
}

