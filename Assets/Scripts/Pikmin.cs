using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

[SelectionBase]
public class Pikmin : MonoBehaviour
{

   
    public enum State { Idle, Follow, Interact, Fighting, Dead, Drinking, Recoil, Panic }

    [HideInInspector]
    public NavMeshAgent agent = default;
    private Coroutine updateTarget = default;
    public State state = default;
    public InteractiveObject objective;
    public EnemyHealth objectiveEnemy;
    public PikminManager pikminManager;
    public Transform nectar;
    public LayerMask whatIsGround;
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange = 3;
    public bool isFlying;
    public bool isGettingIntoPosition;
    public bool cooldown;
    public bool isFlowered;
    public bool isHoneySprayed;
    public bool isSpicySprayed;
    private Transform leaf;  //Parts of pikmin 
    private Transform flower;
    private Rigidbody rigidbody;
    private Light light;


    [Header("What Color Pikmin? (Red,Yellow,Blue,Cyan)")]
    public string pikminColor;

    public Transform enemy;
    public float baseDamage;
    private float damage;
    public float knockBack;
    public Vector3 recoilDirection;

    public PikminEvent OnStartFollow;
    public PikminEvent OnStartThrow;
    public PikminEvent OnEndThrow;
    public PikminEvent OnStartCarry;
    public PikminEvent OnEndCarry;
    public PikminEvent OnHit;
    public PikminEvent OnDeath;
    public PikminEvent OnNectar;
    public PikminEvent OnWah;
    public PikminEvent OnFire;

    private Animator anim;
    private PikminVisualHandler visualHandler;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
        visualHandler = GetComponent<PikminVisualHandler>();
        pikminManager = FindObjectOfType<PikminManager>();
        anim = GetComponent<Animator>();
        if ( pikminColor != "Cyan")
        {
            flower = this.gameObject.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(1);//Find("PikminFlower");
            leaf = this.gameObject.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(2);//Find("StemLeaf");
        }
       else light = this.gameObject.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(3).gameObject.GetComponent<Light>();

