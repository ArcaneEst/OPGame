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
        transform.position = Vector2.MoveTowards(transform.position, target.position, step);
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
        if (damageSource == DamageSource.Player)
        {
            Destroy(gameObject);
        }
        else if (damageSource == DamageSource.Fireball)
        {
            hp -= 1;
            if (hp == 0)
                Destroy(gameObject);
        }
    }
}
