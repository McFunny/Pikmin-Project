using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwarfBulborbAudio : MonoBehaviour
{
    private AudioSource source;
    public AudioClip attack;
    public AudioClip chew;
    public AudioClip death1;
    public AudioClip death2;
    public float volume = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Notice()
    {
        source.Play();
    }

    private void Attack()
    {
        source.PlayOneShot(attack, volume);
    }

    private void AttackSuccessful()
    {
        source.PlayOneShot(chew, volume);
    }
    private void Death1()
    {
        source.PlayOneShot(death1, volume);
    }
    private void Death2()
    {
        source.PlayOneShot(death2, volume);
    }
}
