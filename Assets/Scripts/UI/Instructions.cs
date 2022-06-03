using System;
using UnityEngine;
using UnityEngine.UI;

public class Instructions : MonoBehaviour
{
    [SerializeField] private Image image;

    private float timer = 0;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > 7)
            Destroy(gameObject);
    }
}
