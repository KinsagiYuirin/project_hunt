using System;
using System.Collections;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace MadDuck.Scripts.Character.Module
{
    [Serializable]
    public struct AttackPattern
    {
        [Group("Area"), Required] public DamageArea damageArea;
        [Group("Damage"), Min(0)] public float damage;
        [Group("Timing"), Min(0)] public float delay;
        [Group("Timing"), Min(0)] public float duration;
        [Group("Timing"), Min(0)] public float interval;
        [Group("Timing"), Min(0)] public float resetComboTime;
    }
    /// <summary>
    /// Module responsible for handling basic attacks.
    /// </summary>
    public class CharacterBasicAttackModule : CharacterModule
    {
        [Title("Settings")]
        [SerializeField] private Transform comboParent;
        [TableList(Draggable = true,
            HideAddButton = false,
            HideRemoveButton = false,
            AlwaysExpanded = false)]
        [SerializeField] protected List<AttackPattern> attackPatterns;
        
        [Title("Animator")]
        [SerializeField] protected Animator attackAnimator;
        private static readonly int IsDrawnLightSword = Animator.StringToHash("IsDrawnLightAttack");
        private static readonly int IsLightAttack = Animator.StringToHash("IsLightAttack 0");
        [SerializeField, DisplayAsString] private bool IsSwordDrawn;
        [SerializeField, DisplayAsString] private bool IsAttacking;

        [Title("Sound")] 
        [SerializeField] private bool haveAttackSound;
        [SerializeField, ShowIf("haveAttackSound")] private AudioSource audioSource;
        [SerializeField, ShowIf("haveAttackSound")] private AudioClip[] attackSound; 
        
        [Title("Debug")]
        [SerializeField, DisplayAsString] protected int currentPatternIndex;
        [SerializeField, DisplayAsString] protected int previousPatternIndex = -1;
        [SerializeField, DisplayAsString] protected bool attackReady;
        [SerializeField, DisplayAsString] protected float currentInterval;
        [SerializeField, DisplayAsString] protected float currentComboTime;

        public AttackPattern? CurrentPattern => attackPatterns[currentPatternIndex];
        protected AttackPattern? PreviousPattern
        {
            get
            {
                if (previousPatternIndex == -1) return null;
                return attackPatterns[previousPatternIndex];
            }
        }

        protected void Start()
        {
            IsSwordDrawn = false;
            IsAttacking = false;
        }

        protected Coroutine attackCoroutine;

        public override void Initialize(CharacterHub characterHub)
        {
            base.Initialize(characterHub);
            currentPatternIndex = 0;
            currentInterval = 0;
            previousPatternIndex = -1;
            attackPatterns.ForEach(pattern =>
            {
                pattern.damageArea.SetActive(false);
                pattern.damageArea.OnHitEvent += OnHit;
            });
        }

        public override void Shutdown()
        {
            base.Shutdown();
            currentPatternIndex = 0;
            currentInterval = 0;
            previousPatternIndex = -1;
            attackPatterns.ForEach(pattern =>
            {
                pattern.damageArea.SetActive(false);
                pattern.damageArea.OnHitEvent -= OnHit;
            });
        }

        /// <summary>
        /// Method called when the damage area hits a collider.
        /// </summary>
        /// <param name="collider">Collider that was hit.</param>
        protected virtual void OnHit(Collider2D collider)
        {
            if (!collider.TryGetComponent(out CharacterHub characterHub)) return;
            var healthModule = characterHub.FindModuleOfType<CharacterHealthModule>();
            if (healthModule && CurrentPattern != null) 
                healthModule.ChangeHealth(-CurrentPattern.Value.damage);
        }
        
        protected override void HandleInput()
        {
            if (characterHub.CharacterType is not CharacterType.Player) return;
            base.HandleInput();
            if (PlayerInput.LightAttackButton.isDown)
            {
                Attack();
            }
        }

        protected override void UpdateModule()
        {
            if (!ModulePermitted) return;
            base.UpdateModule();
            if (PreviousPattern != null)
            {
                if (attackReady && currentComboTime < PreviousPattern.Value.resetComboTime)
                {
                    currentComboTime += Time.deltaTime;
                }
                if (currentComboTime >= PreviousPattern.Value.resetComboTime)
                {
                    currentComboTime = 0;
                    currentPatternIndex = 0;
                    previousPatternIndex = -1;
                }
                if (!attackReady && currentInterval < PreviousPattern.Value.interval)
                {
                    currentInterval += Time.deltaTime;
                    return;
                }
            }
            attackReady = true;
            currentInterval = 0;
        }
        
        /// <summary>
        /// Method that triggers the attack.
        /// </summary>
        public virtual void Attack()
        {
            if (!ModulePermitted) return;
            if (!attackReady) return;
            if (attackCoroutine != null) return;

            if (haveAttackSound)
            {
                audioSource.PlayOneShot(attackSound[Random.Range(0,attackSound.Length)]);
            }
            attackCoroutine = StartCoroutine(AttackCoroutine());
        }

        public virtual void SetAttackDirection(Vector2 direction)
        {
            if (!ModulePermitted) return;

            direction = new Vector2(direction.x, 0f); // ตัดค่า Y ทิ้ง
            direction.Normalize();

            if (direction.x != 0f) // ป้องกันการตั้งค่าทิศทางเป็น (0,0)
                comboParent.right = direction;
        }

        
        /// <summary>
        /// Coroutine that handles the timing of the attack.
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator AttackCoroutine()
        {
            IsSwordDrawn = false;
            IsAttacking = false;
            
            if (CurrentPattern == null) yield break;
            currentComboTime = 0;
            attackAnimator.SetTrigger(IsLightAttack);
            characterHub.ChangeActionState(CharacterActionState.Basic);
            yield return new WaitForSeconds(CurrentPattern.Value.delay);
            CurrentPattern.Value.damageArea.SetActive(true);
            yield return new WaitForSeconds(CurrentPattern.Value.duration);
            CurrentPattern.Value.damageArea.SetActive(false);
            characterHub.ChangeActionState(CharacterActionState.None);
            previousPatternIndex = currentPatternIndex;
            currentPatternIndex = (currentPatternIndex + 1) % attackPatterns.Count;
            attackReady = false;
            attackCoroutine = null;
        }
        
        protected override void UpdateAnimator()
        {
            base.UpdateAnimator();
            if (attackAnimator == null)
            {
                Debug.LogWarning("Attack animator is not assigned.");
                return;
            }
        }
    }
}

