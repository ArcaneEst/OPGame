using System;
using UnityEngine;

public class GoblinEnemy : MonoBehaviour, IEnemy
{
    private Animator animator;

    private Player player;
   
    void Awake()
    {
        player = GameObject.FindWithTag(Tags.Player).GetComponent<Player>();
        animator = GetComponent<Animator>();
    }

    public void TakeDamage()
    {
        Destroy(gameObject);
    }
    
    public void PlayAttackAnimation()   
    {
        animator.SetBool(AnimationBools.GoblinAttack, true);
    }

    public void EndAttackAnimation()
    {
        animator.SetBool(AnimationBools.GoblinAttack, false);
        player.TakeDamage();
    }
}
