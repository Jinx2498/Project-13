using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyAmmo : MonoBehaviour
{
    private Transform ammoPos;
    private Transform spawnPoint;
    private bool destroyed = false;
    // Start is called before the first frame update
    void Start()
    {
        ammoPos = GameObject.FindGameObjectWithTag("HeavyAmmo").transform;
        spawnPoint = GameObject.FindGameObjectWithTag("HeavyAmmo").transform;
        
    }

    // Update is called once per frame
    public void Update()
    {
        if(destroyed == true) {
            StartCoroutine("respawnHeavyAmmo");
        }
    }

    IEnumerator respawnHeavyAmmo() {
        yield return new WaitForSeconds(2f);
        ammoPos.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z);
        
    }

    void OnTriggerEnter(Collider col) {
    
        Destroy(gameObject);
        destroyed = true;
        
    }
}
