using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpawnParticles : MonoBehaviour {

    public Transform Particle_ShipSpawn;
    public Transform ParticlePosition;

    public void SpawnParticles ()
    {
        Instantiate(Particle_ShipSpawn, ParticlePosition.position, ParticlePosition.rotation);
    }
}
