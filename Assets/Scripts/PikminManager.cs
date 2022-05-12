using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Animations.Rigging;
using TMPro;
using System.Linq;


[System.Serializable] public class PikminEvent : UnityEvent<int> { }
[System.Serializable] public class PlayerEvent : UnityEvent<Vector3> { }

[RequireComponent(typeof(PikminController))]
public class PikminManager : MonoBehaviour
{
    private MovementInput charMovement;

    [Header("Positioning")]
    public Transform pikminThrowPosition;

    [Header("Targeting")]
    [SerializeField] private Transform target = default;
    [SerializeField] private PikminController controller = default;
    [SerializeField] private float selectionRadius = 1;

    [Header("Spawning")]
    [SerializeField] private Pikmin pikminPrefab = default;

    [Header("Events")]
    public PikminEvent pikminFollow;
    public PlayerEvent pikminHold;
    public PlayerEvent pikminThrow;
    public PikminEvent pikminDied;

    public static List<Pikmin> allPikmin = new List<Pikmin>();
    public int controlledPikmin = 0;

    public bool gameIsPaused;
    float dismissRange = 6;
    public Vector3 dismissPointRed;
    public Vector3 dismissPointYellow;
    public Vector3 dismissPointBlue;
    public Vector3 dismissPointCyan;

    [Header("What Color Pikmin will be thrown? (Red,Yellow,Blue,Cyan,Any)")]
    public string pikminColor;
    private InterfaceManager interfaceManager;

    public Rig whistleRig;
    public ParticleSystem whistlePlayerParticle;

    public AudioClip dismissWhistle;
    public AudioClip nameSwitch;

    //Poko Value Tracking
    public int pokototal = 0;
    public TextMeshProUGUI pokoText;

    #region Singleton

    public static PikminManager instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    public GameObject Pikjammo;

    // Start is called before the first frame update
    void Start()
    {
        charMovement = FindObjectOfType<MovementInput>();
        interfaceManager = GameObject.Find("MainCanvas").GetComponent<InterfaceManager>();
        pikminColor = "Any";
        PikminSpawner[] spawners = FindObjectsOfType(typeof(PikminSpawner)) as PikminSpawner[];
        foreach (PikminSpawner spawner in spawners)
        {
            spawner.SpawnPikmin(pikminPrefab, ref allPikmin);
        }
    }

    public void SetWhistleRadius(float radius)
    {
        selectionRadius = radius;
    }

    public void SetWhistleRigWeight(float weight)
    {
        whistleRig.weight = weight;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            //Time.timeScale = Time.timeScale == 1 ? .2f : 1;

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);

        //Spray Functionality
        if (Input.GetKeyDown(KeyCode.I))
        {
            if ( CarryObject.honeyspray >= 1)
            {

            }

        }
        
