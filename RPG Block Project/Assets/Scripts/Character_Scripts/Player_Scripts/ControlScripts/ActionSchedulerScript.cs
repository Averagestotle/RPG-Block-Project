using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asset.Player.Controller
{
    public class ActionSchedulerScript : MonoBehaviour
    {
        MonoBehaviour currentAction;

        public void StartAction(MonoBehaviour action)
        {
            if (currentAction == action) return;
            if (currentAction != null) 
            {
                Debug.Log("Canceling previous action: " + currentAction);
            };
            
            currentAction = action;
        }
    }

}

