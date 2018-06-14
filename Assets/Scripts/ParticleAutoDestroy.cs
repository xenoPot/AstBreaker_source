using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// パーティクル自動破棄
/// </summary>
public class ParticleAutoDestroy : MonoBehaviour
{

    private ParticleSystem[] _particles;

    private void Start()
    {
        _particles = this.GetComponents<ParticleSystem>();
    }

    private void Update()
    {
        foreach (var particle in _particles)
        {
            if (particle.isEmitting || particle.particleCount != 0)
            {
                return;
            }
        }
        Destroy(this.gameObject);
    }
}
