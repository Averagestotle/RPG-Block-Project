using UnityEngine;
using UnityEngine.AI;

namespace Asset.Player.Movement
{
    public class PlayerMoveScript : MonoBehaviour
    {
        #region Properties
            private GameObject player;
            private NavMeshAgent playerNavAgent;
            private IsNullCheckScript IsNullCheck = new IsNullCheckScript();
        #endregion

        #region Start
        // Start is called before the first frame update
        void Start()
            {
                player = this.gameObject;
                if (IsNullCheck.IsGameObjectEmpty(player))
                {
                    playerNavAgent = player.GetComponent<NavMeshAgent>();
                }
            }
        #endregion

        #region Movement
        public void MoveTowardsDestination(Vector3 pos)
            {
                if (playerNavAgent != null)
                {
                    playerNavAgent.destination = pos;
                }
                else
                {
                    print("Agent cannot move: No navMesh assigned.");
                }
            }
        #endregion
    }
}

