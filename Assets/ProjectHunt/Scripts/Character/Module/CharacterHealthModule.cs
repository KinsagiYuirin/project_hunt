using System;
using MadDuck.Scripts.Manangers;
using MadDuck.Scripts.Utils;
using TriInspector;
using UnityEngine;

namespace MadDuck.Scripts.Character.Module
{
    [Serializable]
    public record HealthData
    {
        public float currentHealth;
        public float maxHealth;
        public bool invincible;
    }
    /// <summary>
    /// Module responsible for handling character health.
    /// </summary>
    public class CharacterHealthModule : CharacterModule
    {
        [Title("Health Settings")] 
        [SerializeField] public bool iFrame;
        [SerializeField] private HealthData healthData = new HealthData();
        public HealthData pHealthData => healthData;
        [SerializeField] private float bumpThreshold = 10f;
        [SerializeField] private bool useMMHealthBar = true;
        
        private YuirinHealthBar healthBar;
        
        [SerializeField] private GameObject healthScreenUI;
        
        [SerializeField] private Animator deadAnimator;
        
        /*
        [SerializeField, ShowIf(nameof(useMMHealthBar))] 
        private MMHealthBar healthBar;
        */

        [Title("Debug")] 
        [SerializeField] 
        private float testAmount;
        [Button("Test Change Health")] 
        private void TestChangeHealth() => ChangeHealth(testAmount);
        private float _previousChange;

        private void Start()
        {
            healthData.currentHealth = healthData.maxHealth;
            iFrame = false;
        }

        private void OnHealthDataChanged(HealthData previousvalue, HealthData newvalue)
        {
            _previousChange = newvalue.currentHealth - previousvalue.currentHealth;
            //UpdateHealthBar();
        }

        public virtual void ChangeHealth(float amount)
        {
            if (iFrame) return;
            if (!ModulePermitted) return;
            if (healthData.invincible) return;
            _previousChange = amount;
            healthData.currentHealth += amount;
            healthData.currentHealth = Mathf.Clamp(healthData.currentHealth, 0, healthData.maxHealth);
            if (healthData.currentHealth <= 0)
            {
                Die();
            }
            //UpdateHealthBar();
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
                        
                        /*
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
        
        protected virtual void Die()
        {
            if (!ModulePermitted) return;
            characterHub.ChangeConditionState(CharacterConditionState.Dead);
        }

        protected override void UpdateAnimator()
        {
            base.UpdateAnimator();
            if (deadAnimator == null) return;
            if (characterHub.ConditionState == CharacterConditionState.Dead)
            {
                deadAnimator.SetBool("IsDead", true);
            }
            else
            {
                deadAnimator.SetBool("IsDead", false);
            }
        }
    }
}