        if (gameIsPaused == false)
        {
            if (Input.GetMouseButton(1))
            {
                foreach (Pikmin pikmin in allPikmin)
                {
                    if (Vector3.Distance(pikmin.transform.position, controller.hitPoint) < selectionRadius)
                    {
                        if (pikmin.state != Pikmin.State.Follow && pikmin.state != Pikmin.State.Dead && pikmin.state != Pikmin.State.Drinking && pikmin.state != Pikmin.State.Recoil)
                        {
                            if (pikmin.isFlying || pikmin.isGettingIntoPosition)
                                return;

                            StartCoroutine(pikmin.Drop());
                            pikmin.SetTarget(target, 0.25f);
                            controlledPikmin++;
                            pikminFollow.Invoke(controlledPikmin);
                        }
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {

               
                foreach (Pikmin pikmin in allPikmin)
                {
                     allPikmin = allPikmin.OrderBy(x => Vector3.Distance(pikmin.transform.position, x.transform.position)).ToList();
                    if (pikmin.state == Pikmin.State.Follow && Vector3.Distance(pikmin.transform.position, charMovement.transform.position) < 2)
                    {
                        if (pikminColor == "Any")
                        {
                            pikmin.agent.enabled = false;
                            float delay = .05f;
                            pikmin.transform.DOMove(pikminThrowPosition.position, delay);

                            pikmin.Throw(controller.hitPoint, .5f, delay);
                            controlledPikmin--;

                            pikminThrow.Invoke(controller.hitPoint);
                            pikminFollow.Invoke(controlledPikmin);
                            break;
                        }

                        else
                        {
                            if(pikmin.pikminColor == pikminColor)
                            {
                                pikmin.agent.enabled = false;
                                float delay = .05f;
                                pikmin.transform.DOMove(pikminThrowPosition.position, delay);

                                pikmin.Throw(controller.hitPoint, .5f, delay);
                                controlledPikmin--;

                                pikminThrow.Invoke(controller.hitPoint);
                                pikminFollow.Invoke(controlledPikmin);
                                break;
                            }
                        }
                    }
                }
            }
            
            //Change which pikmin can be thrown
            if (Input.GetKeyDown(KeyCode.R)) 
            {
                if (pikminColor == "Any")
                {
                    pikminColor = "Red";
                }
                else if (pikminColor == "Red")
                {
                    pikminColor = "Yellow";
                }
                else if (pikminColor == "Yellow")
                {
                    pikminColor = "Blue";
                }
                else if (pikminColor == "Blue")
                {
                    pikminColor = "Cyan";
                }
                else if (pikminColor == "Cyan")
                {
                    pikminColor = "Any";
                }
                interfaceManager.UpdatePikminSelected(pikminColor);
                controller.audio.PlayOneShot(nameSwitch, 0.7f);
            }

            if (Input.GetMouseButtonDown(1))
                SetWhistleCylinder(true);

            if (Input.GetMouseButtonUp(1))
                SetWhistleCylinder(false);

            if (Input.GetKeyDown(KeyCode.C))
            {
                //dismiss current pikmin
                controller.audio.PlayOneShot(dismissWhistle, 0.7f);
                foreach (Pikmin pikmin in allPikmin)
                {
                    if (pikmin.state == Pikmin.State.Follow)
                    {
                        float randomZ = Random.Range(-dismissRange, dismissRange);
                        float randomX = Random.Range(-dismissRange, dismissRange);
                        dismissPointRed = new Vector3(target.position.x + randomX, target.position.y, target.position.z + randomZ);
                        dismissPointYellow = new Vector3(target.position.x + randomX, target.position.y, target.position.z + randomZ);



                        if (pikmin.pikminColor == "Red") pikmin.SetDismiss(dismissPointRed, 0.25f);
                        if (pikmin.pikminColor == "Yellow") pikmin.SetDismiss(dismissPointYellow, 0.25f);

                        //pikmin.SetTarget(target, 
                        controlledPikmin--;
                        pikminFollow.Invoke(controlledPikmin);
                        pikmin.StartCoroutine("Dismiss");
                    }

                }
            }
        }
        

    }
    public void FinishInteraction(InteractiveObject objective)
    {
        foreach (Pikmin pikmin in allPikmin)
        {
            if (pikmin.objective == objective)
            {
                pikmin.SetCarrying(false);
                pikmin.SetIdle();
                pikmin.Celebrate();
            }
        }
    }

    public void StartIntetaction(InteractiveObject objective)
    {
        foreach (Pikmin pikmin in allPikmin)
        {
            if (pikmin.objective == objective)
            {
                pikmin.SetCarrying(true);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controller.target.position, selectionRadius);
    }

    //for when a pikmin dies while in player control
    public void PikminDeath() 
    {
        controlledPikmin--;
        pikminDied.Invoke(controlledPikmin);
    }

    //for when a pikmin dies, removes specific pikmin from list
    public void Remove(Pikmin pikmin) 
    {
        allPikmin.Remove(pikmin);
    }

    //for when a pikmin enters the onion
    public void PikminEnterOnion()
    {
        controlledPikmin = controlledPikmin - 1;
        pikminDied.Invoke(controlledPikmin);
    }

   




    //Polish
    public void SetWhistleCylinder(bool on)
    {
        if (on)
        {
            whistlePlayerParticle.Play();
            controller.audio.Play();
            DOVirtual.Float(0, (5 / 2) + .5f, .5f, SetWhistleRadius).SetId(2);
            DOVirtual.Float(0, 1, .2f, SetWhistleRigWeight).SetId(1);

            charMovement.transform.GetChild(0).DOScaleY(27f, .05f).SetLoops(-1, LoopType.Yoyo).SetId(3);

            controller.visualCylinder.localScale = Vector3.zero;
            controller.visualCylinder.DOScaleX(5, .5f);
            controller.visualCylinder.DOScaleZ(5, .5f);
            controller.visualCylinder.DOScaleY(2, .4f).SetDelay(.4f);
        }
        else
        {

            whistlePlayerParticle.Stop();
            controller.audio.Stop(); DOTween.Kill(2); DOTween.Kill(1); DOTween.Kill(3);
            charMovement.transform.GetChild(0).DOScaleY(28, .1f);
            DOVirtual.Float(whistleRig.weight, 0, .2f, SetWhistleRigWeight);
            selectionRadius = 0;
            controller.visualCylinder.DOKill();
            controller.visualCylinder.DOScaleX(0, .2f);
            controller.visualCylinder.DOScaleZ(0, .2f);
            controller.visualCylinder.DOScaleY(0f, .05f);
        }
    }

}
