using System;
using UnityEngine;

public static class Extensions
{
    // public static string CapitalizeFirstLetter(this string s)
    // {
    //     if (s.IsEmpty())
    //         return s;
    //     
    //     return s.Substring(0, 1).ToUpper() + s.Substring(1);
    // }
    //
    // public static bool IsEmpty(this string s)
    // {
    //     return s.Trim() == "";
    // }

    public static float GetAnimationLength(this Animator animator, string name)
    {
        foreach (var clip in animator.runtimeAnimatorController.animationClips)
            if (clip.name == name)
                return clip.length;
        
        throw new ArgumentException();
    }
}