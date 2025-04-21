using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Add value", story: "Set [int] += [value]", category: "Action", id: "9e9db28fb0aa457ad0a3dd9f83ad493c")]
public partial class AddValueAction : Action
{
    [SerializeReference] public BlackboardVariable<int> Int;
    [SerializeReference] public BlackboardVariable<int> Value;

    protected override Status OnStart()
    {
        Int.Value += Value.Value;
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

