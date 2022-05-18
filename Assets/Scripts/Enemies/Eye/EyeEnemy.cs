using System;
using UnityEngine;

public class EyeEnemy : MonoBehaviour, IEnemy
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Transform target;

    private Player player;
    
    private const float Speed = 1;

    private int hp = 3;

    private void Awake()
    {
        player = GameObject.FindWithTag(Tags.Player).GetComponent<Player>();
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

        transform.position = newPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
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
                Destroy(gameObject);
                break;
            case DamageSource.Fireball:
                hp -= 1;
                if (hp == 0)
                    Destroy(gameObject);
                break;
            default:
                throw new ArgumentException($"Unknown DamageSource: {damageSource}");
        }
    }

    public void PlayAttackAnimation()   
    {
        animator.SetBool(AnimationBools.EyeAttack, true);
    }

    public void EndAttackAnimation()
    {
        animator.SetBool(AnimationBools.EyeAttack, false);
        player.TakeDamage();
    }
}
