using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with an object that implements the IDamageable interface
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        Debug.Log(1);
        Debug.Log(collision.gameObject.name);

        // Apply damage to the object if it implements the IDamageable interface
        if (damageable != null)
        {   
            Debug.Log("Hit " + collision.gameObject.name);
            damageable.TakeDamage(damage);
            Destroy(gameObject);
        }

       
    }
}
