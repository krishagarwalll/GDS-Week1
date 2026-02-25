using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    public GameObject player;
    public int maxCapacity = 3;

    [Header("Tags")]
    public string collectibleTag;
    public string baseTag;
    public string obstacleTag;

    [Header("UI")]
    public TMP_Text inVehicleText;
    public TMP_Text totalCollectedText;
    public TMP_Text messageText;

    [Header("Text Labels")]
    public string inVehicleLabel = "In Vehicle: ";
    public string totalCollectedLabel = "Delivered: ";
    public string winMessage = "YOU WIN!";
    public string loseMessage = "GAME OVER";

    private int inVehicle = 0;
    private int totalCollected = 0;
    private bool gameEnded = false;

    void Start()
    {
        UpdateUI();
        if (messageText != null)
            messageText.text = "";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void HandleCollision(GameObject other)
    {
        if (gameEnded) return;

        if (other.CompareTag(collectibleTag))
        {
            if (inVehicle < maxCapacity)
            {
                inVehicle++;
                UpdateUI();
                Destroy(other);
            }
        }
        else if (other.CompareTag(baseTag))
        {
            if (inVehicle > 0)
            {
                totalCollected += inVehicle;
                inVehicle = 0;
                UpdateUI();

                if (GameObject.FindGameObjectsWithTag(collectibleTag).Length == 0)
                {
                    if (messageText != null)
                        messageText.text = winMessage;

                    EndGame();
                }
            }
        }
        else if (other.CompareTag(obstacleTag))
        {
            if (messageText != null)
                messageText.text = loseMessage;

            EndGame();
        }
    }

    void UpdateUI()
    {
        if (inVehicleText != null)
            inVehicleText.text = inVehicleLabel + inVehicle;

        if (totalCollectedText != null)
            totalCollectedText.text = totalCollectedLabel + totalCollected;
    }

    void EndGame()
    {
        gameEnded = true;

        if (player != null)
        {
            // Disable ALL scripts on player
            foreach (MonoBehaviour script in player.GetComponents<MonoBehaviour>())
            {
                if (script != this)
                    script.enabled = false;
            }

            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = Vector2.zero;
        }
    }
}