using UnityEngine;
using System.Collections;

public class GunBehavior : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    public float fireRate = 15f;

    public int maxAmmo = 10;
    public int sniperAmmo = 0;
    public int rifleAmmo = 0;
    public int pistolAmmo = 0;
    public int heavyAmmo = 0;
    public int sniperAmmoReserve = 0;
    public int rifleAmmoReserve = 0;
    public int pistolAmmoReserve = 0;
    public int heavyAmmoReserve = 0;
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
        sniperAmmo = maxAmmo;
        rifleAmmo = maxAmmo;
        pistolAmmo = maxAmmo;
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

        if(sniperAmmo == 0 && sniperAmmoReserve >= 10)
        {
            StartCoroutine(ReloadSniper());           //Begins reloading
            return;
        }

        if(pistolAmmo == 0 && pistolAmmoReserve >= 10)
        {
            StartCoroutine(ReloadPistol());           //Begins reloading
            return;
        }

        if(heavyAmmo == 0 && heavyAmmoReserve >= 10)
        {
            StartCoroutine(ReloadHeavy());           //Begins reloading
            return;
        }

        if(rifleAmmo == 0 && rifleAmmoReserve >= 10)
        {
            StartCoroutine(ReloadRifle());           //Begins reloading
            return;
        }

        //Shoot when fired if allowed
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;     //Fire rate
            Shoot();
        }

        //Manual reload
        // if (Input.GetKey("r") && currentAmmo < maxAmmo)
        // {
        //     StartCoroutine(Reload());
        //     return;
        // }
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

    IEnumerator ReloadSniper()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        if(sniperAmmo == 0)
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

        sniperAmmo = maxAmmo;
        sniperAmmoReserve -= maxAmmo;
        isReloading = false;

    }

    IEnumerator ReloadPistol()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        if(pistolAmmo == 0)
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

        pistolAmmo = maxAmmo;
        pistolAmmoReserve -= maxAmmo;
        isReloading = false;

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

        heavyAmmo = maxAmmo;
        heavyAmmoReserve -= maxAmmo;
        isReloading = false;

    }

    IEnumerator ReloadRifle()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        if(rifleAmmo == 0)
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

        rifleAmmo = maxAmmo;
        rifleAmmoReserve -= maxAmmo;
        isReloading = false;

    }

    void Shoot()
    {
        muzzleFlash.Play();                 //Show flash when fired
        recoilAnim.ResetTrigger("Fired");   //Recoil gun animation
        recoilAnim.SetTrigger("Fired");     //Reset recoil gun animation
        recoilScript.RecoilFire();          //Recoil mouse animation
        
        if(gameObject.tag == "Sniper") {
            sniperAmmo--;
        }

        if(gameObject.tag == "Pistol") {
            pistolAmmo--;
        }

        if(gameObject.tag == "Heavy") {
            heavyAmmo--;
        }

        if(gameObject.tag == "Rifle") {
            rifleAmmo--;
        }

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

        if(other.gameObject.tag == "SniperAmmo") {
            sniperAmmoReserve += 10;
        }

        if(other.gameObject.tag == "HeavyAmmo") {
            heavyAmmoReserve += 10;
        }

        if(other.gameObject.tag == "PistolAmmo") {
            pistolAmmoReserve += 10;
        }

        if(other.gameObject.tag == "RifleAmmo") {
            rifleAmmoReserve += 10;
        }

    }
}
