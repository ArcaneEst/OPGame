using System;
using UnityEngine;

public class BallEnemy : MonoBehaviour
{
    private Transform target;
    
    private const float Speed = 1;

    private int hp = 3;

    private void Update()
    {
        if (target == null) 
            return;
        
        var step = Speed * Time.deltaTime;
        var newPosition = Vector2.MoveTowards(transform.position, target.position, step);
        
        var sprite = GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        sprite.flipX = transform.position.x > newPosition.x;

        transform.position = newPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            target = other.transform;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
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
}
