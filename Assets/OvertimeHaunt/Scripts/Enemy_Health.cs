using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    public void Start()
    {
        currentHealth = maxHealth;
    }

    public void ChangeHealth( int amount)
    {
        currentHealth += amount;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
            Debug.Log("Enemy HP: " + currentHealth);
        }

        else if (currentHealth <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Enemy Dead");
        }
    }
}
