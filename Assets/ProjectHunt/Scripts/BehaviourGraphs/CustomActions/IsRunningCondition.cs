using MadDuck.Scripts.Character.Module;
using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsRunning", story: "Agent have [Running]", category: "Conditions", id: "72ac9aaa440bc97bf00c16015d1ca2cc")]
public partial class IsRunningCondition : Condition
{
    [SerializeReference] public BlackboardVariable<CharacterMovementModule> Running;

    public override bool IsTrue()
    {
        return Running.Value.isRunning;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
