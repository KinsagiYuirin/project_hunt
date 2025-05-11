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
    public record HealthData
    {
        public float currentHealth;
        public float maxHealth;
        public bool invincible;
    }
    /// <summary>
    /// Module responsible for handling character health.
    /// </summary>
    public class CharacterHealthModule : CharacterModule, IDamageable
    {
        [Title("Health Settings")] 
        [SerializeField] private DamageType receiveDamageType;
        [SerializeField] private HealthData healthData = new HealthData();
        public HealthData pHealthData => healthData;
        [SerializeField] private float bumpThreshold = 10f;
        [SerializeField] private bool useHealthBar = true;
        
        [SerializeField] private YuirinHealthBar yuirinHealthBar;
        public YuirinHealthBar YuirinHealthBar => yuirinHealthBar;
        
        [SerializeField] private GameObject healthScreenUI;
        [SerializeField] private GameObject characterObject;
        
        [SerializeField] private Animator deadAnimator;
        
        [SerializeField] private SpriteRenderer spriteImage;
        [SerializeField] private Color _redColor = Color.red;
        [SerializeField] private Color _whiteColor = Color.white;
        
        [Title("Armor Settings")]
        [SerializeField] private bool haveArmor;
        [SerializeField, ShowIf("haveArmor")] private CharacterArmorModule armorModule;
        
        /*
        [SerializeField, ShowIf(nameof(useMMHealthBar))] 
        private MMHealthBar healthBar;
        */

        [Title("Debug")] 
        [SerializeField, DisplayAsString] private bool iFrame;
        public bool IFrame {get => iFrame; set => iFrame = value; }
        
        [SerializeField] private float testAmount;
        [Button("Test Change Health")] 
        private void TestChangeHealth() => ChangeHealth(testAmount);
        private float _previousChange;

        private void Start()
        {
            healthData.currentHealth = healthData.maxHealth;
            iFrame = false;
            if (yuirinHealthBar != null)
            {
                yuirinHealthBar.CurrentHealth = healthData.maxHealth;
            }
        }

        private void OnHealthDataChanged(HealthData previousvalue, HealthData newvalue)
        {
            _previousChange = newvalue.currentHealth - previousvalue.currentHealth;
            //UpdateHealthBar();
        }
        
        public void ReceiveDamage(float amount, DamageData data)
        {
            if (data.type != receiveDamageType)
            {
                ChangeHealth(amount);
            }
        }

        public virtual void ChangeHealth(float amount)
        {
            if (iFrame) return;
            if (!ModulePermitted) return;
            if (healthData.invincible) return;
            //if (characterHub.ConditionState == CharacterConditionState.Armor) return;
            if (haveArmor)
                if (armorModule.HaveArmor)
                    return;
            
            _previousChange = amount;
            healthData.currentHealth += amount;
            healthData.currentHealth = Mathf.Clamp(healthData.currentHealth, 0, healthData.maxHealth);
            if (healthData.currentHealth <= 0)
            {
                Die();
            }
            if (amount < 0) // โดนดาเมจ
            {
                StartCoroutine(FlashRed());
            }
            UpdateHealthBar();
        }

        public void UpdateHealthBar()
        {
            if (yuirinHealthBar == null) return;
            if (!useHealthBar) return;
            
            yuirinHealthBar.UpdateHealthUI(healthData.currentHealth, healthData.maxHealth);
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
        
        protected virtual void Die()
        {
            if (!ModulePermitted) return;
            characterHub.ChangeConditionState(CharacterConditionState.Dead);
            characterObject.layer = LayerMask.NameToLayer("Dead");
            characterObject.GetComponent<Collider2D>().enabled = false;
            characterObject.GetComponent<Rigidbody2D>().simulated = false;
            StartCoroutine(YuirinHealthBar.DrainSmoothly());
        }
        
        private IEnumerator FlashRed()
        {
            spriteImage.color = _redColor;
            yield return new WaitForSeconds(0.1f);
            spriteImage.color = _whiteColor;
        }

        protected override void UpdateAnimator()
        {
            base.UpdateAnimator();
            if (deadAnimator == null) return;
            deadAnimator.SetBool("IsDead", characterHub.ConditionState == CharacterConditionState.Dead);
        }
    }
}
