using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public float maxHealth = 100f;
    public float health = 0f;
    public Text healthDispley;

    public void Update()
    {
        healthDispley.text = health.ToString();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if(health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.tag == "HealthPickup")
        {
            health = maxHealth;
        }
    }

}
