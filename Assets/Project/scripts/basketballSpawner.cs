using UnityEngine;

public class basketballSpawner : MonoBehaviour
{
    public GameObject basketball;

    public void SpawnPrefab()
    {
        if (basketball != null)
        {
            // Instantiate the prefab at the adjusted position without any rotation
            Instantiate(basketball, transform.position, Quaternion.identity);
        }
    }
}
