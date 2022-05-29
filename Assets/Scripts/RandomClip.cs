using System.Collections;
using System.Collections.Generic;
using R60N.Utility;
using UnityEngine;

[System.Serializable]
public class RandomClip
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private MinMaxFloat pitch;
    [SerializeField] private MinMaxFloat volume;
    
    public AudioClip Clip => clip;
    public MinMaxFloat Pitch => pitch;
    public MinMaxFloat Volume => volume;
}
