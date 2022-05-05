using System;
using UnityEngine;

public class MineEnemy : MonoBehaviour
{
    private Rigidbody2D body;

    private float speed = 3;
    private float timer = 0;
    private int hp = 3;
 
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 3)
        {
            speed = -speed;
            timer = 0;
            var spriteRenderer = gameObject.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
        
        body.velocity = new Vector2(speed, body.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Fireball"))
        {
            GetDamage();
        }
    }

    private void GetDamage()
    {
        hp -= 1;
        if (hp == 0)
            Destroy(gameObject);
    }
}
