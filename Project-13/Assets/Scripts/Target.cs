using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{

    public float health = 100f;
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

}
