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
        }

        else if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
