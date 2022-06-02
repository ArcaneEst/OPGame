using System;
using UnityEngine;

public class GoblinEnemy : Enemy
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Player player;

    void Awake()
    {
        player = GameObject.FindWithTag(Tags.Player).GetComponent<Player>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void Die()
    {
        animator.SetTrigger(AnimationTriggers.GoblinDie);
        OnDeath(animator.GetAnimationLength(AnimationTriggers.GoblinDie));
    }

    public override void PlayAttackAnimation(Action onAnimationEnd)
    {
        base.PlayAttackAnimation(onAnimationEnd);
        animator.SetBool(AnimationBools.GoblinAttack, true);
        
        var xDiff = transform.position.x - player.GetComponent<Rigidbody2D>().position.x;
        if (xDiff > 0)
        {
            if (!spriteRenderer.flipX)
                spriteRenderer.flipX = true;
        }
        else
        {
            if (spriteRenderer.flipX)
                spriteRenderer.flipX = false;
        }
    }
    
    public override void EndAttackAnimation()
    {
        animator.SetBool(AnimationBools.GoblinAttack, false);
        base.EndAttackAnimation();
    }
}
