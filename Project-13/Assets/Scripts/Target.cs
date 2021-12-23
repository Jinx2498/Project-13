using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public float maxHealth = 100f;
    public float health = 100f;
    public Text healthDispley;

    private Transform spawnPoint;
    private Transform playerPos;

    void start(){

        playerPos = GameObject.FindGameObjectWithTag("Respawn").transform;
        spawnPoint = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Update()
    {
        healthDispley.text = health.ToString();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if(health <= 0f)
        {
            health = maxHealth;
            playerPos.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z);
        }
    }

    // void Die()
    // {
    //     Destroy(gameObject);
    // }

    void OnTriggerEnter(Collider other) {

        if (other.gameObject.CompareTag("HealthPickup")) {

            health = maxHealth;  
        }

        if (other.gameObject.CompareTag("SniperAmmo")) {

            SniperWeapon sniper = other.transform.GetComponent<SniperWeapon>();
            sniper.AddSniperAmmo();

        }

        if (other.gameObject.CompareTag("HeavyAmmo")) {

            HeavyWeapon heavy = other.transform.GetComponent<HeavyWeapon>();
            heavy.AddHeavyAmmo();
        }

        if (other.gameObject.CompareTag("PistolAmmo")) {

            PistolWeapon pistol = other.transform.GetComponent<PistolWeapon>();
            pistol.AddPistolAmmo();
        }

        if (other.gameObject.CompareTag("RifleAmmo")) {

            RifleWeapon rifle = other.transform.GetComponent<RifleWeapon>();
            rifle.AddRifleAmmo(); 
        }
    }

}
