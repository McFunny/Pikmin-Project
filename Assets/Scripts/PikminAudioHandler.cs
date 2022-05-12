using UnityEngine;

public class PikminAudioHandler : MonoBehaviour
{
    private Pikmin pikmin;

    public AudioSource generalSource;
    public AudioSource carrySource;
    public AudioSource throwSource;

    [Header("Sounds")]
    public AudioClip throwSound;
    public AudioClip noticeSound;
    public AudioClip grabSound;
    public AudioClip attackSound;
    public AudioClip deathSound;
    public AudioClip nectarSound;
    public AudioClip wahSound;
    public AudioClip[] panicSounds;

    //public AudioClip panicSound1;
    //public AudioClip panicSound2;
    //public AudioClip panicSound3;

    private void Awake()
    {
        pikmin = GetComponent<Pikmin>();
        pikmin.OnStartFollow.AddListener((x) => OnStartFollow(x));
        pikmin.OnStartThrow.AddListener((x) => OnStartThrow(x));
        pikmin.OnEndThrow.AddListener((x) => OnEndThrow(x));

        pikmin.OnStartCarry.AddListener((x) => OnStartCarry(x));
        pikmin.OnEndCarry.AddListener((x) => OnEndCarry(x));
        pikmin.OnHit.AddListener((x) => OnHit(x));
        pikmin.OnDeath.AddListener((x) => OnDeath(x));
        pikmin.OnNectar.AddListener((x) => OnNectar(x));
        pikmin.OnWah.AddListener((x) => OnWah(x));
        pikmin.OnFire.AddListener((x) => OnFire(x));
    }

    public void OnStartFollow(int num)
    {
        carrySource.Stop();
        generalSource.PlayOneShot(noticeSound);
    }

    public void OnStartThrow(int num)
    {
        generalSource.PlayOneShot(throwSound);
        throwSource.Play();
    }

    public void OnEndThrow(int num)
    {
        if(pikmin.objective != null)
            generalSource.PlayOneShot(grabSound);
    }

    public void OnStartCarry(int num)
    {
        carrySource.Play();
    }

    public void OnEndCarry(int num)
    {
        carrySource.Stop();
    }

    public void OnHit(int num)
    {
        generalSource.PlayOneShot(attackSound);
    }

    public void OnDeath(int num)
    {
        AudioSource.PlayClipAtPoint(deathSound, transform.position); 
    }

    public void OnNectar(int num)
    {
        generalSource.PlayOneShot(nectarSound);
    }

    public void OnWah(int num)
    {
        generalSource.PlayOneShot(wahSound);
    }

    public void OnFire(int num)
    {
        generalSource.clip = panicSounds[Random.Range(0, panicSounds.Length)];
        generalSource.Play();
    }
}
