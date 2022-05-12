using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

[SelectionBase]
public class CarryObject : InteractiveObject
{
    [SerializeField] private DestinationScript destination;
    private NavMeshAgent agent = default;
    private Coroutine destinationRoutine = default;
    private float originalAgentSpeed;
    private Renderer objectRenderer;
    private Collider collider;
    [SerializeField] private Vector3 destinationOffset;
    [SerializeField] [ColorUsage(false,true)] private Color captureColor;
    //Poko Values
    [Header("Poko Values")]
    public int pokovalue = 5;
    [SerializeField] public static int pokototal;
    public TextMeshProUGUI PokoText;

    //Spray Values
    [Header("Spray Values")]
    public TextMeshProUGUI HoneySprayText;
    public double honeysprayvalue = 0.0;
    [SerializeField] public static double honeyspraystored = 0.0;
    [SerializeField] public static int honeyspray = 0;

    //Seed Values
    [Header("Seed Values")]
    public int seedWorth = 0;
    //Color of seeds that will spawn (Red,Yellow,Blue,Cyan)
    public string seedColor;

    //Enemy Carrying
    public bool canBeCarried = true;

    //Destination Onion or Ship
    public bool GoesToOnion = false;
    private GameObject onion;
    private GameObject ship;
    private Onion onionScript = null;



    public override void Initialize()
    {
        base.Initialize();
        onion = GameObject.Find("GreenOnion");
        ship = GameObject.Find("SpaceCapsule");
        if (GoesToOnion == true)
        {
            destination = onion.GetComponent<DestinationScript>();
            onionScript = onion.GetComponent<Onion>();
        }
        else
        {
            destination = ship.GetComponent<DestinationScript>();
        }
        
        objectRenderer = GetComponentInChildren<Renderer>();
        agent = GetComponent<NavMeshAgent>();
        collider = GetComponent<Collider>();
        originalAgentSpeed = agent.speed;
    }
    public override void Interact()
    {
        if (canBeCarried == true)
        {
            if (destinationRoutine != null)
                StopCoroutine(destinationRoutine);

            agent.enabled = true;
            destinationRoutine = StartCoroutine(GetInPosition());

            IEnumerator GetInPosition()
            {
                (FindObjectOfType(typeof(PikminManager)) as PikminManager).StartIntetaction(this);

                agent.avoidancePriority = 50;
                agent.isStopped = false;
                agent.SetDestination(destination.Point());
                yield return new WaitUntil(() => agent.IsDone());
                agent.enabled = false;
                //collider.enabled = false;

                (FindObjectOfType(typeof(PikminManager)) as PikminManager).FinishInteraction(this);

                //Delete UI
                if (fractionObject != null)
                    Destroy(fractionObject);

                //Capture Animation
                float time = 1.3f;
                Sequence s = DOTween.Sequence();
                s.AppendCallback(() => destination.StartCapture());
                s.Append(objectRenderer.material.DOColor(captureColor, "_EmissionColor", time));
                s.Join(transform.DOMove(destination.transform.position, time).SetEase(Ease.InQuint));
                s.Join(transform.DOScale(0, time).SetEase(Ease.InQuint));
                s.AppendCallback(() => destination.FinishCapture());
                if (GoesToOnion == true)
                {

                }
                else
                {
                    s.Append(destination.transform.DOPunchScale(-Vector3.one * 35, .5f, 10, 1));
                }
            }
        }
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "FruitCollision")
        {
            print("I Collided");
            pokototal = pokototal + pokovalue;
            PokoText.transform.DOComplete();
            PokoText.transform.DOPunchScale(Vector3.one / 3, .3f, 10, 1);
            PokoText.text = pokototal.ToString();
            honeyspraystored = honeyspraystored + honeysprayvalue;
            if (honeyspraystored == 1.0)
            {
                honeyspray = honeyspray + 1;
                HoneySprayText.transform.DOComplete();
                HoneySprayText.transform.DOPunchScale(Vector3.one / 3, .3f, 10, 1);
                HoneySprayText.text = honeyspray.ToString();
                honeyspraystored = honeyspraystored - 1;
            }



            Destroy(gameObject);
        }

        if (other.gameObject.name == "OnionCollision")
        {
            //onionScript.pikminSeedRed += seedWorth;
            if (seedColor == "Red")
            {
                onionScript.pikminSeedRed += seedWorth;
            }

            if (seedColor == "Yellow")
            {
                onionScript.pikminSeedYellow += seedWorth;
            }

            if (seedColor == "Blue")
            {
                onionScript.pikminSeedBlue += seedWorth;
            }

            if (seedColor == "Cyan")
            {
                onionScript.pikminSeedCyan += seedWorth;
            }

            Destroy(gameObject);
        }
        
       

    }

    

    public override void UpdateSpeed(int extra)
    {
        agent.speed = (extra > 0) ? originalAgentSpeed + (extra * .2f) : originalAgentSpeed;
    }

    public override void StopInteract()
    {
        agent.avoidancePriority = 30;
        agent.isStopped = true;
        if(destinationRoutine != null)
            StopCoroutine(destinationRoutine);
    }

    private void Update()
    {
        if(fractionObject != null)
            fractionObject.transform.position = Camera.main.WorldToScreenPoint(transform.position + uiOffset);
    }
    // added as a test variable 
    //static class Butter
    //{
        //public static int score;
    //}
}
