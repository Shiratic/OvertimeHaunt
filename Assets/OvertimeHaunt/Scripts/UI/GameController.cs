using UnityEngine;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // Press 'R' to restart
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Debug.Log("Restart Scene!");
        }

        if (Input.GetKeyDown(KeyCode.Escape)) // Press 'esc' to quit game
        {
            Application.Quit();
            Debug.Log("Quit Game!");
        }
    }
}
