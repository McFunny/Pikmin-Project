using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAnimator : MonoBehaviour
{
    private EnemyHealth enemyHealth;
    private float oldHealth;
    private Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {

        if (oldHealth != enemyHealth.health)
        {
            anim.SetBool("Attacked", true);
            print("I am being hit");
            StartCoroutine("AttackCheck");
            oldHealth = enemyHealth.health;
        }


       

        if (enemyHealth.health <= 200)
        {
            anim.SetBool("WallDamage1", true);

        }

        if (enemyHealth.health <= 100)
        {
            anim.SetBool("WallDamage2", true);

        }

        if (enemyHealth.health <= 0)
        {
            anim.SetBool("WallDestroyed", true);
            Destroy(gameObject);
        }
    }

    IEnumerator AttackCheck()
    {
        yield return new WaitForSeconds(1);
        if (oldHealth == enemyHealth.health)
        {
            anim.SetBool("Attacked", false);
        }
    }
}
