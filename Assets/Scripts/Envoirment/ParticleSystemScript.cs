using UnityEngine;
using System.Collections;

public class ParticleSystemScript : MonoBehaviour
{
    void Start()
    {
        particleSystem.Emit(5);
    }
    void Update()
    {
        if (!particleSystem.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
