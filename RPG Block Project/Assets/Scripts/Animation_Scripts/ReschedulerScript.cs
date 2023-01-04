using Asset.Player.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReschedulerScript : MonoBehaviour
{
    // Either drag in via Inspector
    [SerializeField] private CombatControllerScript _scriptOnOtherObject;

    // or get at runtime if you are always sure about the hierachy
    private void Awake()
    {
        _scriptOnOtherObject = this.transform.GetComponentInParent<CombatControllerScript>();
    }

    // and now call this from the AnimationEvent
    public void RescheduleAttack()
    {
        _scriptOnOtherObject.Hit();
    }
}
