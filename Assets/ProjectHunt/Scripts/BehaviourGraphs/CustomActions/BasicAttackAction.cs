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
    [SerializeReference] public BlackboardVariable<Transform> Target;

    private Vector3 _direction;
    
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Target.Value == null)
        {
            Debug.LogWarning("Target หรือ Target.target เป็น null");
            return Status.Failure;
        }
    
        if (Agent.Value == null)
        {
            Debug.LogWarning("Agent เป็น null");
            return Status.Failure;
        }
    
        if (Attack.Value == null)
        {
            Debug.LogWarning("Attack module เป็น null");
            return Status.Failure;
        }

        var direction = Target.Value.position - Agent.Value.transform.position;
        Attack.Value.SetAttackDirection(direction);
        Attack.Value.Attack();
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

