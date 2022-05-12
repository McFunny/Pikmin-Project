using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHazard : MonoBehaviour
{
    public bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11 && isActive == true)
        {
            other.gameObject.GetComponent<Pikmin>().StartCoroutine("PanicFire");
        }
    }
}
