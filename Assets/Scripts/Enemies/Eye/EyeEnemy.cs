using System;
using UnityEngine;

public class EyeEnemy : Enemy
{
    private Rigidbody2D rigidbody2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Transform target;
    
    private const float Speed = 2;
    private int hp = 2;
    private bool isAttacking = false;

    private float timerForAttacking = Mathf.Infinity;
    private const float DefaultCooldownForAttacking = 1;

    private float pushbackTimer = 0;
    private bool pushedback = false;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (target == null) 
            return;
        
        var step = Speed * Time.deltaTime;
        var newPosition = Vector2.MoveTowards(transform.position, target.position, step);
        
        spriteRenderer.flipX = transform.position.x > newPosition.x;
        
        timerForAttacking += Time.deltaTime;

        ManagePushback();

        if (!isAttacking && timerForAttacking >= DefaultCooldownForAttacking)
        {
            transform.position = newPosition;
        }
    }

    private void ManagePushback()
    {
        if (pushedback)
            pushbackTimer += Time.deltaTime;

        if (pushbackTimer > 1)
        {
            pushbackTimer = 0;
            pushedback = false;
            rigidbody2D.velocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (timerForAttacking >= DefaultCooldownForAttacking && other.gameObject.CompareTag(Tags.Player))
            target = other.transform;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
            target = null;
    }

    public void TakeDamage(DamageSource damageSource)
    {
        switch (damageSource)
        {
            case DamageSource.Player:
                Die();
                break;
            case DamageSource.Fireball:
                hp -= 1;
                rigidbody2D.velocity = Vector2.down * 4;
                pushedback = true;
                if (hp == 0)
                    Die();
                break;
            default:
                throw new ArgumentException($"Unknown DamageSource: {damageSource}");
        }
    }

    public override void Die()
    {
        animator.SetTrigger(AnimationTriggers.EyeDie);
        OnDeath(animator.GetAnimationLength(AnimationTriggers.EyeDie));
    }

    public override void PlayAttackAnimation(Action onAnimationEnd)
    {
        base.PlayAttackAnimation(onAnimationEnd);
        animator.SetBool(AnimationBools.EyeAttack, true);
        
        isAttacking = true;
    }

    public override void EndAttackAnimation()
    {
        animator.SetBool(AnimationBools.EyeAttack, false);
        base.EndAttackAnimation();
        
        isAttacking = false;
        timerForAttacking = 0;
    }
}
