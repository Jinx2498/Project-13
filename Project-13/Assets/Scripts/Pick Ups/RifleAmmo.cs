using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleAmmo : MonoBehaviour
{
    public RifleWeapon rifleScript;

    void OnTriggerEnter()
    {
        //PistolWeapon pistol = transform.GetComponent<PistolWeapon>();
        rifleScript.AddRifleAmmo();
        Destroy(gameObject);
    }
}
