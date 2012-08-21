using UnityEngine;
using System.Collections;

public class ParticleSystemScript : MonoBehaviour
{
    void Update()
    {
        if (!particleSystem.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
