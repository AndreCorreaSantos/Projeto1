using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot")]
public class SimpleShoot : MonoBehaviour
{
    [Header("Prefab Refrences")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    public AudioSource source;

    public AudioClip shootSound;

    public VisualEffect hitEffect;


    [Header("Location References")]
    [SerializeField] private Animator gunAnimator;

    // [SerializeField] private Transform barrelLocation;
    public Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] [SerializeField] private float  shotPower = 1000f;
    [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 150f;


    void Start()
    {
        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();
    }

    void Update(){
        Debug.DrawRay(barrelLocation.position, barrelLocation.forward*100, Color.red,0.5f);
        /// stop ray
        /// 

    }

    public void PullTheTrigger()
    {
        gunAnimator.SetTrigger("Fire");
    }

    //This function creates the bullet behavior
    void Shoot()
    {  
        source.PlayOneShot(shootSound,0.5f);
        // shoot raycast
        Ray ray = new Ray(barrelLocation.position, barrelLocation.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
          IDamageable damageable = hit.collider.GetComponent<IDamageable>();
          // get if parent has IDamageable
            if (damageable == null)
            {
                damageable = hit.collider.GetComponentInParent<IDamageable>();
            }

            // Check if the component implements the IDamageable interface
            if (damageable != null)
            {
                // instantiate the hit effect
                VisualEffect tempHit;
                tempHit = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
                // Call the TakeDamage method on the component
                damageable.TakeDamage(10);
                // Destroy the hit effect after 2 seconds
                Destroy(tempHit, 1f);
            }
        // Debug.DrawLine(ray.origin, hit.point, Color.green); //debug collision
        }
        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }

        //cancels if there's no bullet prefeb
        if (!bulletPrefab)
        { return; }

        // Create a bullet and add force on it in direction of the barrel
        Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation).GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);

    }

    //This function creates a casing at the ejection slot
    void CasingRelease()
    {
        //Cancels function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
        { return; }

        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
    }

}
