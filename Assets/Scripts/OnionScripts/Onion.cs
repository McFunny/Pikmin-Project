using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;

public class Onion : MonoBehaviour
{
    public int totalPikmin;
    public int pikminOutside;
    public int pikminSeedOutside;
    public bool underOnion = false;
    private GameObject onionHudObject;
    private GameObject pikminManager;
    private GameObject caveHudObject;
    private InterfaceManager interfaceManager;
    private PikminManager pikminManagerScript;
    public float spawnRadius = 1;
    [SerializeField] private Pikmin redPikmin = default;
    [SerializeField] private Pikmin yellowPikmin = default;
    [SerializeField] private Pikmin bluePikmin = default;
    [SerializeField] private Pikmin cyanPikmin = default;
    public List<Pikmin> allPikmin = new List<Pikmin>();

    [System.Serializable] public class OnionEvent : UnityEvent<int> { }

    private AudioSource audioSource;
    public AudioClip seedSpawn;
    public AudioClip buttonPress;
    public AudioClip buttonDeny;
    public AudioClip menuOpen;
    public AudioClip menuClose;

    [Header("Pikmin In Onion")]
    //Total of Pikmin type within Onion
    [SerializeField] public int pikminRed;
    [SerializeField] public int pikminYellow;
    [SerializeField] public int pikminBlue;
    [SerializeField] public int pikminCyan;
    
    [Header("List of Pikmin Following Player")]
    //Total of Pikmin type currently following
    public int pikminFollowingRed;
    public int pikminFollowingYellow;
    public int pikminFollowingBlue;
    public int pikminFollowingCyan;

    [Header("List of Pikmin to be taken out or put in Onion")]
    //If InOut is negative, Pikmin enter Onion. If positive, Pikmin exit Onion
    public int pikminInOutRed = 0;
    public int pikminInOutYellow = 0;
    public int pikminInOutBlue = 0;
    public int pikminInOutCyan = 0;

    [Header("Pikmin Seeds To Eject")]
    //How many seeds to eject of each color
    public int pikminSeedRed = 0;
    public int pikminSeedYellow = 0;
    public int pikminSeedBlue = 0;
    public int pikminSeedCyan = 0;

    [Header("List of Flowered Pikmin")]
    //How many of the Pikmin in the Onion are flowered;
    [SerializeField] public int pikminFlowerRed = 0;
    [SerializeField] public int pikminFlowerYellow = 0;
    [SerializeField] public int pikminFlowerBlue = 0;
    [SerializeField] public int pikminFlowerCyan = 0;

    [Header("Particle Systems")]
    public ParticleSystem seedParticleRed;
    public ParticleSystem seedParticleYellow;
    public ParticleSystem seedParticleBlue;
    public ParticleSystem seedParticleCyan;

  


    public void Awake()
    {
        onionHudObject = GameObject.Find("OnionUI");
        onionHudObject.SetActive(false);
        pikminManager = GameObject.Find("Pikmin Manager");
        pikminManagerScript = pikminManager.GetComponent<PikminManager>();
        interfaceManager = GameObject.Find("MainCanvas").GetComponent<InterfaceManager>();
        audioSource = GetComponent<AudioSource>();
        allPikmin = PikminManager.allPikmin;
    }

    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            if (underOnion == true && pikminManagerScript.gameIsPaused == false)
            {
                pikminOutside = 0 + pikminSeedOutside;
                allPikmin = PikminManager.allPikmin;
                foreach (Pikmin pikmin in allPikmin)
                {
                    pikminOutside++;
                    if (pikmin.state == Pikmin.State.Follow && pikmin.pikminColor == "Red")
                    {
                        pikminFollowingRed++;
                    }
                    if (pikmin.state == Pikmin.State.Follow && pikmin.pikminColor == "Yellow")
                    {
                        pikminFollowingYellow++;
                        //print("Ding");
                    }
                    if (pikmin.state == Pikmin.State.Follow && pikmin.pikminColor == "Blue")
                    {
                        pikminFollowingBlue++;
                    }
                    if (pikmin.state == Pikmin.State.Follow && pikmin.pikminColor == "Cyan")
                    {
                        pikminFollowingCyan++;
                    }
                }
                audioSource.PlayOneShot(menuOpen);
                interfaceManager.UpdateTotalOnion(totalPikmin);
                interfaceManager.UpdatePikminOutside(pikminOutside);
                interfaceManager.UpdateRedInside(pikminRed);
                interfaceManager.UpdateRedOutside(pikminFollowingRed);
                interfaceManager.UpdateYellowInside(pikminYellow);
                interfaceManager.UpdateYellowOutside(pikminFollowingYellow);
                interfaceManager.UpdateBlueInside(pikminBlue);
                interfaceManager.UpdateBlueOutside(pikminFollowingBlue);
                interfaceManager.UpdateCyanInside(pikminCyan);
                interfaceManager.UpdateCyanOutside(pikminFollowingCyan);
                pikminManagerScript.gameIsPaused = true;
                Time.timeScale = 0;
                onionHudObject.SetActive(true);
               
            }
              


