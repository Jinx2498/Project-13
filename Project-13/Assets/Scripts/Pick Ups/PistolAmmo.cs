using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolAmmo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col) {
        Destroy(gameObject);
        PistolWeapon pistol = col.transform.GetComponent<PistolWeapon>();
        pistol.AddPistolAmmo();
    }
}
