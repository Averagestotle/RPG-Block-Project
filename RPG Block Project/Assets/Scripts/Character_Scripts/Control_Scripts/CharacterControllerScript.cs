using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asset.Player.Controller
{
    using Asset.Player.Combat;
    using Asset.Player.Movement;

    public class CharacterControllerScript : MonoBehaviour
    {
        #region Properties
        public LayerMask navigationLayerMask;
        public LayerMask combatLayerMask;

        private CharacterMoveScript playerMove = new CharacterMoveScript();
        private CombatControllerScript combatController = new CombatControllerScript();
        private IsNullCheckScript IsNullCheck = new IsNullCheckScript();
        #endregion

        #region Start
        // Start is called before the first frame update
        void Start()
        {
            playerMove = this.GetComponent<CharacterMoveScript>();
            combatController = this.GetComponent<CombatControllerScript>();
        }
        #endregion

        #region Update
        #endregion

        #region LateUpdate
        #endregion

        #region Movement Functions
        #endregion

        #region Combat Functions
        #endregion

        #region Idle Functions
        #endregion
    }

}