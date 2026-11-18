using UnityEngine;

public class CarPunch : MonoBehaviour
{
    [SerializeField] private Rigidbody2D ballRb;
    [SerializeField] private Transform punchPoint;
    [SerializeField] private CarController carController;

    private bool punchInput;
    [SerializeField] private float punchRange = 1.2f;
    [SerializeField] private float punchForce = 7f;
    [SerializeField] private float punchCooldown = 0.25f;

    private float nextPunchTime = 0f;

    private void Update()
    {
        if (carController != null && carController.canDrive == false) return;
        if (Time.time < nextPunchTime) return;

        punchInput = false;
        if (Input.GetKeyDown(KeyCode.E)) punchInput = true;

        Vector2 from = punchPoint.position;
        Vector2 to = ballRb.position;

        if (punchInput == false) return;

        float distance = Vector2.Distance(from, to);
        if (distance > punchRange) return;

        
        Vector2 direction = (to - from).normalized;
        ballRb.AddForce(direction * punchForce, ForceMode2D.Impulse);
        nextPunchTime = Time.time + punchCooldown;
    }
}
