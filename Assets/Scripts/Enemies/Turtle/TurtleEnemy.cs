using System;
using UnityEngine;

public class TurtleEnemy : MonoBehaviour
{
    private Rigidbody2D body;
   
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage()
    {
        Destroy(gameObject);
    }
}
