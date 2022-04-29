using System;
using UnityEngine;

public class BallHead : MonoBehaviour
{
    [SerializeField] private GameObject parentEnemy;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            parentEnemy.GetComponent<BallEnemy>().TakeDamage(DamageSource.Player);
        }
        else if (col.gameObject.CompareTag("Fireball"))
        {
            parentEnemy.GetComponent<BallEnemy>().TakeDamage(DamageSource.Fireball);
        }
    }
}
