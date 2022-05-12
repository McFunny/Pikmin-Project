using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using Sirenix.OdinInspector;

// Place this on a particle system with a trigger module enabled

//[RequireComponent(typeof(ParticleSystem))]
public class PikminSeedSpawner : MonoBehaviour
{
    //public GameObject Prefab;

    ParticleSystem Ps;
    private Vector3 spawnPoint;
    List<ParticleSystem.Particle> Enter;
    [SerializeField] private Pikmin pikminPrefab = default;
    public List<Pikmin> allPikmin = new List<Pikmin>();
    public float spawnRadius = 2;
    private GameObject onion;
    Onion onionScript;

    void Awake()
    {
        Ps = GetComponent<ParticleSystem>();
        Enter = new List<ParticleSystem.Particle>();
        onion = GameObject.Find("GreenOnion");
        onionScript = onion.GetComponent<Onion>();
        
    }

    void OnValidate()
    {
        Ps = GetComponent<ParticleSystem>();
        var trigger = Ps.trigger;
        if (trigger.enter != ParticleSystemOverlapAction.Callback)
        {
            trigger.enter = ParticleSystemOverlapAction.Callback;
        }
    }

    void OnParticleTrigger()
    {
        // get
        int numInside = Ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, Enter);

        // on enter
        for (int i = 0; i < numInside; i++)
        {
            ParticleSystem.Particle particle = Enter[i];
            
            //Instantiate(Prefab, particle.position, Quaternion.identity);
            //newPikmin.transform.position = transform.position + (Random.insideUnitSphere * radius);
            //spawnPoint = new Vector3(particle.position.x, particle.position.y, particle.position.z);
            SpawnPikminFromSeed(pikminPrefab, ref allPikmin);
            onionScript.pikminSeedOutside = onionScript.pikminSeedOutside - 1;
        }
    }

    public void SpawnPikminFromSeed(Pikmin pikminPrefab, ref List<Pikmin> allPikmin)
    {

        //Pikmin newPikmin = Instantiate(pikminPrefab, spawnPoint, Quaternion.identity);
        Pikmin newPikmin = Instantiate(pikminPrefab);
        //newPikmin.transform.position = spawnPoint;
        
        newPikmin.transform.position = transform.position + (Random.insideUnitSphere * spawnRadius);
        allPikmin.Add(newPikmin);
        PikminManager.allPikmin.Add(newPikmin);


    }
}
