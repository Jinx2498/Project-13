using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    public WeaponSwitching weaponSwitchScript;
    public GameObject grenadePref;      //Grenade Prefab
    public float throwForce = 40f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && weaponSwitchScript.numGrenades > 0 && weaponSwitchScript.selectedWeapon == 4)        //0 is the left mouse click
        {
            weaponSwitchScript.numGrenades--;
            ThrowGrenade();
        }
    }

    public void ThrowGrenade()
    {
        GameObject grenade = Instantiate(grenadePref, transform.position, transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
    }
}
