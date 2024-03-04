using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunSpawnLogic : MonoBehaviour
{
    private int puzzlesSolved = 0;

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
    
}