        ChangeLeafState();
    }
   
    public void SetTarget(Transform target, float updateTime = 1f)
    {
        if (state == State.Interact)
        {
            transform.parent = null;
            agent.enabled = true;
            objective.ReleasePikmin();
            objective = null;
        }
        
        state = State.Follow;
        agent.stoppingDistance = 1f;

        OnStartFollow.Invoke(0);

        if (updateTarget != null)
            StopCoroutine(updateTarget);

        WaitForSeconds wait = new WaitForSeconds(updateTime);
        updateTarget = StartCoroutine(UpdateTarget());

        IEnumerator UpdateTarget()
        {
            while (true)
            {
                if(agent.enabled)
                    agent.SetDestination(target.position);
                yield return wait;
            }
        }
    }
    public void Throw(Vector3 target, float time, float delay)
    {
        OnStartThrow.Invoke(0);

        isFlying = true;
        state = State.Idle;

        if (updateTarget != null)
            StopCoroutine(updateTarget);

        agent.stoppingDistance = 0f;
        agent.enabled = false;

        transform.DOJump(target, 2, 1, time).SetDelay(delay).SetEase(Ease.Linear).OnComplete(() =>
        {
            agent.enabled = true;
            isFlying = false;
            CheckInteraction();

            OnEndThrow.Invoke(0);
        });

        transform.LookAt(new Vector3(target.x, transform.position.y, target.z));
        visualHandler.model.DOLocalRotate(new Vector3(360 * 3, 0, 0), time, RotateMode.LocalAxisAdd).SetDelay(delay);

    }

    public void SetCarrying(bool on)
    {
        if (on)
            OnStartCarry.Invoke(0);
        else
            OnEndCarry.Invoke(0);
    }

    public void SetIdle()
    {
        objective = null;
        agent.enabled = true;
        transform.parent = null;
        state = State.Idle;
        OnEndThrow.Invoke(0);
    }

    public void SetDismiss(Vector3 target, float updateTime = 1f)
    {
        //agent.SetDestination(target);
    }

    public IEnumerator Dismiss()
    {
        yield return new WaitForSeconds(0);
        agent.enabled = false;
        state = State.Idle;
        OnEndThrow.Invoke(0);
       

    }

    public void ChangeLeafState()
    {
        if (isFlowered == true)
        {
            if(pikminColor == "Cyan")
            {
                agent.speed = 8.0f;
                damage = baseDamage + 0.25f;
                light.enabled = true;
            }

            else
            {
                agent.speed = 8.0f;
                
                leaf.GetComponent<MeshRenderer>().enabled = false;
                damage = baseDamage + 0.25f;
                foreach (MeshRenderer r in flower.GetComponentsInChildren<MeshRenderer>())
                    r.enabled = true;
            }
            if (pikminColor == "Yellow") agent.speed = 8.5f;

        }

        if (isFlowered == false)
        {
            if (pikminColor == "Cyan")
            {
                agent.speed = 5.0f;
                damage = baseDamage;
                light.enabled = false;
            }

            
            else
            {
                agent.speed = 5.0f;
                damage = baseDamage;
                if (pikminColor != "Cyan") 
                {
                    leaf.GetComponent<MeshRenderer>().enabled = true;

                    foreach (MeshRenderer r in flower.GetComponentsInChildren<MeshRenderer>())
                        r.enabled = false;
                }
                
            }
            if (pikminColor == "Yellow") agent.speed = 6.0f;

        }
    }

    public void CheckInteraction()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);

        if (colliders.Length == 0)
            return;

        foreach (Collider collider in colliders)
        {
            
                if (collider.gameObject.layer == 8)
                {
                    objective = collider.GetComponent<InteractiveObject>();
                    if(objective.currentPikmin >= objective.maxPikmin)
                    {
                       objective = null;
                       return;
                    }
                    collider.GetComponent<CarryObject>().seedColor = pikminColor;
                    objective.fractionColor = pikminColor;
                    objective.AssignPikmin();
                    StartCoroutine(GetInPosition());

                    break;
                }

                if (collider.gameObject.tag == "Attackable" && state != State.Panic)
                {
                    if ( isFlying == true)
                {
                    //code for latching
                    //break
                }
                    enemy = collider.transform;
                    objectiveEnemy = enemy.GetComponentInParent<EnemyHealth>();
                    state = State.Fighting;
                    transform.LookAt(enemy);
                    agent.enabled = false;

                    break;

                }

                if (collider.gameObject.tag == "Nectar" && isFlowered == false)
                {
                   if (state == State.Follow) pikminManager.PikminDeath();
                   state = State.Drinking;
                   nectar = collider.transform;
                   transform.LookAt(nectar);
                   agent.enabled = false;
                   StartCoroutine("Nectar");
                   break;
                }
           
                
        }

        OnEndThrow.Invoke(0);

        IEnumerator GetInPosition()
        {
            isGettingIntoPosition = true;

            agent.SetDestination(objective.GetPositon());
            StartCoroutine("GetInPositionTimer");
            yield return new WaitUntil(() => agent.IsDone());
            agent.enabled = false;
            state = State.Interact;

            if (objective != null)
            {
                transform.parent = objective.transform;
                transform.DOLookAt(new Vector3(objective.transform.position.x, transform.position.y, objective.transform.position.z), .2f);
            }

            if (objective == null) state = State.Idle;

            isGettingIntoPosition = false;
        }

        IEnumerator GetInPositionTimer()
        {
            yield return new WaitForSeconds(4);
            if (isGettingIntoPosition == true)
            {
                state = State.Idle;
                isGettingIntoPosition = false;
            }
        }
        
    }
    void Update()
    {
        if (state == State.Fighting && cooldown == false && objectiveEnemy != null)
        {
            if (agent.enabled == true) agent.SetDestination(enemy.position);
            StartCoroutine(Combat());
        }

        if (state == State.Follow)
        {
            objectiveEnemy = null;
            agent.enabled = true;
            cooldown = false;
        }
        
        if (state == State.Dead)
        {
            agent.enabled = false;
        }

        if (state == State.Panic) PanicRun();
        if (state != State.Fighting) anim.SetBool("isAttacking", false);
    }
    

    void OnTriggerEnter(Collider other)
    {
        if (state == State.Fighting)
        {
            agent.enabled = false;
        }


    }
    void OnTriggerExit(Collider other)
    {
        if (state == State.Fighting)
        {
            agent.enabled = true;
        }

    }
    IEnumerator Nectar()
    {
        OnNectar.Invoke(0);
        yield return new WaitForSeconds(1);
        if (nectar != null && state == State.Drinking)
        {
            isFlowered = true;
            ChangeLeafState();
        }
        state = State.Idle;
    }
    public IEnumerator Drop()
    {
        agent.enabled = false;
        rigidbody.isKinematic = false;
        gameObject.GetComponent<BoxCollider>().enabled = true;
        rigidbody.AddForce(Vector3.down * 3000);
        yield return new WaitForSeconds(1f);
        rigidbody.isKinematic = true;
        agent.enabled = true;
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }
    IEnumerator Recoil()
    {
        
        OnWah.Invoke(0);
        //The code below is used to remove pikmin from the current pikmin total if they were following
        if (state == State.Follow) FindObjectOfType<PikminManager>().PikminDeath();

        state = State.Recoil;
        agent.enabled = false;
        rigidbody.isKinematic = false;
        gameObject.GetComponent<BoxCollider>().enabled = true;
        //rigidbody.AddForce((transform.forward * -1) * knockBack);
        rigidbody.AddForce((recoilDirection) * knockBack);
        yield return new WaitForSeconds(1.5f);
        rigidbody.AddForce(-Vector3.up * 3000);
        yield return new WaitForSeconds(0.5f);
        rigidbody.isKinematic = true;
        agent.enabled = true;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        objectiveEnemy = null;
        SetIdle();

    }
    IEnumerator Combat()
    {
        anim.SetBool("isAttacking", true);
        OnHit.Invoke(0);
        objectiveEnemy.health -= damage;
        cooldown = true;
        yield return new WaitForSeconds(1);

        if (objectiveEnemy.health <= 0)
        {
            anim.SetBool("isAttacking", false);
            objectiveEnemy = null;
            //rigidbody.isKinematic = false;
            agent.enabled = false;
            //yield return new WaitForSeconds(0.5f);
            //rigidbody.isKinematic = true;
            agent.enabled = true;
            yield return new WaitForSeconds(4);
            if (state == State.Fighting && objectiveEnemy == null) 
            {
                state = State.Idle;
                CheckInteraction();
            } 
        }
        cooldown = false;
    }
    public void Death()
    {
        if (state == State.Follow) FindObjectOfType<PikminManager>().PikminDeath();
     
        if (state == State.Interact) objective.ReleasePikmin();
      
        if (state == State.Fighting) objectiveEnemy = null;
        state = State.Dead;
        foreach (MeshRenderer r in GetComponentsInChildren<MeshRenderer>())
            r.enabled = false;
        print("I was eaten");
        OnDeath.Invoke(0);
        StartCoroutine("DeathFinish");
        GetComponent<Collider>().enabled = false;
        FindObjectOfType<PikminManager>().Remove(this);
        if (pikminColor == "Cyan") light.enabled = false;
    }
    IEnumerator DeathSuccumb()
    {
        if (state == State.Follow) FindObjectOfType<PikminManager>().PikminDeath();

        if (state == State.Interact) objective.ReleasePikmin();

        if (state == State.Fighting) objectiveEnemy = null;
        state = State.Dead;
        anim.SetBool("hasSuccumbed", true);
        yield return new WaitForSeconds(2);
        foreach (MeshRenderer r in GetComponentsInChildren<MeshRenderer>())
            r.enabled = false;
        OnDeath.Invoke(0);
        StartCoroutine("DeathFinish");
        GetComponent<Collider>().enabled = false;
        FindObjectOfType<PikminManager>().Remove(this);
        if (pikminColor == "Cyan") light.enabled = false;
    }
    public void EnterOnion()
    {
        //OnDeath.Invoke(0);
        
        pikminManager.PikminDeath();
        pikminManager.Remove(this);
        Destroy(gameObject);
    }
    IEnumerator DeathFinish()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
    IEnumerator PanicFire()
    {
        if (state != State.Dead && pikminColor != "Red" && state != State.Panic)
        {
            if (state == State.Interact) objective.ReleasePikmin();
            if (state == State.Fighting) objectiveEnemy = null;
            if (state == State.Follow) FindObjectOfType<PikminManager>().PikminDeath();
            
            OnFire.Invoke(0);
            state = State.Panic;
            

            yield return new WaitForSeconds(5);
            if (state == State.Panic) StartCoroutine("DeathSuccumb");
        }

        

    }
    public void PanicRun()
    {
        
        if (!walkPointSet) SearchPanicPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    public void SearchPanicPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        OnFire.Invoke(0);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }
    public void Celebrate()
    {
        anim.SetBool("isCelebrate", true);
        StartCoroutine("CelebrateOff");
    }
    IEnumerator CelebrateOff()
    {
        yield return new WaitForSeconds(1);
            anim.SetBool("isCelebrate", false);
    }
}
