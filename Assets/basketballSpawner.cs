using UnityEngine;

public class basketballSpawner : MonoBehaviour
{
    public GameObject basketball;

    public void SpawnPrefab()
    {
        if (basketball != null)
        {
            Instantiate(basketball, transform.position, Quaternion.identity);
        }
    }
}