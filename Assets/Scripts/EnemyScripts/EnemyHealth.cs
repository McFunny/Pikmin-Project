using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float maxhealth;
    private Camera camera;
    

    public GameObject healthBarUI;
    public GameObject healthBarUIBackground;
    public Image healthPie;
    public Image healthPieBackground;
    public bool dead = false;
    public bool hideBar;
    public float lerpSpeed;

    
   
    void Start()
    {
        
        camera = Camera.main;
        health = maxhealth;
        if (hideBar == true) 
        {
            healthBarUI.SetActive(false); 
            healthBarUIBackground.SetActive(false);
        } 
        lerpSpeed = 3f * Time.deltaTime;

        

    }

    void Update()
    {
        

       // healthPie.transform.LookAt(transform.position + camera.transform.rotation * -Vector3.back,
       //  camera.transform.rotation * -Vector3.down);
       // healthPieBackground.transform.LookAt(transform.position + camera.transform.rotation * -Vector3.back,
       // camera.transform.rotation * -Vector3.down);
        healthPie.transform.LookAt(camera.transform.position);
        healthPieBackground.transform.LookAt(camera.transform.position);
        CalculateHealth();
        ColorChanger();
        if(health < maxhealth && health > 0)
        {
            if (hideBar == false)
            {
                healthBarUI.SetActive(true);
                healthBarUIBackground.SetActive(true);
            }
            
        }

        if(health <= 0)
        {
            //healthBarUI.SetActive(false);
            if(dead == false) StartCoroutine("BarOff");
            dead = true;
        }

        if(health > maxhealth)
        {
            health = maxhealth;
        }

        if(health == maxhealth)
        {
            healthBarUI.SetActive(false);
            healthBarUIBackground.SetActive(false);
        }
    }

    void CalculateHealth()
    {
        healthPie.fillAmount = Mathf.Lerp(healthPie.fillAmount, health / maxhealth, lerpSpeed);
    }
    
    IEnumerator BarOff()
    {
        if (hideBar == false)
        {
            healthBarUI.SetActive(true);
            healthBarUIBackground.SetActive(true);
        }
        yield return new WaitForSeconds(2);
        healthBarUI.SetActive(false);
        healthBarUIBackground.SetActive(false);

    }

    void ColorChanger()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, (health / maxhealth));
        healthPie.color = healthColor;
        healthColor.a = 1f;
        
    }
}
