using UnityEngine;

public class HelicopterMovement : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float tiltAngle = 20f;
    public float tiltSpeed = 5f;

    private Rigidbody2D rb;
    private float horizontalInput;
    private float verticalInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        HandleTilt();
    }

    void FixedUpdate()
    {
        Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized;
        rb.linearVelocity = movement * moveSpeed;
    }

    void HandleTilt()
    {
        float targetRotation = -horizontalInput * tiltAngle;

        Quaternion target = Quaternion.Euler(0, 0, targetRotation);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            target,
            tiltSpeed * Time.deltaTime
        );
    }
}