using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private GameController _gameController;

    public int currentHealth;
    public int maxHealth;

    public SpriteRenderer playerSr;
    public PlayerMovement playerMovement;

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;

        if(currentHealth <= 0)
        {
            playerSr.enabled = false;
            playerMovement.enabled = false;
            _gameController.DisplayLoseMenu();
            Debug.Log("You Died.");
        }
    }
}
