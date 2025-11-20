using UnityEngine;
using System.Collections;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private GameController _gameController;

    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.15f;

    public int currentHealth;
    public int maxHealth;

    public SpriteRenderer playerSr;
    private Color _originalColor;
    public PlayerMovement playerMovement;


    private void Start()
    {
        playerSr = GetComponent<SpriteRenderer>();
        if (playerSr != null)
            _originalColor = playerSr.color;
    }


    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        StartCoroutine(FlashDamage());

        if (currentHealth <= 0)
        {
            playerSr.enabled = false;
            playerMovement.enabled = false;
            _gameController.DisplayLoseMenu();
            Debug.Log("You Died.");
        }
    }

    private IEnumerator FlashDamage()
    {
        if (playerSr == null)
            yield break;

        playerSr.color = damageColor;
        yield return new WaitForSeconds(flashDuration);
        playerSr.color = _originalColor;
    }
}
