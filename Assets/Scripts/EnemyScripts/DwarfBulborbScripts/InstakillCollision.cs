using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstakillCollision : MonoBehaviour
{
    GameObject Enemy;
    // Start is called before the first frame update
    void Start()
    {
        Enemy = gameObject.transform.parent.gameObject;
    }

    
    private void OnTriggerEnter(Collider other)
    {
        //if (collision.gameObject.layer == 11)
        // {
        Enemy.GetComponent<EnemyHealth>().health = 0;
        
        //Destroy(Enemy);
       // }
    }
}
