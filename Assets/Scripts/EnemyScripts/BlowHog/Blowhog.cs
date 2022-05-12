using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Blowhog : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform pikmin;


    public LayerMask whatIsGround, whatIsPikmin;

    private Animator anim;

    EnemyHealth enemyHealth;
    FireHazard fireHazard;

    //From other scripts
    public Transform Enemy;


    private Transform nearestPikmin;
    private int pikminLayer;
    

    public Vector3 recoilDirection;
    public float thrust = 6;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    public float shootRange;
    bool alreadyAttacked;
    bool canMove;
    bool isDead;
    public bool pikminAttacking;
    RaycastHit hit;

    //Particles
    public ParticleSystem deathParticles;
    public ParticleSystem fireParticles;

    //States
    public float sightRange, attackRange;
    public bool pikminInSightRange, pikminInAttackRange;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();

        enemyHealth = GetComponent<EnemyHealth>();
        fireHazard = GetComponent<FireHazard>();

        pikminLayer = LayerMask.NameToLayer("Pikmin");
        //Debug.Log(pikminLayer);
        canMove = true;
        isDead = false;
    }

    private void Update()
    {

        
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
            pikminAttacking = Physics.CheckSphere(transform.position, 2, whatIsPikmin);
        }

        if (pikminAttacking) anim.SetBool("BeingAttacked", true);
        if (!pikminAttacking) anim.SetBool("BeingAttacked", false);
    }


    private void Patroling()
    {
        if (isDead == false) canMove = true;

        if (!walkPointSet) StartCoroutine("SearchWalkPoint");

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        anim.SetBool("IsAttacking", false);
        anim.SetBool("EnteredRadius", false);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    IEnumerator SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        anim.SetBool("LookingAround", true);
        yield return new WaitForSeconds(1);
        anim.SetBool("LookingAround", false);
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
        
        anim.SetBool("IsAttacking", true);
        if (canMove == true)
        {
            agent.SetDestination(transform.position);
            transform.LookAt(pikmin);
        }
        canMove = false;
        if (!alreadyAttacked)
        {
            //Attack code here

            anim.SetBool("IsAttacking", false);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    public void Attacking()
    {
        fireHazard.isActive = true;
        fireParticles.Play();
    }

    public void DoneAttacking()
    {
        fireHazard.isActive = false;
        fireParticles.Stop();
        canMove = true;
    }

    private void ResetAttack()
    {

        alreadyAttacked = false;
    }

    public void ShakingOff()
    {
        agent.SetDestination(transform.position);
        canMove = false;
        
        
            recoilDirection = transform.forward;
            Collider[] attackingPikmin = Physics.OverlapSphere(Enemy.position, 2, 1 << pikminLayer);
            foreach (Collider collider in attackingPikmin)
            {
                collider.GetComponent<Pikmin>().knockBack = thrust;
                collider.GetComponent<Pikmin>().recoilDirection = recoilDirection;
                collider.GetComponent<Pikmin>().StartCoroutine("Recoil");
            }
    }

    public void CanMove()
    {
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
        yield return new WaitForSeconds(3);
        deathParticles.enableEmission = false;
        canMove = false;
        gameObject.layer = 8;
        gameObject.GetComponent<CarryObject>().canBeCarried = true;


    }

}
  
