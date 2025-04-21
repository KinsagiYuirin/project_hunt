using System;
using MadDuck.Scripts.Character.Module;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set False", story: "Set [bool] [false]", category: "Action", id: "0c0d4831f57308fd3d07540a8314f575")]
public partial class SetFalseAction : Action
{
    [SerializeReference] public BlackboardVariable<CharacterMovementModule> Bool;
    [SerializeReference] public BlackboardVariable<bool> False;

    protected override Status OnStart()
    {
        Bool.Value.isRunning = False.Value;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

