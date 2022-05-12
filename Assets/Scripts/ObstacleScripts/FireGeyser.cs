using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGeyser : MonoBehaviour
{
    public EnemyHealth enemyHealth;
    bool isDead;
    public ParticleSystem deathParticles;
    public ParticleSystem fireParticles;
    public FireHazard fireHazard;
    public AudioSource source;
    public AudioClip deathSound;

    // Start is called before the first frame update
    void Start()
    {
        fireHazard.isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth.dead == true)
        {
            if(isDead == false)
            {
                fireHazard.isActive = false;
                deathParticles.Play();
                fireParticles.Stop();
                this.tag = "Untagged";
                deathParticles.enableEmission = false;
                source.PlayOneShot(deathSound);
                isDead = true;
            }
           
        }
    }
}
