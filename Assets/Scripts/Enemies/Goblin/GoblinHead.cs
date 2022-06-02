using UnityEngine;

public class GoblinHead : MonoBehaviour
{
    [SerializeField] private GameObject parentEnemy;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(Tags.Player))
        {
            parentEnemy.GetComponent<GoblinEnemy>().Die();
        }
    }
}
