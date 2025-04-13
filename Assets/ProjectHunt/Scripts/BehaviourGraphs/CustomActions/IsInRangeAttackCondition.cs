using MadDuck.Scripts.AI.ActionModule;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsInRangeAttack", story: "[Is] in [attack] range?", category: "Conditions", id: "8e1c13b55627e753da064918e1a54613")]
public partial class IsInRangeAttackCondition : Condition
{
    [SerializeReference] public BlackboardVariable<TrackTarget> Is;
    [SerializeReference] public BlackboardVariable<bool> Attack;

    public override bool IsTrue()
    {
        return Is.Value.InRangeAttack;
    }

    public override void OnStart()
    {
        
    }

    public override void OnEnd()
    {
    }
}
