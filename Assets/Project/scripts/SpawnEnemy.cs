using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;

    public float spawnRadius;

    private bool spawning = false;

    // audio 
    public AudioSource source;
    public AudioClip hitsound;


    void Start(){
        
    }

    // on collision
    void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.name == "bullet_head(Clone)"){
                source.PlayOneShot(hitsound);
                spawning = true; // spawn enemies
                StartCoroutine(SpawnEnemies());
            }
        }
    IEnumerator SpawnEnemies()
    {
        while (spawning){
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            int numEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
            float minDistance = 5f;
            Debug.Log(1);


            // Check if the player is within the spawn radius
            if (numEnemies < 10)
            {
                // Generate a random position within the spawn radius
                
                Vector3 randomPosition = player.transform.position+ (Random.insideUnitSphere * spawnRadius)+(Random.insideUnitSphere * minDistance);

                // Set the y-coordinate of the random position to 0 to ensure the enemy is spawned on the ground
                randomPosition.y = 0;

                // Spawn a new enemy at the random position
                Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
            }
            yield return new WaitForSeconds(3f);
        }
    }

}
