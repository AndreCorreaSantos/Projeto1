using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;

    public float spawnRadius;

    void Start(){
        SpawnEnemies();
    }
    IEnumerator SpawnEnemies()
    {
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        int numEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;



        // Check if the player is within the spawn radius
        if (Vector3.Distance(transform.position, player.transform.position) <= spawnRadius && numEnemies < 10)
        {
            // Generate a random position within the spawn radius
            Vector3 randomPosition = Random.insideUnitSphere * spawnRadius;

            // Set the y-coordinate of the random position to 0 to ensure the enemy is spawned on the ground
            randomPosition.y = 0;

            // Spawn a new enemy at the random position
            Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        }
        yield return new WaitForSeconds(3f);
    }

}
