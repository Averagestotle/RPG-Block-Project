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
                currentAction.SwitchAction();

                if ((debugLogEnabled != null && debugLogEnabled.debugStateLog))
                {
                    Debug.Log("Starting action: " + action);
                }                
            };
            
            currentAction = action;
        }

        public void CancelAction(SceneDebugLogScript debugLogEnabled = null)
        {
            if (currentAction == null) return;

            if (currentAction != null)
            {
                if ((debugLogEnabled != null && debugLogEnabled.debugStateLog))
                {
                    Debug.Log("Canceling previous action: " + currentAction);
                }

                StartAction(null);
            }
        }
    }

}

