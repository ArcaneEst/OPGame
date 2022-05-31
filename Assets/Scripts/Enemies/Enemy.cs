using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    private Action onAnimationEnd;

    public virtual void PlayAttackAnimation(Action onAnimationEnd)
    {
        this.onAnimationEnd = onAnimationEnd;
    }

    public virtual void EndAttackAnimation()
    {
        onAnimationEnd();
    }
}
