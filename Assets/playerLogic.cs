using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class playerLogic : MonoBehaviour
{
    public int health = 100;
    private bool canTakeDamage = true;

    void Start()
    {

    }

    void Update()
    {

    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        Debug.Log("Player took " + damageAmount + " damage. Current health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player has died.");
        RestartGame();
    }

    void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Enemy") && canTakeDamage)
        {
            int damageAmount = 10;
            TakeDamage(damageAmount);
            StartCoroutine(TakeDamageCooldown());
        }
    }

    private IEnumerator TakeDamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(2f);
        canTakeDamage = true;
    }
}
