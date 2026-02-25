using UnityEngine;

public class CollisionForwarder : MonoBehaviour
{
    public GameManager gameManager;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameManager != null)
            gameManager.HandleCollision(collision.gameObject);
    }
}