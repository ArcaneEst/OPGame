using System;
using UnityEngine;

public class MushroomEnemy : Enemy
{
    private Rigidbody2D body;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    private Player player;

    private float speed = 3;
    private float timer = 0;
    private int hp = 2;

    private bool isAttacking = false;
    private bool changedDir = false;
 
    private void Awake()
    {
        player = GameObject.FindWithTag(Tags.Player).GetComponent<Player>();
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isAttacking)
            return;
        
        timer += Time.deltaTime;
        if (timer > 3)
        {
            ChangeMovingDirection();
        }
        
        body.velocity = new Vector2(speed, body.velocity.y);
    }

    private void ChangeMovingDirection()
    {
        speed = -speed;
        timer = 0;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag(Tags.Fireball))
        {
            GetDamage();
        }
        else if (col.gameObject.CompareTag(Tags.Wall))
        {
            ChangeMovingDirection();
        }
        else if (col.gameObject.CompareTag(Tags.Player))
        {
            isAttacking = true;
            body.velocity = Vector2.zero;

            var xDiff = transform.position.x - player.GetComponent<Rigidbody2D>().position.x;
            if (xDiff > 0 && !spriteRenderer.flipX)
            {
                spriteRenderer.flipX = !spriteRenderer.flipX;
                changedDir = true;
            }
        }
    }

    private void GetDamage()
    {
        hp -= 1;
        if (hp == 0)
            Destroy(gameObject);
    }

    public override void PlayAttackAnimation(Action onAnimationEnd)
    {
        base.PlayAttackAnimation(onAnimationEnd);
        animator.SetBool(AnimationBools.MushroomAttack, true);
    }

    public override void EndAttackAnimation()
    {
        animator.SetBool(AnimationBools.MushroomAttack, false);
        base.EndAttackAnimation();
        
        isAttacking = false;

        if (changedDir)
        {
            changedDir = false;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }
}
