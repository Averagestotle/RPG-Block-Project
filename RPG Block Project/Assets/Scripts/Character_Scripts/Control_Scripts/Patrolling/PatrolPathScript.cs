using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asset.Player.Controller
{
    public class PatrolPathScript : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            DrawAllWaypoints();
        }

        private void DrawAllWaypoints()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextWaypoint(i);


                Gizmos.DrawSphere(GetWaypoint(i), 0.2f);

                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));                  
            }
        }

        public int GetNextWaypoint(int i)
        {
            if (i + 1 == transform.childCount)
            {
                return 0;
            }
            else
            {
                return i + 1;
            }            
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}

