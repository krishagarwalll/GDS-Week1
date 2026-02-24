using UnityEngine;

public class HelicopterCollisionForwarder : MonoBehaviour
{
    public GameManager gameManager;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameManager != null)
            gameManager.HandleCollision(collision.gameObject);
    }
}