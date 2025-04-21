using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Subtract", story: "Subtract [int] -= [value]", category: "Action", id: "ad10ab2b0323873fafed9092ee6473a0")]
public partial class SubtractAction : Action
{
    [SerializeReference] public BlackboardVariable<int> Int;
    [SerializeReference] public BlackboardVariable<int> Value;

    protected override Status OnStart()
    {
        Int.Value -= Value.Value;
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

