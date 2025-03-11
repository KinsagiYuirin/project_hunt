using UnityEngine;
using System.Collections;
using MadDuck.Scripts.Utils;
using TriInspector;

namespace ProjectHunt.Character.Module 
{
    public class CharacterKnockBackModule : MonoBehaviour
    {
        [Title("Settings")]
        [SerializeField] private LayerMask targetLayer;
        public float knockbackForce = 5f;       // แรงกระแทก
        public float knockbackDuration = 0.2f;  // ระยะเวลาที่โดนกระแทก
        [SerializeField] private Rigidbody2D rb;
        private bool canMove = true;            // เช็คว่าผู้เล่นควบคุมได้หรือไม่

        void OnTriggerEnter2D(Collider2D other)
        {
            if (LayerMaskUtils.IsInLayerMask(other.gameObject.layer, targetLayer))  // ชนกับศัตรู
            {
                Vector2 knockbackDirection = (transform.position - other.transform.position).normalized;
                StartCoroutine(ApplyKnockback(knockbackDirection));
            }
        }

        private IEnumerator ApplyKnockback(Vector2 direction)
        {
            canMove = false; // ปิดการควบคุม
            rb.linearVelocity = Vector2.zero; // รีเซ็ตความเร็วก่อนกระเด็น
            rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);

            yield return new WaitForSeconds(knockbackDuration);

            canMove = true; // กลับมาควบคุมได้
        }
    }
}
