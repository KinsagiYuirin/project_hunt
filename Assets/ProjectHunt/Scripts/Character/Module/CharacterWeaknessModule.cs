using System;
using TriInspector;
using UnityEngine;

namespace MadDuck.Scripts.Character.Module
{
    [Serializable]
    public struct weaknessData
    {
        [Group("Area"), Required] public WeaknessArea weaknessArea;
        [Group("Durability"), Min(0)] public float durability;
        [Group("Timing"), Min(0)] public float duration;
    }
    public class CharacterWeaknessModule : MonoBehaviour
    {
        
    }
}

