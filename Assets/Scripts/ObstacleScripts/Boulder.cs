using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Boulder : MonoBehaviour
{

    public AudioClip crash;
    public AudioSource source;
    
   // void Awake()
   // {
      
   // }

    
    void Update()
    {
        transform.Rotate(0, 25, 0 * Time.deltaTime);
    }

    public void Throw(Vector3 target, float time, float delay)
    {
        transform.DOJump(target, 4, 1, time).SetDelay(delay).SetEase(Ease.Linear);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            other.gameObject.GetComponent<Pikmin>().Death();
        }
        if (other.gameObject.layer == 0)
        {
            Destroy(gameObject);
            source.PlayOneShot(crash);
        }
        if (other.gameObject.layer == 10)
        {
            Destroy(gameObject);
            source.PlayOneShot(crash);
        }
    }
}
