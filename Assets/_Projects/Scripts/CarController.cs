using Unity.VisualScripting;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Tuning of Speed")]
    [SerializeField] private float acceleration = 9.5f;
    [SerializeField] private float maxSpeed = 8.5f;
    [SerializeField] private float turnSpeed = 240f;

    [Header("Tuning of Reverse Speed")]
    [SerializeField] private float reverse_acceleration = 7.5f;
    [SerializeField] private float reverse_maxSpeed = 6.5f;

    [Header("Setting for LateralGrip")]
    [SerializeField, Range(0f, 1f)] private float lateralGrip = 0.8f;

    [Header("Settings for Drift")]
    [SerializeField] private float driftMinSpeed = 5f;
    [SerializeField, Range(0f, 1f)] private float driftGrip = 0.22f;
    [SerializeField] private float driftTurnMultiplier = 2.1f;

    [Header("Turn Speed Loss")]
    [SerializeField, Range(0f, 0.8f)] private float turnAccelCut = 0.25f;
    [SerializeField] private float turnAccelCutMinSpeed = 2f;

    private float throttleInput;
    private float steerInput;
    private bool handbrakeInput = false;

    public bool canDrive = true;

    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        throttleInput = 0f;
        steerInput = 0f;
        handbrakeInput = false;

        if (Input.GetKey(KeyCode.W)) throttleInput += 1f;
        if (Input.GetKey(KeyCode.S)) throttleInput -= 1f;

        if (Input.GetKey(KeyCode.D)) steerInput -= 1f;
        if (Input.GetKey(KeyCode.A)) steerInput += 1f;

        if (Input.GetKey(KeyCode.Space)) handbrakeInput = true;

    }

    private void FixedUpdate()
    {
        if (canDrive == false) { rb.linearVelocity = Vector2.zero; rb.angularVelocity = 0f; return; }

        Vector2 forward = transform.right;
        Vector2 lateral = transform.up;

        Vector2 v = rb.linearVelocity;
        
        float forwardSpeed = Vector2.Dot(rb.linearVelocity, forward);
        float steerDir = (forwardSpeed >= 0f) ? 1f : -1f;

        float speed = rb.linearVelocity.magnitude;


        bool drifting = handbrakeInput && speed > driftMinSpeed;
        float drift01 = drifting ? Mathf.Clamp01((speed - driftMinSpeed) / (maxSpeed - driftMinSpeed)) : 0f;

        float steerAbs = Mathf.Abs(steerInput);
        float accelMultiplier = 1f;

        if (speed > turnAccelCutMinSpeed)
        {
            accelMultiplier = 1f - (steerAbs * turnAccelCut);
        }

        if (throttleInput > 0f && speed <= maxSpeed)
        {
            rb.AddForce(acceleration * accelMultiplier * forward * throttleInput);
        }
        
        if(throttleInput < 0f && speed <= reverse_maxSpeed)
        {
            rb.AddForce(reverse_acceleration * accelMultiplier * forward * throttleInput);
        }

        float speed01 = Mathf.Clamp01(speed/maxSpeed);

        float steerStrength = Mathf.Pow(speed01, 1.5f);
        float turnMul = drifting ? Mathf.Lerp(1f, driftTurnMultiplier, drift01) : 1f;

        float turnAmount = turnSpeed * steerStrength * steerInput * steerDir * turnMul * Time.fixedDeltaTime;

        rb.MoveRotation(rb.rotation + turnAmount);

        v = rb.linearVelocity;

        float vForward = Vector2.Dot(v, forward);
        float vLateral = Vector2.Dot(v, lateral);

        Vector2 forwardVel = vForward * forward;
        Vector2 lateralVel = vLateral * lateral;

        float gripUsed = drifting ? Mathf.Lerp(lateralGrip, driftGrip, drift01) : lateralGrip;

        Vector2 lateralKept = lateralVel * (1f - gripUsed);
        rb.linearVelocity = forwardVel + lateralKept;

    }
}
