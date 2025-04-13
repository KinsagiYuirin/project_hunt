using System;
using MadDuck.Scripts.AI.ActionModule;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Target In range", story: "[Target] in range [yep]", category: "Action", id: "54779a28f531294fd4d19f2502061681")]
public partial class TargetInRangeAction : Action
{
    [SerializeReference] public BlackboardVariable<TrackTarget> Target;
    [SerializeReference] public BlackboardVariable<bool> Yep;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Yep.Value = Target.Value.InRangeAttack;
        return Yep.Value ? Status.Success : Status.Failure;
    }

    protected override void OnEnd()
    {
    }
}

