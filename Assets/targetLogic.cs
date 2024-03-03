using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetLogic : MonoBehaviour
{

    public Transform spawnTransform;
    public GameObject prefab;
 private void OnTriggerEnter(Collider other)
    {

           // spawn prefab at spawn transform
            Instantiate(prefab, spawnTransform.position, spawnTransform.rotation);
    }

}
