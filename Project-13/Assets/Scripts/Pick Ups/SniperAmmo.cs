using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperAmmo : MonoBehaviour
{
    public SniperWeapon sniperScript;

    void OnTriggerEnter()
    {
        //PistolWeapon pistol = transform.GetComponent<PistolWeapon>();
        sniperScript.AddSniperAmmo();
        Destroy(gameObject);
    }
}
