using UnityEngine;
using System.Collections;

public class ParticleSystemScript : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("Particles");
    }
    void Start()
    {
    }
    void Update()
    {
        if (!particleSystem.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
