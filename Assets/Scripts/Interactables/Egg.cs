using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public GameObject nectar;
    public EnemyHealth enemyHealth;
    public ParticleSystem deathParticle;
    private bool isDead = false;
    private Vector3 spawnPosition;
    public AudioClip breakSound;
    
    void Awake()
    {
        //enemyHealth = GetComponent<EnemyHealth>();
        //deathParticle = GetComponent<ParticleSystem>();
        spawnPosition = new Vector3(transform.position.x, 0, transform.position.z);
    }

    void Update()
    {
        if(enemyHealth.health <= 0 && isDead == false)
        {
            isDead = true;
            Death();
        }
    }

    public void Death()
    {
        AudioSource.PlayClipAtPoint(breakSound, spawnPosition);
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        GameObject newNectar = Instantiate(nectar);
        newNectar.transform.position = spawnPosition;
        deathParticle.Play();
        StartCoroutine("DeathComplete");
    }

    IEnumerator DeathComplete()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
