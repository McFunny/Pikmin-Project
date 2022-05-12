using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveInitializer : MonoBehaviour
{
    public float spawnRadius = 2;
    public GameObject spawn;
    //public GameObject player;
    private PikminManager pikminManagerScript;
    public List<Pikmin> allPikmin = new List<Pikmin>();
    

    [SerializeField] private Pikmin redPikmin = default;
    [SerializeField] private Pikmin yellowPikmin = default;
    [SerializeField] private Pikmin bluePikmin = default;
    [SerializeField] private Pikmin cyanPikmin = default;

    [Header("Pikmin Following Into Caves")]
    //how many pikmin to spawn when entering a new sublevel
    static public int pikminRed;
    static public int pikminYellow;
    static public int pikminBlue;
    static public int pikminCyan;
    static public int pikminFloweredRed;
    static public int pikminFloweredYellow;
    static public int pikminFloweredBlue;
    static public int pikminFloweredCyan;

    // Start is called before the first frame update
    void Awake()
    {
        pikminManagerScript = GameObject.Find("Pikmin Manager").GetComponent<PikminManager>();
        spawn = GameObject.Find("PikminSpawn");
       // player = GameObject.Find("Jammo_Player");
        //allPikmin = PikminManager.allPikmin;
        //foreach (Pikmin pikmin in allPikmin)
        // {

        //pikmin.pikminManager = FindObjectOfType<PikminManager>();
        
        // }

        for (int i = 0; i < pikminRed; i++)
        {
            Pikmin newPikmin = Instantiate(redPikmin, spawn.transform.position, spawn.transform.rotation);
            //newPikmin.transform.position = spawn.transform.position + (Random.insideUnitSphere * spawnRadius);
            PikminManager.allPikmin.Add(newPikmin);
            if (pikminFloweredRed > 0)
            {
                newPikmin.GetComponent<Pikmin>().isFlowered = true;
                newPikmin.GetComponent<Pikmin>().ChangeLeafState();
                pikminFloweredRed--;
            }
        }

        for (int i = 0; i < pikminYellow; i++)
        {
            Pikmin newPikmin = Instantiate(yellowPikmin, spawn.transform.position, spawn.transform.rotation);
           // newPikmin.transform.position = spawn.transform.position;
            PikminManager.allPikmin.Add(newPikmin);
            if (pikminFloweredYellow > 0)
            {
                newPikmin.GetComponent<Pikmin>().isFlowered = true;
                newPikmin.GetComponent<Pikmin>().ChangeLeafState();
                pikminFloweredYellow--;
            }
        }

        for (int i = 0; i < pikminBlue; i++)
        {
            Pikmin newPikmin = Instantiate(bluePikmin, spawn.transform.position, spawn.transform.rotation);
            //newPikmin.transform.position = spawn.transform.position;
            PikminManager.allPikmin.Add(newPikmin);
            if (pikminFloweredBlue > 0)
            {
                newPikmin.GetComponent<Pikmin>().isFlowered = true;
                newPikmin.GetComponent<Pikmin>().ChangeLeafState();
                pikminFloweredBlue--;
            }
        }

        for (int i = 0; i < pikminCyan; i++)
        {
            Pikmin newPikmin = Instantiate(cyanPikmin, spawn.transform.position, spawn.transform.rotation);
            //newPikmin.transform.position = spawn.transform.position;
            PikminManager.allPikmin.Add(newPikmin);
            if (pikminFloweredCyan > 0)
            {
                newPikmin.GetComponent<Pikmin>().isFlowered = true;
                newPikmin.GetComponent<Pikmin>().ChangeLeafState();
                pikminFloweredCyan--;
            }
        }
    }

    
}