        if (pikminSeedRed > 0)
        {
            
            if (pikminOutside < 100)
            {
                seedParticleRed.Play();
                pikminSeedOutside += 1;
                audioSource.PlayOneShot(seedSpawn, 0.5f);
            }

            else
            {
                totalPikmin += 1;
                pikminRed += 1;
              
            }
            pikminSeedRed--;
        }

        if (pikminSeedYellow > 0)
        {
            if (pikminOutside < 100)
            {
                seedParticleYellow.Play();
                pikminSeedOutside += 1;
                audioSource.PlayOneShot(seedSpawn, 0.5f);
            }

            else
            {
                totalPikmin += 1;
                pikminYellow += 1;

            }
            pikminSeedYellow--;
        }

        if (pikminSeedBlue > 0)
        {
            if (pikminOutside < 100)
            {
                seedParticleBlue.Play();
                pikminSeedOutside += 1;
                audioSource.PlayOneShot(seedSpawn, 0.5f);
            }

            else
            {
                totalPikmin += 1;
                pikminBlue += 1;

            }
            pikminSeedBlue--;
        }

        if (pikminSeedCyan > 0)
        {
            if (pikminOutside < 100)
            {
                seedParticleCyan.Play();
                pikminSeedOutside += 1;
                audioSource.PlayOneShot(seedSpawn, 0.5f);
            }

            else
            {
                totalPikmin += 1;
                pikminCyan += 1;

            }
            pikminSeedCyan--;
        }


    }

    //To check if player is under Onion
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            underOnion = true;
        }
        
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            underOnion = false;
        }
    }

    public void Done()
    {
        Time.timeScale = 1;
        onionHudObject.SetActive(false);
        pikminManagerScript.gameIsPaused = false;
        pikminFollowingRed = 0;
        pikminFollowingYellow = 0;
        pikminFollowingBlue = 0;
        pikminFollowingCyan = 0;
        audioSource.PlayOneShot(menuClose);

        if (pikminInOutRed != 0)
        {
            RedPikminEnterExit(redPikmin, ref allPikmin);
        }
        if (pikminInOutYellow != 0)
        {
            YellowPikminEnterExit(yellowPikmin, ref allPikmin);
        }
        if (pikminInOutBlue != 0)
        {
            BluePikminEnterExit(bluePikmin, ref allPikmin);
        }
        if (pikminInOutCyan != 0)
        {
            CyanPikminEnterExit(cyanPikmin, ref allPikmin);
        }



    }

    public void RedPikminEnterExit(Pikmin redPikmin, ref List<Pikmin> allPikmin)
    {
        for (int i = 0; i < pikminInOutRed; i++)
        {
            Pikmin newPikmin = Instantiate(redPikmin);
            newPikmin.transform.position = transform.position + (Random.insideUnitSphere * spawnRadius);
            PikminManager.allPikmin.Add(newPikmin);
            if (pikminFlowerRed > 0)
            {
                newPikmin.GetComponent<Pikmin>().isFlowered = true;
                newPikmin.GetComponent<Pikmin>().ChangeLeafState();
                pikminFlowerRed--;
            }
        }
        
        for (int i = 0; i > pikminInOutRed; i--)
        {
           
            foreach (Pikmin pikmin in allPikmin)
            {
                if (pikmin.state == Pikmin.State.Follow && pikmin.pikminColor == "Red")
                {
                    allPikmin.Remove(pikmin);
                    pikmin.EnterOnion();
                    if (pikmin.isFlowered == true) pikminFlowerRed++;
                    break;
                }
            }
        }
        pikminInOutRed = 0;
    }

    public void YellowPikminEnterExit(Pikmin yellowPikmin, ref List<Pikmin> allPikmin)
    {
        for (int i = 0; i < pikminInOutYellow; i++)
        {
            Pikmin newPikmin = Instantiate(yellowPikmin);
            newPikmin.transform.position = transform.position + (Random.insideUnitSphere * spawnRadius);
            PikminManager.allPikmin.Add(newPikmin);
            if (pikminFlowerYellow > 0)
            {
                newPikmin.GetComponent<Pikmin>().isFlowered = true;
                newPikmin.GetComponent<Pikmin>().ChangeLeafState();
                pikminFlowerYellow--;
            }
        }

        for (int i = 0; i > pikminInOutYellow; i--)
        {

            foreach (Pikmin pikmin in allPikmin)
            {
                if (pikmin.state == Pikmin.State.Follow && pikmin.pikminColor == "Yellow")
                {
                    allPikmin.Remove(pikmin);
                    pikmin.EnterOnion();
                    if (pikmin.isFlowered == true) pikminFlowerYellow++;
                    break;
                }
            }
        }
        pikminInOutYellow = 0;
    }

    public void BluePikminEnterExit(Pikmin bluePikmin, ref List<Pikmin> allPikmin)
    {
        for (int i = 0; i < pikminInOutBlue; i++)
        {
            Pikmin newPikmin = Instantiate(bluePikmin);
            newPikmin.transform.position = transform.position + (Random.insideUnitSphere * spawnRadius);
            PikminManager.allPikmin.Add(newPikmin);
            if (pikminFlowerBlue > 0)
            {
                newPikmin.GetComponent<Pikmin>().isFlowered = true;
                newPikmin.GetComponent<Pikmin>().ChangeLeafState();
                pikminFlowerBlue--;
            }
        }

        for (int i = 0; i > pikminInOutBlue; i--)
        {

            foreach (Pikmin pikmin in allPikmin)
            {
                if (pikmin.state == Pikmin.State.Follow && pikmin.pikminColor == "Blue")
                {
                    allPikmin.Remove(pikmin);
                    pikmin.EnterOnion();
                    if (pikmin.isFlowered == true) pikminFlowerBlue++;
                    break;
                }
            }
        }
        pikminInOutBlue = 0;
    }

    public void CyanPikminEnterExit(Pikmin cyanPikmin, ref List<Pikmin> allPikmin)
    {
        for (int i = 0; i < pikminInOutCyan; i++)
        {
            Pikmin newPikmin = Instantiate(cyanPikmin);
            newPikmin.transform.position = transform.position + (Random.insideUnitSphere * spawnRadius);
            PikminManager.allPikmin.Add(newPikmin);
            if (pikminFlowerCyan > 0)
            {
                newPikmin.GetComponent<Pikmin>().isFlowered = true;
                newPikmin.GetComponent<Pikmin>().ChangeLeafState();
                pikminFlowerCyan--;
            }
        }

        for (int i = 0; i > pikminInOutCyan; i--)
        {

            foreach (Pikmin pikmin in allPikmin)
            {
                if (pikmin.state == Pikmin.State.Follow && pikmin.pikminColor == "Cyan")
                {
                    allPikmin.Remove(pikmin);
                    pikmin.EnterOnion();
                    if (pikmin.isFlowered == true) pikminFlowerCyan++;
                    break;
                }
            }
        }
        pikminInOutCyan = 0;
    }

    public void RedButtonUp()
    {
        if (pikminFollowingRed == 0)
        {
            audioSource.PlayOneShot(buttonDeny);
            return;
        }
        pikminFollowingRed--;
        pikminRed++;
        totalPikmin++;
        pikminOutside--;
        pikminInOutRed--;
        audioSource.PlayOneShot(buttonPress, 0.5f);
        interfaceManager.UpdateTotalOnion(totalPikmin);
        interfaceManager.UpdatePikminOutside(pikminOutside);
        interfaceManager.UpdateRedInside(pikminRed);
        interfaceManager.UpdateRedOutside(pikminFollowingRed);
    }
    public void RedButtonDown()
    {
        if (pikminRed == 0)
        {
            audioSource.PlayOneShot(buttonDeny);
            return;
        } 
        pikminFollowingRed++;
        pikminRed--;
        totalPikmin--;
        pikminOutside++;
        pikminInOutRed++;
        audioSource.PlayOneShot(buttonPress, 0.5f);
        interfaceManager.UpdateTotalOnion(totalPikmin);
        interfaceManager.UpdatePikminOutside(pikminOutside);
        interfaceManager.UpdateRedInside(pikminRed);
        interfaceManager.UpdateRedOutside(pikminFollowingRed);
    }

    public void YellowButtonUp()
    {
        if (pikminFollowingYellow == 0)
        {
            audioSource.PlayOneShot(buttonDeny);
            return;
        }
        pikminFollowingYellow--;
        pikminYellow++;
        totalPikmin++;
        pikminOutside--;
        pikminInOutYellow--;
        audioSource.PlayOneShot(buttonPress, 0.5f);
        interfaceManager.UpdateTotalOnion(totalPikmin);
        interfaceManager.UpdatePikminOutside(pikminOutside);
        interfaceManager.UpdateYellowInside(pikminYellow);
        interfaceManager.UpdateYellowOutside(pikminFollowingYellow);
    }
    public void YellowButtonDown()
    {
        if (pikminYellow == 0)
        {
            audioSource.PlayOneShot(buttonDeny);
            return;
        }
        pikminFollowingYellow++;
        pikminYellow--;
        totalPikmin--;
        pikminOutside++;
        pikminInOutYellow++;
        audioSource.PlayOneShot(buttonPress, 0.5f);
        interfaceManager.UpdateTotalOnion(totalPikmin);
        interfaceManager.UpdatePikminOutside(pikminOutside);
        interfaceManager.UpdateYellowInside(pikminYellow);
        interfaceManager.UpdateYellowOutside(pikminFollowingYellow);
    }

    public void BlueButtonUp()
    {
        if (pikminFollowingBlue == 0)
        {
            audioSource.PlayOneShot(buttonDeny);
            return;
        }
        pikminFollowingBlue--;
        pikminBlue++;
        totalPikmin++;
        pikminOutside--;
        pikminInOutBlue--;
        audioSource.PlayOneShot(buttonPress, 0.5f);
        interfaceManager.UpdateTotalOnion(totalPikmin);
        interfaceManager.UpdatePikminOutside(pikminOutside);
        interfaceManager.UpdateBlueInside(pikminBlue);
        interfaceManager.UpdateBlueOutside(pikminFollowingBlue);
    }
    public void BlueButtonDown()
    {
        if (pikminBlue == 0)
        {
            audioSource.PlayOneShot(buttonDeny);
            return;
        }
        pikminFollowingBlue++;
        pikminBlue--;
        totalPikmin--;
        pikminOutside++;
        pikminInOutBlue++;
        audioSource.PlayOneShot(buttonPress, 0.5f);
        interfaceManager.UpdateTotalOnion(totalPikmin);
        interfaceManager.UpdatePikminOutside(pikminOutside);
        interfaceManager.UpdateBlueInside(pikminBlue);
        interfaceManager.UpdateBlueOutside(pikminFollowingBlue);
    }

    public void CyanButtonUp()
    {
        if (pikminFollowingCyan == 0)
        {
            audioSource.PlayOneShot(buttonDeny);
            return;
        }
        pikminFollowingCyan--;
        pikminCyan++;
        totalPikmin++;
        pikminOutside--;
        pikminInOutCyan--;
        audioSource.PlayOneShot(buttonPress, 0.5f);
        interfaceManager.UpdateTotalOnion(totalPikmin);
        interfaceManager.UpdatePikminOutside(pikminOutside);
        interfaceManager.UpdateCyanInside(pikminCyan);
        interfaceManager.UpdateCyanOutside(pikminFollowingCyan);
    }
    public void CyanButtonDown()
    {
        if (pikminCyan == 0)
        {
            audioSource.PlayOneShot(buttonDeny);
            return;
        }
        pikminFollowingCyan++;
        pikminCyan--;
        totalPikmin--;
        pikminOutside++;
        pikminInOutCyan++;
        audioSource.PlayOneShot(buttonPress, 0.5f);
        interfaceManager.UpdateTotalOnion(totalPikmin);
        interfaceManager.UpdatePikminOutside(pikminOutside);
        interfaceManager.UpdateCyanInside(pikminCyan);
        interfaceManager.UpdateCyanOutside(pikminFollowingCyan);
    }
}
