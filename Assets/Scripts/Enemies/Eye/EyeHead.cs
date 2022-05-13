using System;
using UnityEngine;

public class EyeHead : MonoBehaviour
{
    [SerializeField] private GameObject parentEnemy;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(Tags.Player))
        {
            parentEnemy.GetComponent<EyeEnemy>().TakeDamage(DamageSource.Player);
        }
        else if (col.gameObject.CompareTag(Tags.Fireball))
        {
            parentEnemy.GetComponent<EyeEnemy>().TakeDamage(DamageSource.Fireball);
        }
    }
}
