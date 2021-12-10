using UnityEngine;

public class GunBehavior : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    public float fireRate = 15f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f;      //Fire rate cap

    void Update()
    {
        //Shoot when fired if allowed
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;     //Fire rate
            Shoot();
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();     //Show flash when fired

        RaycastHit hit;

        //If aimed at an object when fired, hit is registered
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();       //Find what was hit
            if (target != null)
            {
                target.TakeDamage(damage);                              //Damage object if object takes damage
            }

            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);      //Apply force of bullet to objects with rigidbodies
            }

            //Create visual impact as an object at the point of contact on the object being hit
            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);                                      //Destroy visual impact after 2 seconds
        }
    }
}
