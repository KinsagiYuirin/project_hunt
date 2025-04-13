using MadDuck.Scripts.Character.Module;
using System;
using MadDuck.Scripts.AI.ActionModule;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Basic Attack", story: "[Agent] get [Attack] to [Target]", category: "Action", id: "0666d24f52fae9c430e7b4a745deb1f5")]
public partial class BasicAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<CharacterBasicAttackModule> Attack;
    [SerializeReference] public BlackboardVariable<TrackTarget> Target;
    
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        var Direction = Target.Value.target.position - Agent.Value.transform.position;
        Attack.Value.SetAttackDirection(Direction);
        Attack.Value.Attack();
        return Status.Success;  
    }

    protected override void OnEnd()
    {
    }
}

