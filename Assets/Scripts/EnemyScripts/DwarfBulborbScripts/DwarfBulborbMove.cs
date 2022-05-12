using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DwarfBulborbMove : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform pikmin;

    public LayerMask whatIsGround, whatIsPikmin;

    private Animator anim;

    EnemyHealth enemyHealth;

    //From other scripts
    public Transform Enemy;
    

    private Transform nearestPikmin;
    private int pikminLayer;
    public GameObject pikminEaten;


    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    public float biteRange;
    bool alreadyAttacked;
    bool canMove;
    bool isDead;
    RaycastHit hit;

    //Particles
    public ParticleSystem deathParticles;

    //States
    public float sightRange, attackRange;
    public bool pikminInSightRange, pikminInAttackRange;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();

        enemyHealth = GetComponent<EnemyHealth>();

        pikminLayer = LayerMask.NameToLayer("Pikmin");
        //Debug.Log(pikminLayer);
        canMove = true;
        isDead = false;
    }

    private void Update()
    {

        Debug.DrawRay(transform.position, transform.forward * biteRange, Color.white);
        if (enemyHealth.dead == true && isDead == false)
        {
            StartCoroutine("Death");
            isDead = true;
        }

        Collider[] hitColliders = Physics.OverlapSphere(Enemy.position, sightRange, 1 << pikminLayer);
        float minimumDistance = Mathf.Infinity;
        foreach (Collider collider in hitColliders)
        {
            float distance = Vector3.Distance(Enemy.position, collider.transform.position);
            if (distance < minimumDistance)
            {
                minimumDistance = distance;
                nearestPikmin = collider.transform;
            }
        }

        pikmin = nearestPikmin;   //GameObject.Find("Pikjammo(Clone)").transform;
        pikminInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPikmin);
        pikminInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPikmin);
        if (isDead == false)
        {
            if (!pikminInSightRange && !pikminInAttackRange) Patroling();
            if (pikminInSightRange && !pikminInAttackRange) ChasePikmin();
            if (pikminInSightRange && pikminInAttackRange) AttackPikmin();
        }
        
    }


    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        anim.SetBool("IsAttacking", false);
        anim.SetBool("EnteredRadius", false);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);



        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;

    }
    private void ChasePikmin()
    {
        if (canMove == true)
        {
            agent.SetDestination(pikmin.position);

            anim.SetBool("IsAttacking", false);
            anim.SetBool("EnteredRadius", true);
        }


    }
    private void AttackPikmin()
    {
        agent.SetDestination(transform.position);
        anim.SetBool("IsAttacking", true);
        transform.LookAt(pikmin);

        if (!alreadyAttacked)
        {
            //Attack code here

            anim.SetBool("IsAttacking", false);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void Attack()
    {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, biteRange))
        {
            print("Raycast hit");

            if (hit.collider.tag == "Pikmin")
            {
                print("Raycast hit Pikmin");
                
                anim.SetBool("AttackSuccessful", true);
                StartCoroutine("DoneAttacking");
                pikminEaten = hit.collider.gameObject;
                pikminEaten.GetComponent<Pikmin>().Death();

            }
        }


    }

    private void ResetAttack()
    {

        alreadyAttacked = false;
    }

    IEnumerator DoneAttacking()
    {
        agent.SetDestination(transform.position);
        canMove = false;
        yield return new WaitForSeconds(3);
        anim.SetBool("AttackSuccessful", false);
        canMove = true;
    }

    public IEnumerator Death()
    {
        agent.SetDestination(transform.position);
        //anim.SetBool("IsAttacking", false);
        //anim.SetBool("EnteredRadius", false);
        
        canMove = false;
        anim.SetBool("IsDead", true);
        deathParticles.Play();
        yield return new WaitForSeconds(4);
        deathParticles.enableEmission = false;
        canMove = false;
        gameObject.layer = 8;
        gameObject.GetComponent<CarryObject>().canBeCarried = true;
        

    }

}
