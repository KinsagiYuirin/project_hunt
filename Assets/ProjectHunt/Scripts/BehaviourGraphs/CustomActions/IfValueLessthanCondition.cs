using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "If Value lessthan", story: "If [int] <= [value]", category: "Conditions", id: "8ec03f32c91fb7b25a955fca99e54730")]
public partial class IfValueLessthanCondition : Condition
{
    [SerializeReference] public BlackboardVariable<int> Int;
    [SerializeReference] public BlackboardVariable<int> Value;

    public override bool IsTrue()
    {
        return Int.Value <= Value.Value;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
