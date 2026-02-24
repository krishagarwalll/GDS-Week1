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

    [Header("Sound")]
    public AudioClip pickupSound;
    private AudioSource audioSource;

    private int soldiersInHelicopter = 0;
    private int soldiersRescued = 0;

    private bool gameEnded = false;

    void Start()
    {
        UpdateUI();
        messageText.text = "";

        // Get AudioSource from helicopter
        if (helicopter != null)
            audioSource = helicopter.GetComponent<AudioSource>();
    }

    void Update()
    {
        // Reset game
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // Called from HelicopterCollisionForwarder
    public void HandleCollision(GameObject other)
    {
        if (gameEnded) return; // Stop processing after game ends

        if (other.CompareTag("Soldier"))
        {
            if (soldiersInHelicopter < maxCapacity)
            {
                soldiersInHelicopter++;
                UpdateUI();
                Destroy(other);

                // Play pickup sound
                if (audioSource != null && pickupSound != null)
                    audioSource.PlayOneShot(pickupSound);
            }
        }
        else if (other.CompareTag("Hospital"))
        {
            if (soldiersInHelicopter > 0)
            {
                soldiersRescued += soldiersInHelicopter;
                soldiersInHelicopter = 0;
                UpdateUI();

                // Win check
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

    // Optional for checking in helicopter script
    public bool GameEnded()
    {
        return gameEnded;
    }
}