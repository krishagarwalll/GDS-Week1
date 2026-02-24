using UnityEngine;

public class HelicopterCollisionForwarder : MonoBehaviour
{
    public GameManager gameManager;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Helicopter collided with: " + collision.name);
        gameManager.HandleCollision(collision.gameObject);
    }
}