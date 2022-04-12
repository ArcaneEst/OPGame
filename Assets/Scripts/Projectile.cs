using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
     private BoxCollider2D boxCollider2D;
     private Animator animator;
     
     public float speed = 20;
     
     private bool hit;

     private void Awake()
     {
          boxCollider2D = GetComponent<BoxCollider2D>();
          animator = GetComponent<Animator>();
     }

     private void Update()
     {
          if (hit) return;
          var movementSpeed = speed * Time.deltaTime;
          transform.Translate(0, -movementSpeed, 0); 
     }

     private void OnTriggerEnter2D(Collider2D col)
     {
          hit = true;
          boxCollider2D.enabled = false;
          animator.SetTrigger("explode");
     }

     public void Shoot()
     {
          gameObject.SetActive(true); 
          hit = false;
          boxCollider2D.enabled = true;
     }

     private void Deactivate()
     {
          gameObject.SetActive(false);
     }
}
