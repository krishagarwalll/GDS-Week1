using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Helicopter")]
    public GameObject helicopter;
    public int maxCapacity = 3;

    [Header("UI")]
    public TMP_Text soldiersInHelicopterText;
    public TMP_Text soldiersRescuedText;
    public TMP_Text messageText;

    private int soldiersInHelicopter = 0;
    private int soldiersRescued = 0;

    private bool gameEnded = false;

    void Start()
    {
        UpdateUI();
        messageText.text = "";
    }

    void Update()
    {
        // Reset game at any time
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // Called by HelicopterCollisionForwarder
    public void HandleCollision(GameObject other)
    {
        if (gameEnded) return; // stop processing collisions after game ends

        if (other.CompareTag("Soldier"))
        {
            if (soldiersInHelicopter < maxCapacity)
            {
                soldiersInHelicopter++;
                UpdateUI();
                Destroy(other); // remove soldier
                // Play pickup sound here if needed
            }
        }
        else if (other.CompareTag("Hospital"))
        {
            if (soldiersInHelicopter > 0)
            {
                soldiersRescued += soldiersInHelicopter;
                soldiersInHelicopter = 0;
                UpdateUI();

                // Check win condition
                if (GameObject.FindGameObjectsWithTag("Soldier").Length == 0)
                {
                    messageText.text = "YOU WIN!";
                    EndGame();
                }
            }
        }
        else if (other.CompareTag("Tree"))
        {
            messageText.text = "GAME OVER";
            EndGame();
        }
    }

    void UpdateUI()
    {
        soldiersInHelicopterText.text = "In Helicopter: " + soldiersInHelicopter;
        soldiersRescuedText.text = "Rescued: " + soldiersRescued;
    }

    void EndGame()
    {
        gameEnded = true;
        // Stop helicopter movement
        if (helicopter != null)
        {
            var movement = helicopter.GetComponent<HelicopterMovement>();
            if (movement != null)
                movement.enabled = false;

            // Stop physics motion
            var rb = helicopter.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = Vector2.zero;
        }
    }
    
    public bool GameEnded()
    {
        return gameEnded;
    }
}