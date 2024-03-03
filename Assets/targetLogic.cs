using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetLogic : MonoBehaviour
{
 private void OnTriggerEnter(Collider other)
    {

            Debug.Log("The target collider has been hit by another object.");
    }

}
