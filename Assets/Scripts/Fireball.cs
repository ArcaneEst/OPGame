using System;
using UnityEngine;

public class Fireball : MonoBehaviour
{
     private BoxCollider2D boxCollider2D;
     private Animator animator;
     
     public float speed = 20;
     
     private bool hit = false;

     private void Awake()
     {
          boxCollider2D = GetComponent<BoxCollider2D>();
          animator = GetComponent<Animator>();
     }

     private void Update()
     {
          if (hit) 
               return;
          
          transform.Translate(0, -speed * Time.deltaTime, 0); 
     }

     private void OnCollisionEnter2D(Collision2D col)
     {
          hit = true;
          boxCollider2D.enabled = false;
          
          if (col.gameObject.CompareTag(Tags.Breakable))
          {
               Destroy(col.gameObject, 0.1f);
          }
          
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
