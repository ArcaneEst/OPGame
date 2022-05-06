using System;
using UnityEngine;

public class GoblinEnemy : MonoBehaviour, IEnemy
{
    private Rigidbody2D body;
    private Animator animator;
   
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void TakeDamage()
    {
        Destroy(gameObject);
    }
    
    public void PlayAttackAnimation()   
    {
        animator.SetBool("goblinAttack", true);
    }

    public void EndAttackAnimation()
    {
        animator.SetBool("goblinAttack", false);
    }
}
