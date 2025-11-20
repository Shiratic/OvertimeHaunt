using UnityEngine;
using System.Collections;

public class Enemy_Health : MonoBehaviour
{
    [Header("Health Settings")]
    public int currentHealth;
    public int maxHealth;
    public bool isBoss = false; // 👑 assign true for boss enemies

    [Header("Damage Flash Settings")]
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.15f;
    [SerializeField] private ParticleSystem _enemyParticle;
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;
    private GameController _gameController;
    

    [System.Obsolete]
    void Start()
    {
        currentHealth = maxHealth;
        _gameController = FindObjectOfType<GameController>();

        // Cache SpriteRenderer for flashing
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer != null)
            _originalColor = _spriteRenderer.color;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;

        // 🔥 Flash whenever damaged
        if (amount < 0)
            if (CompareTag("Enemy"))
            {
                StartCoroutine(FlashDamage());
                Instantiate(_enemyParticle, transform.position, Quaternion.identity);
            }


        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0)
        {
            HandleDeath();
        }
    }

    private IEnumerator FlashDamage()
    {
        if (_spriteRenderer == null)
            yield break;

        _spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(flashDuration);
        _spriteRenderer.color = _originalColor;
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
