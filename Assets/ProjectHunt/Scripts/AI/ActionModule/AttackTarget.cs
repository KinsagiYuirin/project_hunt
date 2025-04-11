using MadDuck.Scripts.Character.Module;
using TriInspector;
using UnityEngine;


namespace MadDuck.Scripts.AI.ActionModule
{
    public class AttackTarget : CharacterModule
    {
        [Title("References")]
        [SerializeField] private CharacterBasicAttackModule attackModule;
        [SerializeField] private TrackTarget trackTarget;

        private void AttackCondition()
        {
            if (!trackTarget.InRangeAttack) return;
            
            /*var direction = ;
            attackModule.Value.SetAttackDirection(direction);*/
            attackModule.Attack();
        }
    }
}
