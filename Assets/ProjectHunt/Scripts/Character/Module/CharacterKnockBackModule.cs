using UnityEngine;
using System.Collections;
using MadDuck.Scripts.Utils;
using TriInspector;
using UnityEngine.Serialization;

namespace ProjectHunt.Character.Module 
{
    public class CharacterKnockBackModule : MonoBehaviour
    {
        [Title("Settings")]
        [SerializeField] private LayerMask targetLayer;
        public float knockbackForce = 5f;       // แรงกระแทก
        public float knockbackDuration = 0.2f;  // ระยะเวลาที่โดนกระแทก
        [SerializeField] private Rigidbody2D rb; // กำหนดเองจาก Inspector หรือโค้ดอื่น

        private bool canMove = true;            // เช็คว่าผู้เล่นควบคุมได้หรือไม่

        void OnTriggerEnter2D(Collider2D other)
        {
            if (rb == null) 
            {
                Debug.LogError("❌ Rigidbody2D ยังไม่ได้ถูกกำหนดใน CharacterKnockBackModule!");
                return;
            }

            if (LayerMaskUtils.IsInLayerMask(other.gameObject.layer, targetLayer))  // ชนกับศัตรู
            {
                Vector2 knockbackDirection = (transform.position - other.transform.position).normalized;
                StartCoroutine(ApplyKnockback(knockbackDirection));
                Debug.Log($"💥 Knockback applied! Direction: {knockbackDirection}");
            }
        }

        private IEnumerator ApplyKnockback(Vector2 direction)
        {
            canMove = false; // ปิดการควบคุม

            //rb.linearVelocity = Vector2.zero; // รีเซ็ตความเร็ว
            rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

            yield return new WaitForSeconds(knockbackDuration);

            canMove = true; // กลับมาควบคุมได้
        }
        
        public bool CanMove()
        {
            return canMove;
        }
    }
}