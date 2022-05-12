using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarBeetleAudio : MonoBehaviour
{
    public AudioSource source;
    public AudioClip emerge;
    public AudioClip death;
    public AudioClip shoot;
    public AudioClip burrow;
    public float volume;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

  public void Emerge()
  {
        source.PlayOneShot(emerge, volume);
    }

  public void Dying()
  {
        source.PlayOneShot(death, volume);
    }

  public void Attack()
  {
        source.PlayOneShot(shoot, volume);
    }

  public void Burrow() 
  {
        source.PlayOneShot(burrow, volume);
    }

}
