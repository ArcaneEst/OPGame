using System;
using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    private Action onAnimationEnd;

    public void OnDeath(float delay)
    {
        StartCoroutine(Deactivate(delay));
    }
    
    private IEnumerator Deactivate(float delay) {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    public abstract void Die();

    public virtual void PlayAttackAnimation(Action onAnimationEnd)
    {
        this.onAnimationEnd = onAnimationEnd;
    }

    public virtual void EndAttackAnimation()
    {
        onAnimationEnd();
    }
}
