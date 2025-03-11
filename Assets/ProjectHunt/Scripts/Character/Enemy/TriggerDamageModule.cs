using System;
using System.Collections;
using MadDuck.Scripts.Character.Module;
using MadDuck.Scripts.Utils;
using TriInspector;
using UnityEngine;
using UnityEngine.Serialization;


namespace MadDuck.Scripts.Character.Enemy 
{
    public class TriggerDamageModule : CharacterModule
    {
        [Title("Settings")]
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private Rigidbody2D targetRb;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float knockbackForce = 5f;
        [SerializeField] private float knockbackDuration = 0.2f;  

        private bool canMove = true;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (characterHub.CharacterType is not CharacterType.Enemy) return;

            if (LayerMaskUtils.IsInLayerMask(other.gameObject.layer, targetLayer))
            {
                if (targetRb != null)
                {
                    Knockback(targetRb, targetRb.transform.position);
                    Debug.Log("Hit Player!");
                }
            }
        }
        
        private void Knockback(Rigidbody2D targetRb, Vector2 targetPosition)
        {
            Vector2 knockbackDirection = (targetPosition - (Vector2)transform.position).normalized;
            StartCoroutine(ApplyKnockback(targetRb, knockbackDirection));
        }
        
        private IEnumerator ApplyKnockback(Rigidbody2D targetRb, Vector2 direction)
        {
            canMove = false; // ปิดการควบคุม
            targetRb.linearVelocity = Vector2.zero; // รีเซ็ตความเร็ว
            targetRb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

            yield return new WaitForSeconds(knockbackDuration);

            canMove = true; // กลับมาควบคุมได้
        }
    }
}
