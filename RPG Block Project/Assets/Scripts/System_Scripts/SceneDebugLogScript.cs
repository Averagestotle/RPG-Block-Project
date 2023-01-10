using Unity.VisualScripting;
using UnityEngine;

namespace System.DebugLogs
{
    public class SceneDebugLogScript : MonoBehaviour
    {
        #region Properties
        public bool debugNullValues = false;
        public bool debugLayerMaskValues = false;
        public bool debugColliderObject = false;
        public bool debugStateLog = false;
        public bool debugCombatLog = false;
        public bool debugCharacterMovementLog = false;
        #endregion

        #region Gizmos Functions
        public void DebugFindAgentsDestination(Vector3 origin, Vector3 destination, Color color)
        {
            if (destination != new Vector3() && origin != destination)
            {
                Gizmos.color = color;
                Gizmos.DrawLine(origin, destination);
            }
        }

        public void DebugDrawAgentsChaseRange(Vector3 position, float range, Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawWireSphere(position, range);
        }

        public void DebugDrawAgentsAttackRange(Vector3 position, float range, Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawWireSphere(position, range);
        }
        #endregion
    }


}

