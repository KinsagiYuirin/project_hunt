using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "If Value is ", story: "If [int] >= [value]", category: "Conditions", id: "835d8b444d763fba5dd9d311417e9a02")]
public partial class IfValueIsCondition : Condition
{
    [SerializeReference] public BlackboardVariable<int> Int;
    [SerializeReference] public BlackboardVariable<int> Value;

    public override bool IsTrue()
    {
        return Int.Value >= Value.Value;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
