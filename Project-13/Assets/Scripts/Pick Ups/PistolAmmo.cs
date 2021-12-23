using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolAmmo : MonoBehaviour
{
    public PistolWeapon pistolScript;

    void OnTriggerEnter()
    {
        //PistolWeapon pistol = transform.GetComponent<PistolWeapon>();
        pistolScript.AddPistolAmmo();
        Destroy(gameObject);
    }
}
