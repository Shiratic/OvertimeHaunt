using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 1; // How much health this pickup restores
    [SerializeField] private ParticleSystem _healthParticle;
    [SerializeField] AudioClip _healthPickUp = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Instantiate(_healthParticle, transform.position, Quaternion.identity);
            AudioHelper.PlayClip2D(_healthPickUp, 0.1f);
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Heal player
                
                playerHealth.ChangeHealth(healAmount);

                // Update max health to match current health
                if (playerHealth.currentHealth > playerHealth.maxHealth)
                {
                    playerHealth.maxHealth = playerHealth.currentHealth;
                }
            }

            
            Destroy(gameObject); // Remove the pickup
        }
    }
}
