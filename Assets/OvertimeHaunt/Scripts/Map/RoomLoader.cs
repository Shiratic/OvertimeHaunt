using UnityEngine;

public class RoomLoader : MonoBehaviour
{
    [Header("Enemies to Activate")]
    [SerializeField] private GameObject[] _enemiesToActivate;

    [Header("Doors to Activate")]
    [SerializeField] private GameObject[] _doorsToActivate;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Activate all doors
        foreach (GameObject door in _doorsToActivate)
        {
            if (door != null)
                door.SetActive(true);
        }

        // Activate all enemies
        foreach (GameObject enemy in _enemiesToActivate)
        {
            if (enemy != null)
                enemy.SetActive(true);
        }

        // Update the UI
        GameController gameController = FindObjectOfType<GameController>();
        if (gameController != null)
        {
            gameController.RecountEnemies();
        }

        // Destroy trigger so it only happens once
        Destroy(gameObject);
    }
}
