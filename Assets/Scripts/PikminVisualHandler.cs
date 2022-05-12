using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PikminVisualHandler : MonoBehaviour
{
    private Pikmin pikmin;

    public TrailRenderer trail;
    public ParticleSystem particleTrail;
    public ParticleSystem leafParticle;
    public ParticleSystem activationParticle;
    public ParticleSystem attackParticle;
    public ParticleSystem deathParticle;
    public ParticleSystem fireParticle;
    public Transform model;

    private void Awake()
    {
        pikmin = GetComponent<Pikmin>();
        pikmin.OnStartFollow.AddListener((x) => OnStartFollow(x));
        pikmin.OnStartThrow.AddListener((x) => OnStartThrow(x));
        pikmin.OnEndThrow.AddListener((x) => OnEndThrow(x));
        pikmin.OnHit.AddListener((x) => OnHit(x));
        pikmin.OnDeath.AddListener((x) => OnDeath(x));
        pikmin.OnFire.AddListener((x) => OnFire(x));
    }
    void Start()
    {
        trail = GetComponentInChildren<TrailRenderer>();
        trail.emitting = false;
    }

    public void OnStartFollow(int num)
    {
        transform.DOJump(transform.position, .4f, 1, .3f);
        transform.DOPunchScale(-Vector3.up / 2, .3f, 10, 1).SetDelay(Random.Range(0, .1f));

        activationParticle.Play();
        leafParticle.Clear();
        leafParticle.Stop();
        fireParticle.Clear();
        fireParticle.Stop();
    }

    public void OnStartThrow(int num)
    {
        trail.Clear();
        trail.emitting = true;
        particleTrail.Play();
    }

    public void OnEndThrow(int num)
    {
        particleTrail.Stop();
        trail.emitting = false;

        if(pikmin.objective == null && pikmin.objectiveEnemy == null)
            leafParticle.Play();
    }
    public void OnHit(int num)
    {
        attackParticle.Clear();
        
        attackParticle.enableEmission = true;
        attackParticle.Play();
        StartCoroutine("OnHitDone");
    }
    
    IEnumerator OnHitDone()
    {
        yield return new WaitForSeconds(0.5f);
        attackParticle.enableEmission = false;
        attackParticle.Stop();
        
    } 

    public void OnDeath(int num)
    {
        //GetComponent<MeshRenderer>().enabled = true;
        deathParticle.Play();
        leafParticle.Stop();
        leafParticle.Clear();
        fireParticle.Clear();
        fireParticle.Stop();
    }

    public void OnFire(int num)
    {
        fireParticle.enableEmission = true;
        fireParticle.Play();

    }
}
