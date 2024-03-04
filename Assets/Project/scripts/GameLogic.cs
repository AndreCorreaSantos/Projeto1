using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private int puzzlesSolved = 0;

    private int killedEnemies = 0;

    public GameObject gun;
    public void onPuzzleSolved()
    {
        puzzlesSolved++;
        if (puzzlesSolved >= 2)
        {
            spawnGun();
        }
    }

    public void spawnGun()
    {
        Instantiate(gun, transform.position, Quaternion.identity);
    }

    public void OnEnemyDeath()
    {
        Debug.Log("Enemy died");    
        killedEnemies++;
        if (killedEnemies >= 5)
        {
            Debug.Log("You win!");
        }
    }
    
}
