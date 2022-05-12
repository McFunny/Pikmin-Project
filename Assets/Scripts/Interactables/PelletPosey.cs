using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PelletPosey : MonoBehaviour
{
    public GameObject pellet;
    public EnemyHealth enemyHealth;
    public Animator anim;
    public AudioClip pelletSound;
    public bool isDead = false;

    void Awake()
    {
       pellet.GetComponent<NavMeshAgent>().enabled = false;
       pellet.layer = 1;
       //this.transform.parent = transform.parent.gameObject;
    }

    void Update()
    {
        if (enemyHealth.health <= 0 && isDead == false)
        {
            isDead = true;
            anim.SetBool("IsDead", true);
            DetachFromParent();
            GetComponent<Collider>().enabled = false;
            StartCoroutine("ReleasePellet");
        }
    }

    public void DetachFromParent()
    {
        // Detaches the transform from its parent.
        pellet.transform.parent = null;
    }

    IEnumerator ReleasePellet()
    {
        AudioSource.PlayClipAtPoint(pelletSound, transform.position);
        pellet.GetComponent<Rigidbody>().isKinematic = false;
        pellet.GetComponent<Collider>().enabled = true;
        yield return new WaitForSeconds(2);
        pellet.GetComponent<Rigidbody>().isKinematic = true;
        pellet.GetComponent<NavMeshAgent>().enabled = true;
        pellet.GetComponent<CarryObject>().canBeCarried = true;
        pellet.layer = 8;
        Destroy(transform.parent.gameObject);

    }
}
