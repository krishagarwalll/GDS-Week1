using UnityEngine;
using TMPro;
using UnityEngine.UI;
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

    void Start()
    {
        UpdateUI();
        messageText.text = "";
    }

    void Update()
    {
        // Reset game
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void HandleCollision(GameObject other)
    {
        if (other.CompareTag("Soldier"))
        {
            if (soldiersInHelicopter < maxCapacity)
            {
                soldiersInHelicopter++;
                UpdateUI();
                Destroy(other); // remove soldier
                // Play pickup sound here
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
                    messageText.text = "YOU WIN!";
            }
        }
        else if (other.CompareTag("Tree"))
        {
            messageText.text = "GAME OVER";
            helicopter.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
    }

    void UpdateUI()
    {
        soldiersInHelicopterText.text = "In Helicopter: " + soldiersInHelicopter;
        soldiersRescuedText.text = "Rescued: " + soldiersRescued;
    }
}