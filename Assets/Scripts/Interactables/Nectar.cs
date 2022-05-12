using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Nectar : MonoBehaviour
{
    public bool beingEaten = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Spawn");
        GetComponent<Collider>().enabled = false;
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(1.5f);
        GetComponent<Collider>().enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        print("I am touched");
        if(other.gameObject.layer == 11)
        {
            if (other.gameObject.GetComponent<Pikmin>().isFlowered == false) other.gameObject.GetComponent<Pikmin>().CheckInteraction();

            if (beingEaten == false && other.gameObject.GetComponent<Pikmin>().isFlowered == false)
            {
                
                StartCoroutine("BeingEaten");
                beingEaten = true;
            }
        }
    }

    IEnumerator BeingEaten()
    {
        Sequence s = DOTween.Sequence();
        s.Join(transform.DOScale(0, 2.0f).SetEase(Ease.InQuint));
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
