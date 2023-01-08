using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asset.Player.Controller
{
    using Asset.Player.Combat;
    using Asset.Player.Movement;
    using System.DebugLogs;

    public class CharacterControllerScript : MonoBehaviour
    {
        #region Properties
        public LayerMask navigationLayerMask;
        public LayerMask combatLayerMask;

        private CharacterMoveScript characterMove = new CharacterMoveScript();
        private CombatControllerScript combatController = new CombatControllerScript();
        private IsNullCheckScript IsNullCheck = new IsNullCheckScript();
        private RaycastAgentByComponent raycastAgent = new RaycastAgentByComponent();
        private SceneDebugLogScript sceneDebugLog = new SceneDebugLogScript();
        private GameObject targetAgent = null;
        #endregion

        #region Start
        // Start is called before the first frame update
        void Start()
        {
            characterMove = this.GetComponent<CharacterMoveScript>();
            combatController = this.GetComponent<CombatControllerScript>();
            sceneDebugLog = FindObjectOfType<SceneDebugLogScript>();
            targetAgent = GameObject.FindGameObjectWithTag("Player");
        }
        #endregion

        #region Update
        private void Update()
        {
            if (combatController.IsCharacterDead()) { return; }
            if (MoveToAgent()) { return; }
        }
        #endregion

        #region LateUpdate
        #endregion

        #region Movement Functions
        private bool MoveToAgent()
        {
            Vector3 destination = new Vector3();
            Vector3 origin = this.gameObject.transform.position;
            bool inRange = false;
            bool tooClose = false;

            //targetTransform = raycastAgent.FindPlayerByLayerMaskAndComponent(this.transform.position, combatLayerMask, 10f, sceneDebugLog);

            if (!IsNullCheck.IsGameObjectNotEmpty(targetAgent, sceneDebugLog.debugNullValues))
            {
                return false;               
            }

            destination = targetAgent.transform.position;
            inRange = characterMove.IsAgentInRange(origin, destination, sceneDebugLog);
            tooClose = characterMove.IsAgentTooClose(origin, destination, 4f, sceneDebugLog);

            if (destination != new Vector3() && inRange && !tooClose)
            {
                characterMove.StartMovementAction(destination, sceneDebugLog);
                return true;         
            }
            else
            {
                characterMove.MovementStopped();
                return false;
            }
        }
        #endregion

        #region Combat Functions
        #endregion

        #region Idle Functions
        #endregion
    }

}