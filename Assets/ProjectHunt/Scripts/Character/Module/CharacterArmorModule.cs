using System;
using System.Collections;
using MadDuck.Scripts.Manangers;
using MadDuck.Scripts.Utils;
using TriInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MadDuck.Scripts.Character.Module
{
    [Serializable]
    public record ArmorData
    {
        public float currentArmor;
        public float maxArmor;
        public bool invincible;
    }
    /// <summary>
    /// Module responsible for handling character health.
    /// </summary>
    public class CharacterArmorModule : CharacterModule, IDamageable
    {
        [Title("Health Settings")] 
        [SerializeField] private DamageType receiveDamageType;
        [SerializeField] private DamageType receiveHaftDamageType;
        [SerializeField] private ArmorData armorData = new ArmorData();
        public ArmorData PArmorData => armorData;
        [SerializeField] private float bumpThreshold = 10f;
        [SerializeField] private bool useHealthBar = true;
        
        [SerializeField] private YuirinHealthBar yuirinHealthBar;
        public YuirinHealthBar YuirinHealthBar => yuirinHealthBar;
        
        [SerializeField] private GameObject armorScreenUI;
        [SerializeField] private GameObject characterObject;
        
        [SerializeField] private bool haveArmor;
        public bool HaveArmor => haveArmor;
        
        [SerializeField] private Animator animation;
        
        /*
        [SerializeField, ShowIf(nameof(useMMHealthBar))] 
        private MMHealthBar healthBar;
        */

        [Title("Debug")] 
        [SerializeField] 
        private float testAmount;
        [Button("Test Change Armor")] 
        private void TestChangeArmor() => ChangeArmor(testAmount);
        private float _previousChange;

        private void Start()
        {
            GetArmor();
            if (yuirinHealthBar != null)
            {
                yuirinHealthBar.CurrentHealth = armorData.maxArmor;
            }
        }

        public void GetArmor()
        {
            armorData.currentArmor = armorData.maxArmor;
            haveArmor = true;
            //characterHub.ChangeConditionState(CharacterConditionState.Armor);
        }
        
        private void OnHealthDataChanged(HealthData previousvalue, HealthData newvalue)
        {
            _previousChange = newvalue.currentHealth - previousvalue.currentHealth;
            //UpdateHealthBar();
        }

        public void ReceiveDamage(float amount, DamageData data)
        {
            if (data.type == receiveDamageType)
            {
                if (!haveArmor) return;
                ChangeArmor(amount);
            }
            else if (data.type == receiveHaftDamageType)
            {
                if (!haveArmor) return;
                ChangeArmor(-amount / 2);
            }
        }

        public virtual void ChangeArmor(float amount)
        {
            if (!ModulePermitted) return;
            if (armorData.invincible) return;
            //if (characterHub.ConditionState != CharacterConditionState.Armor) return;
            
            _previousChange = amount;
            armorData.currentArmor += amount;
            armorData.currentArmor = Mathf.Clamp(armorData.currentArmor, 0, armorData.maxArmor);
            if (armorData.currentArmor <= 0)
            {
                IsArmorBroken();
            }
            UpdateHealthBar();
        }

        public void UpdateHealthBar()
        {
            if (yuirinHealthBar == null) return;
            if (!useHealthBar) return;
            
            yuirinHealthBar.UpdateHealthUI(armorData.currentArmor, armorData.maxArmor);
        }
        
        /*
        public virtual void UpdateHealthBar()
        {
            if (characterHub.CharacterType is CharacterType.Player) return;
            if (!ModulePermitted) return;
            if (useMMHealthBar)
            {
                bool healthBarExists = healthBar;
                var currentHealth = healthData.currentHealth;
                var maxHealth = healthData.maxHealth;
                var shouldBump = Mathf.Abs(_previousChange) >= bumpThreshold;
                switch (healthBarExists)
                {
                    case true:
                        
                        var progressTracker = healthBar.ProgressTracker;
                        progressTracker.BumpScaleOnChange = shouldBump;
                        progressTracker.LerpForegroundBar = shouldBump;
                        progressTracker.LerpDecreasingDelayedBar = shouldBump;
                        progressTracker.LerpIncreasingDelayedBar = shouldBump;
                        #1#
                        
                        healthBar.UpdateBar(currentHealth, 0, maxHealth, true);
                        break;
                    case false when characterHub.CharacterType is CharacterType.Player:
                        PlayerCanvasManager.Instance.UpdateHealthBar(currentHealth, maxHealth, shouldBump);
                        break;
                    default:
                        Debug.LogWarning("Character is NPC but has no health bar.");
                        break;
                }
            }
            else
            {
                //other health bar update logic
                
            }
        }
        */
        
        protected virtual void IsArmorBroken()
        {
            if (!ModulePermitted) return;
            //characterHub.ChangeConditionState(CharacterConditionState.Normal);
            StartCoroutine(YuirinHealthBar.DrainSmoothly());
            haveArmor = false;
        }

        protected override void UpdateAnimator()
        {
            base.UpdateAnimator();
            if (animation == null) return;
            //animation.SetBool("IsDead", characterHub.ConditionState == CharacterConditionState.Dead);
        }
    }
}
