using System;
using UnityEngine;

public class GoblinEnemy : MonoBehaviour, IEnemy
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

    public void TakeDamage()
    {
        Destroy(gameObject);
    }
    
    public void PlayAttackAnimation()   
    {
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

    public void EndAttackAnimation()
    {
        animator.SetBool(AnimationBools.GoblinAttack, false);
        player.TakeDamage();
    }
}
