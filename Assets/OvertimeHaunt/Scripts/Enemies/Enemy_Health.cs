using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public bool isBoss = false; // 👑 assign true for boss enemies

    private GameController _gameController;

    [System.Obsolete]
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
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        if (_gameController == null)
        {
            Destroy(gameObject);
            return;
        }

        // 🧩 Skip breakables entirely
        if (CompareTag("Breakable"))
        {
            Destroy(gameObject);
            Debug.Log($"{name} broken!");
            return;
        }

        // 👑 If this is a boss, trigger the win menu
        if (isBoss || CompareTag("Boss"))
        {
            _gameController.DisplayWinMenu();
            Debug.Log("Boss defeated! Showing Win Menu.");
        }
        else
        {
            // 🧍 Regular enemy defeat behavior
            _gameController.EnemyDefeated();
        }

        Destroy(gameObject);
        Debug.Log($"{name} defeated!");
    }
}
