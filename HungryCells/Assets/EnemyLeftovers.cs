using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyLeftovers : MonoBehaviour
{
    [SerializeField] private VisualEffect visualEffect;
    
    public static void Create(Vector3 position)
    { 
        Instantiate(Resources.Load<EnemyLeftovers>("Leftovers"), position, quaternion.identity);
    }

    private void LateUpdate()
    {
        if (visualEffect.aliveParticleCount == 0)
            Destroy(gameObject);
    }
}
