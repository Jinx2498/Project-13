using UnityEngine;
using System.Collections;

public class HeavyWeapon : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    public float fireRate = 15f;

    public int maxAmmo = 10;
    public int heavyAmmo;
    public int heavyAmmoReserve = 50;

    // private int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;
    private bool isScoped = false;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    private float nextTimeToFire = 0f;      //Fire rate cap

    public Animator recoilAnim;             //Recoil animation
    public Animator reloadAnim;             //Reload animation
    public Animator scopeAnim;              //Scope animation

    public GameObject scopeOverlay;
    public GameObject weaponCamera;
    public Camera mainCamera;

    public float scopedFOV = 15f;           //Scoped/ADS field of view
    private float normalFOV;                //Regular field of view

    public Recoil recoilScript;             //Recoil script

    void Start()
    {
        heavyAmmo = maxAmmo;
        normalFOV = mainCamera.fieldOfView;               //Initialize the normal field of view
    }

    void OnEnable()     //Allows for swapping weapons mid reload
    {
        isReloading = false;
        reloadAnim.SetBool("Reloading", false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire2"))       //Aim down scope
        {
            isScoped = !isScoped;
            recoilScript.isADS = !recoilScript.isADS;   //Lowers recoil when ADS
            scopeAnim.SetBool("Scoped", isScoped);

            if (isScoped)
            {
                StartCoroutine(OnScoped());     //Scope in
            } else {
                OnUnscoped();                   //Scope out
            }
        }

        if(gameObject)

        if (isReloading)
        {
            return;                             //Ignores command to reload if already reloading
        }

        if((heavyAmmo == 0 || Input.GetKey("r")) && heavyAmmoReserve > 0)
        {
            StartCoroutine(ReloadHeavy());           //Begins reloading
            return;
        }

        //Shoot when fired if allowed
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && heavyAmmo > 0)
        {
            nextTimeToFire = Time.time + 1f / fireRate;     //Fire rate
            Shoot();
        }
    }

    IEnumerator OnScoped()
    {
        yield return new WaitForSeconds(.15f);      //Added lag when transitioning into scope
        weaponCamera.SetActive(false);
        scopeOverlay.SetActive(true);

        mainCamera.fieldOfView = scopedFOV;               //Zoom in effect when scoped
    }

    void OnUnscoped()
    {
        scopeOverlay.SetActive(false);
        weaponCamera.SetActive(true);
        mainCamera.fieldOfView = normalFOV;
    }

    IEnumerator ReloadHeavy()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        if(heavyAmmo == 0)
        {
            yield return new WaitForSeconds(.25f);              //Added lag before reload time
        }
        
        scopeAnim.SetBool("Scoped", false);                 //Back out of scope
        OnUnscoped();                                       //Back out of scope overlay
        reloadAnim.SetBool("Reloading", true);              //Begin reloading

        yield return new WaitForSeconds(reloadTime - .25f); //Reduced lag during reload time
        
        reloadAnim.SetBool("Reloading", false);             //Finish reloading
        scopeAnim.SetBool("Scoped", isScoped);              //Scopes in if player was scoped in before reloading
        yield return new WaitForSeconds(.25f);              //Added lag post reload time

        if (isScoped)
        {
            StartCoroutine(OnScoped());                     //Scope overlay applied if player was scoped in before reloading
        }

        if(heavyAmmoReserve > 10)
        {
            heavyAmmo = 10;
            heavyAmmoReserve -= 10;
        } else
        {
            heavyAmmo = heavyAmmoReserve;
            heavyAmmoReserve = 0;
        }

        isReloading = false;
    }

    void Shoot()
    {
        muzzleFlash.Play();                 //Show flash when fired
        recoilAnim.ResetTrigger("Fired");   //Recoil gun animation
        recoilAnim.SetTrigger("Fired");     //Reset recoil gun animation
        recoilScript.RecoilFire();          //Recoil mouse animation

        heavyAmmo--;


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

    void OnCollisionEnter(Collision other) {

        if(other.gameObject.tag == "HeavyAmmo") {
            heavyAmmoReserve += 10;
        }
    }
}
