using UnityEngine;

namespace R60N.Utility
{
    [System.Serializable]
    public class MinMaxFloat
    {
        public float min;
        public float max;

        public MinMaxFloat() { }

        public MinMaxFloat(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public float GetRandom() => Random.Range(min, max);
    }
}
