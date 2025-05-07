using System;
using System.Collections;
using System.Collections.Generic;
using MadDuck.Scripts.Character.Module;
using MadDuck.Scripts.Utils;
using TriInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace MadDuck.Scripts.Character.Enemy 
{
    public class TriggerDamageModule : MonoBehaviour
    {
        [Title("Settings")]
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private float knockbackForce = 5f;
        [SerializeField] private float stayDurationThreshold = 1.5f; // เวลาที่ให้ยืนอยู่ก่อนเด้ง
        
        [SerializeField] private Rigidbody2D player;
        [SerializeField] private Transform target;
        
        private Dictionary<Collider2D, float> stayTimers = new();
        
        private void Update()
        {
            //Knockback(player, target.position );
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            Knockback(player, other.transform.position);
        }

        private void Knockback(Rigidbody2D rb, Vector2 targetPosition)
        {
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        }
    }
}
