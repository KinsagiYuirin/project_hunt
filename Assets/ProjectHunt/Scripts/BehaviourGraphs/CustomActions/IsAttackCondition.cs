using MadDuck.Scripts.AI.ActionModule;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsAttack", story: "Is [Attack]", category: "Conditions", id: "ce9bd59bc87e4413ac796c0e1495b80b")]
public partial class IsAttackCondition : Condition
{
    [SerializeReference] public BlackboardVariable<TrackTarget> Attack;

    public override bool IsTrue()
    {
        if (Attack.Value.InRangeAttack)
        {
            return true;
        }
        return false;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
