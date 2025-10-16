using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    private GameController _gameController;

    void Start()
    {
        currentHealth = maxHealth;
        _gameController = FindObjectOfType<GameController>();
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0)
        {
            if (_gameController != null)
                _gameController.EnemyDefeated();

            Destroy(gameObject);
            Debug.Log("Enemy Dead");
        }
    }
}
