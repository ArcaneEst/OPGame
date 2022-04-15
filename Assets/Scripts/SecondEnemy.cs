using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondEnemy : MonoBehaviour
{
    private Rigidbody2D secondEnemy;

    private bool onRight;
    private float speed = 1f;
    private Vector2 start;
    private float timer = 0;
    private int hp = 3;
 
    private void Awake()
    {
        secondEnemy = GetComponent<Rigidbody2D>();
        start = secondEnemy.position;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 3)
        {
            speed = -speed;
            timer = 0;
        }
        
        secondEnemy.velocity = new Vector2(speed, secondEnemy.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Bullet"))
        {
            GetDamage();
        }
    }

    private void GetDamage()
    {
        hp -= 1;
        if (hp == 0)
            Destroy(this.gameObject);
    }
}
