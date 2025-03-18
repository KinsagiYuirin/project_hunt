using System;
using System.Collections;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace MadDuck.Scripts.Character.Module
{
    [Serializable]
    public struct GuardPattern
    {
        [Group("Area"), Required] public GuardArea guardArea;
        [Group("Timing"), Min(0)] public float delay;
        [Group("Timing"), Min(0)] public float duration;
        [Group("Timing"), Min(0)] public float interval;
        [Group("Timing"), Min(0)] public float resetComboTime;
    }
    
    public class CharacterGuardModule : CharacterModule
    {
        [FormerlySerializedAs("basicGuardPatterns")]
        [Title("Settings")]
        [TableList(Draggable = true,
            HideAddButton = false,
            HideRemoveButton = false,
            AlwaysExpanded = false)]
        [SerializeField] private List<GuardPattern> guardPatterns;
        
        [Title("Animator")]
        [SerializeField] protected Animator guardAnimator;
        private static readonly int IsDrawnSheild = Animator.StringToHash("IsDrawn");
        private static readonly int IsGuard = Animator.StringToHash("IsGuard");
        [SerializeField, DisplayAsString] protected bool IsDrawn;
        [SerializeField, DisplayAsString] protected bool IsGuarding;
        
        [Title("Debug")]
        [SerializeField, DisplayAsString] private int currentPatternIndex;
        [SerializeField, DisplayAsString] private int previousPatternIndex = -1;
        [SerializeField, DisplayAsString] private bool guardReady;
        [SerializeField, DisplayAsString] private float currentInterval;
        [SerializeField, DisplayAsString] private float currentComboTime;
        
        private GuardPattern? CurrentPattern => guardPatterns[currentPatternIndex];
        private GuardPattern? PreviousPattern
        {
            get
            {
                if (previousPatternIndex == -1) return null;
                return guardPatterns[previousPatternIndex];
            }
        }

        private Coroutine guardCoroutine;

        private void Start()
        {
            IsDrawn = false;
            IsGuarding = false;
        }

        public override void Initialize(CharacterHub characterHub)
        {
            base.Initialize(characterHub);
            currentPatternIndex = 0;
            currentInterval = 0;
            previousPatternIndex = -1;
            guardPatterns.ForEach(pattern =>
            {
                pattern.guardArea.SetActive(false);
                pattern.guardArea.OnHitEvent += OnHit;
            });
        }

        public override void Shutdown()
        {
            base.Shutdown();
            currentPatternIndex = 0;
            currentInterval = 0;
            previousPatternIndex = -1;
            guardPatterns.ForEach(pattern =>
            {
                pattern.guardArea.SetActive(false);
                pattern.guardArea.OnHitEvent -= OnHit;
            });
        }
        
        protected virtual void OnHit(Collider2D collider)
        {
            Debug.Log(collider.gameObject.name);
        }
        
        protected override void HandleInput()
        {
            if (characterHub.CharacterType is not CharacterType.Player) return;
            base.HandleInput();
            if (PlayerInput.BlockButton.isDown)
            {
                SetGuard();
            }
        }
        
        protected override void UpdateModule()
        {
            if (!ModulePermitted) return;
            base.UpdateModule();
            if (PreviousPattern != null)
            {
                if (guardReady && currentComboTime < PreviousPattern.Value.resetComboTime)
                {
                    currentComboTime += Time.deltaTime;
                }
                if (currentComboTime >= PreviousPattern.Value.resetComboTime)
                {
                    currentComboTime = 0;
                    currentPatternIndex = 0;
                    previousPatternIndex = -1;
                }
                if (!guardReady && currentInterval < PreviousPattern.Value.interval)
                {
                    currentInterval += Time.deltaTime;
                    return;
                }
            }
            guardReady = true;
            currentInterval = 0;
        }
        
        public virtual void SetGuard()
        {
            if (!ModulePermitted) return;
            if (!guardReady) return;
            if (guardCoroutine != null) return;
            guardCoroutine = StartCoroutine(GuardCoroutine());
        }
        
        protected IEnumerator GuardCoroutine()
        {
            if (CurrentPattern == null) yield break;
            currentComboTime = 0;
            characterHub.ChangeActionState(CharacterStates.CharacterActionState.Basic);
            StepAnimation(0);
            yield return new WaitForSeconds(CurrentPattern.Value.delay);
            StepAnimation(1);
            CurrentPattern.Value.guardArea.SetActive(true);
            StepAnimation(2);
            yield return new WaitForSeconds(CurrentPattern.Value.duration);
            StepAnimation(3);
            CurrentPattern.Value.guardArea.SetActive(false);
            characterHub.ChangeActionState(CharacterStates.CharacterActionState.None);
            previousPatternIndex = currentPatternIndex;
            currentPatternIndex = (currentPatternIndex + 1) % guardPatterns.Count;
            guardReady = false;
            guardCoroutine = null;
        }
        
        private void StepAnimation(int step)
        {
            switch (step)
            {
                case 0:
                    IsDrawn = true;
                    break;
                case 1:
                    IsDrawn = false;
                    break;
                case 2:
                    IsGuarding = true;
                    break;
                case 3:
                    IsGuarding = false;
                    break;
            }
        }
        
        protected override void UpdateAnimator()
        {
            base.UpdateAnimator();
            if (guardAnimator == null) {return;}
            guardAnimator.SetBool(IsDrawnSheild, IsDrawn);
            guardAnimator.SetBool(IsGuard, IsGuarding);
        }
    }
    
}
