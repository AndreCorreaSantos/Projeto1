using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class Enemy : MonoBehaviour, IDamageable
{
    // Start is called before the first frame update
    public AudioSource source;
    public AudioClip hitsound;
    private NavMeshAgent agent;

    public float health = 50f;

    void Start(){
        agent = GetComponent<NavMeshAgent>();
    }
    public void TakeDamage(float amount)
    {
        source.PlayOneShot(hitsound);
        float delay = hitsound.length;

        health -= amount;
        if (health <= 0f)
        {
            Destroy(gameObject, delay);
        }
    }

    void Update(){
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        agent.SetDestination(player.transform.position);
    }


}
