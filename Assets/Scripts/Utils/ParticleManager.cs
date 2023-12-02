using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance;

    [SerializeField] GameObject hitParticles;
    [SerializeField] GameObject sweatParticles;
    [SerializeField] GameObject fishParticles;
    [SerializeField] GameObject blockParticles;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnHitParticles(Vector3 pos)
    {
        SpawnParticles(hitParticles, pos);
    }

    public void SpawnSweatParticles(Vector3 pos)
    {
        SpawnParticles(sweatParticles, pos);
    }

    public void SpawnFishParticles(Vector3 pos)
    {
        SpawnParticles(fishParticles, pos);
    }

    public void SpawnBlockParticles(Vector3 pos)
    {
        SpawnParticles(blockParticles, pos);
    }

    private void SpawnParticles(GameObject original, Vector3 pos)
    {
        GameObject particles = Instantiate(hitParticles, transform);
        particles.transform.position = pos;
    }

}
