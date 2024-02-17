using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    // Start is called before the first frame update

    private NavMeshAgent agent;
    public float health = 50f;

    void Start(){
        agent = GetComponent<NavMeshAgent>();
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    void Update(){
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        agent.SetDestination(player.transform.position);
    }

    void Die()
    {
        Destroy(gameObject);
    }

}
