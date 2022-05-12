using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveEntrance : MonoBehaviour
{
    public string caveName;
    public bool isOutside;
    private GameObject caveHudObject;
    private GameObject player;
    private GameObject pikminManager;

    public List<Pikmin> allPikmin = new List<Pikmin>();

    private InterfaceManager interfaceManager;
    private PikminManager pikminManagerScript;
    //private CaveInitializer caveInitializerScript;


    private AudioSource audioSource;
    public AudioClip menuOpen;
    public AudioClip menuClose;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        pikminManagerScript = GameObject.Find("Pikmin Manager").GetComponent<PikminManager>();
        interfaceManager = GameObject.Find("MainCanvas").GetComponent<InterfaceManager>();
        caveHudObject = GameObject.Find("CaveUI");
        caveHudObject.SetActive(false);
        pikminManager = GameObject.Find("Pikmin Manager");
        //caveInitializerScript = GameObject.Find("CaveInitializer").GetComponent<CaveInitializer>();
    }

    
    void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (pikminManagerScript.gameIsPaused == false)
            {
                pikminManagerScript.gameIsPaused = true;
                Time.timeScale = 0;
                interfaceManager.UpdateCaveName(caveName);
                caveHudObject.SetActive(true);
                audioSource.PlayOneShot(menuOpen);
            }
            
        }
    }

    public void EnterCave()
    {
        allPikmin = PikminManager.allPikmin;
        foreach (Pikmin pikmin in allPikmin)
        {
            if (pikmin.pikminColor == "Red" && pikmin.state == Pikmin.State.Follow) CaveInitializer.pikminRed++;
            if (pikmin.pikminColor == "Red" && pikmin.state == Pikmin.State.Follow && pikmin.isFlowered == true) CaveInitializer.pikminFloweredRed++;
            if (pikmin.pikminColor == "Yellow" && pikmin.state == Pikmin.State.Follow) CaveInitializer.pikminYellow++;
            if (pikmin.pikminColor == "Yellow" && pikmin.state == Pikmin.State.Follow && pikmin.isFlowered == true) CaveInitializer.pikminFloweredYellow++;
            if (pikmin.pikminColor == "Blue" && pikmin.state == Pikmin.State.Follow) CaveInitializer.pikminBlue++;
            if (pikmin.pikminColor == "Blue" && pikmin.state == Pikmin.State.Follow && pikmin.isFlowered == true) CaveInitializer.pikminFloweredBlue++;
            if (pikmin.pikminColor == "Cyan" && pikmin.state == Pikmin.State.Follow) CaveInitializer.pikminCyan++;
            if (pikmin.pikminColor == "Cyan" && pikmin.state == Pikmin.State.Follow && pikmin.isFlowered == true) CaveInitializer.pikminFloweredCyan++;

            //if (pikmin.state != Pikmin.State.Follow)

            


        }
        allPikmin.Clear();
        pikminManagerScript.gameIsPaused = true;
        Time.timeScale = 1;

        SceneManager.LoadScene(caveName, LoadSceneMode.Single);

    }

    public void No()
    {
        pikminManagerScript.gameIsPaused = false;
        Time.timeScale = 1;
        caveHudObject.SetActive(false);
        audioSource.PlayOneShot(menuClose);
    }
}
