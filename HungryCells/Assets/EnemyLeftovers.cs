using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyLeftovers : MonoBehaviour
{
    [SerializeField] private VisualEffect visualEffect;
    
    public static void Create(Vector3 position, Vector3 scale)
    { 
        Instantiate(Resources.Load<EnemyLeftovers>("Leftovers"), position, quaternion.identity)
            .transform.localScale = scale;
    }

    private void LateUpdate()
    {
        if (visualEffect.aliveParticleCount == 0)
            Destroy(gameObject);
    }
}
