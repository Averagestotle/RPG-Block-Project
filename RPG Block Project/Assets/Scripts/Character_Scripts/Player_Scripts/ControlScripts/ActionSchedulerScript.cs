using System.Collections;
using System.Collections.Generic;
using System.DebugLogs;
using UnityEngine;

namespace Asset.Player.Controller
{
    public class ActionSchedulerScript : MonoBehaviour
    {
        IActionScript currentAction;

        public void StartAction(IActionScript action, SceneDebugLogScript debugLogEnabled = null)
        {
            if (currentAction == action) return;

            if (currentAction != null) 
            {
                currentAction.Cancel();

                if ((debugLogEnabled != null && debugLogEnabled.debugStateLog))
                {
                    Debug.Log("Canceling previous action: " + currentAction);
                }
                
            };
            
            currentAction = action;
        }
    }

}

