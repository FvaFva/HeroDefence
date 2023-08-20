using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PercImpactPlayer : MonoBehaviour
{
    [SerializeField] private PercImpactParticle _mainParticles;

    private List<PercImpactParticle> _particlesPool = new List<PercImpactParticle>();

    public void ShowColoredEffect(Color color)
    {
        PercImpactParticle freeParticleSystem = GetFreeParticleSystem();
        freeParticleSystem.Play(color);
    }

    private PercImpactParticle GetFreeParticleSystem()
    {
        var allFreeSystems = _particlesPool.Where(p => p.IsActive == false);
        if (allFreeSystems.Count() == 0)
            return InstantiateNewParticleSystem();
        else
            return allFreeSystems.First();
    }

    private PercImpactParticle InstantiateNewParticleSystem()
    {
        PercImpactParticle newSystem = Instantiate(_mainParticles, transform.position, default(Quaternion), transform);
        newSystem.gameObject.SetActive(false);
        newSystem.transform.forward = Vector3.up;
        _particlesPool.Add(newSystem);
        return newSystem;
    }
}
