using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IDamageable
{
    public AudioSource source;
    public AudioClip hitsound;
    private NavMeshAgent agent;

    public float health = 50f;

    public UnityEvent onDeath;

    // Reference to the GameLogic script
    private GameLogic gameLogicScript;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Find the GameLogic object by name and get the GameLogic component
        GameObject gameLogicObject = GameObject.Find("GameLogic");
        if (gameLogicObject != null)
        {
            gameLogicScript = gameLogicObject.GetComponent<GameLogic>();
            // Subscribe the GameLogic method to the onDeath event
            if (gameLogicScript != null)
            {
                onDeath.AddListener(gameLogicScript.OnEnemyDeath);
            }
            else
            {
                Debug.LogError("GameLogic script not found on 'gameLogic' object.");
            }
        }
        else
        {
            Debug.LogError("'gameLogic' object not found in the scene.");
        }
    }

    public void TakeDamage(float amount)
    {
        source.PlayOneShot(hitsound);
        float delay = hitsound.length;

        health -= amount;
        if (health <= 0f)
        {
            onDeath.Invoke();
            Destroy(gameObject, delay);
        }
    }

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        agent.SetDestination(player.transform.position);
    }
}
