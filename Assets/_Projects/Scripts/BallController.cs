using UnityEngine;

public class BallController : MonoBehaviour
{

    [SerializeField] private float maxSpeed = 12f;
    [SerializeField] private float maxAngularSpeed = 720f;
    [SerializeField] private float extraLinearDamping = 0.05f;

    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity *= (1f - extraLinearDamping);

        float speed = rb.linearVelocity.magnitude;

        if (speed > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -maxAngularSpeed, maxAngularSpeed);

    }
}
