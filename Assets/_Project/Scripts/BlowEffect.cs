using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowEffect : MonoBehaviour
{
    public ParticleSystem particleSystem1, particleSystem3;
    public void SetColor(Color color)
    {
        particleSystem1.startColor = color;
        particleSystem3.startColor = color;
    }
    private void Start()
    {
        Destroy(gameObject, 3f);
    }
}
