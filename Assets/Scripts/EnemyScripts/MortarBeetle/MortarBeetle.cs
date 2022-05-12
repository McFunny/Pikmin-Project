using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class MortarBeetle : MonoBehaviour
{
    public Transform enemy;
    public Transform pikmin;
    public Transform attackCollider;
    //public Transform corpseCollider;
    private Transform nearestPikmin;
    private GameObject boulderSpawn;
    public NavMeshAgent agent;
    private Animator anim;
    private int pikminLayer;
    public GameObject boulder;
    public EnemyHealth enemyHealth;
    public float thrust = 6;
    public Boulder boulderScript;

    public Vector3 recoilDirection;

    public LayerMask whatIsPikmin;

    

    public float sightRange;
    public bool pikminInSightRange;
    public bool pikminAttacking;

    public bool isDead = false;

    public ParticleSystem deathParticles;

    void Awake()
    {
        anim = GetComponentInParent<Animator>();

        pikminLayer = LayerMask.NameToLayer("Pikmin");

        boulderSpawn = GameObject.Find("RockOrigin");
    }


    void Update()
    {
        if (enemyHealth.dead == true && isDead == false)
        {
            StartCoroutine("Death");
            isDead = true;
        }

        Collider[] hitColliders = Physics.OverlapSphere(enemy.position, sightRange, 1 << pikminLayer);
        float minimumDistance = Mathf.Infinity;
        foreach (Collider collider in hitColliders)
        {
            float distance = Vector3.Distance(enemy.position, collider.transform.position);
            if (distance < minimumDistance && distance > 2)
            {
                minimumDistance = distance;
                nearestPikmin = collider.transform;
            }

            float range = sightRange - 1;
            pikmin = nearestPikmin;
            pikminInSightRange = Physics.CheckSphere(transform.position, range, whatIsPikmin);
            pikminAttacking = Physics.CheckSphere(transform.position, 2, whatIsPikmin);

            
        }
        if (pikminInSightRange) anim.SetBool("InRange", true);
        if (!pikminInSightRange) anim.SetBool("InRange", false);

        if (pikminAttacking) anim.SetBool("BeingAttacked", true);
        if (!pikminAttacking) anim.SetBool("BeingAttacked", false);

        
    }

    public void Emerge()
    {
        transform.LookAt(pikmin);
    }

    public void Attack()
    {
        if (nearestPikmin.gameObject == null)
        {
            if (!pikminInSightRange) anim.SetBool("InRange", false);
            if (!pikminAttacking) anim.SetBool("BeingAttacked", false);
        }
        GameObject newBoulder = Instantiate(boulder);

        boulderScript = newBoulder.GetComponent<Boulder>();
        boulderScript.source = GetComponent<AudioSource>();

        newBoulder.transform.position = boulderSpawn.transform.position;
        float delay = .05f;
        //newBoulder.transform.DOMove(nearestPikmin.position, delay);
        newBoulder.GetComponent<Boulder>().Throw(nearestPikmin.position, 1.5f, delay);
    }

    public void KnockOff()
    {
        recoilDirection = transform.forward;
        Collider[] attackingPikmin = Physics.OverlapSphere(enemy.position, 2, 1 << pikminLayer);
        foreach (Collider collider in attackingPikmin)
        {
            collider.GetComponent<Pikmin>().knockBack = thrust;
            collider.GetComponent<Pikmin>().recoilDirection = recoilDirection;
            collider.GetComponent<Pikmin>().StartCoroutine("Recoil");
        }
    }
    
    public IEnumerator Death()
    {
        
        anim.SetBool("IsDead", true);
        deathParticles.Play();
        yield return new WaitForSeconds(4);
        deathParticles.enableEmission = false;
        gameObject.layer = 8;

        GetComponent<Collider>().enabled = true;
        attackCollider.GetComponent<Collider>().enabled = false;
        GetComponent<CarryObject>().canBeCarried = true;
        
        


    }
}
