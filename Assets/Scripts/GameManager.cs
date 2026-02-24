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
    public AudioSource backgroundMusicSource;

    private AudioSource audioSource;
    private int soldiersInHelicopter = 0;
    private int soldiersRescued = 0;
    private bool gameEnded = false;

    void Start()
    {
        UpdateUI();
        messageText.text = "";

        // Helicopter AudioSource for pickup sound
        if (helicopter != null)
            audioSource = helicopter.GetComponent<AudioSource>();

        // Play background music if assigned
        if (backgroundMusicSource != null && !backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.loop = true;
            backgroundMusicSource.Play();
        }
    }

    void Update()
    {
        // Reset game
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void HandleCollision(GameObject other)
    {
        if (gameEnded) return;

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