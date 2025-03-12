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
        [SerializeField] private float knockbackForce = 5f;
        [SerializeField] private float knockbackDuration = 0.2f;  

        [Title("Debug")]
        [SerializeField, DisplayAsString] private Rigidbody2D targetRb;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (characterHub.CharacterType is not CharacterType.Enemy) return;

            if (LayerMaskUtils.IsInLayerMask(other.gameObject.layer, targetLayer))
            {
                targetRb = other.GetComponent<Rigidbody2D>(); // ดึง Rigidbody ของ Player

                if (targetRb != null)
                {
                    Knockback(targetRb, other.transform.position);
                    Debug.Log("🎯 Hit Player! Applying Knockback.");
                }
                else
                {
                    Debug.LogError("❌ No Rigidbody2D found on the target!");
                }
            }
        }
        
        private void Knockback(Rigidbody2D targetRb, Vector2 targetPosition)
        {
            Vector2 knockbackDirection = (targetPosition - (Vector2)transform.position).normalized;
            StartCoroutine(ApplyKnockback(targetRb, knockbackDirection));
        }
        
        private IEnumerator ApplyKnockback(Rigidbody2D targetRbP, Vector2 direction)
        {
            targetRbP.linearVelocity = Vector2.zero; // รีเซ็ตความเร็ว
            targetRbP.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

            yield return new WaitForSeconds(knockbackDuration);
        }
    }
}
