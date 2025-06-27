using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AircraftController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Movement Settings")]
    public float Speed = 6f;
    public float MaxSpeed = 12f;
    public float RotationSpeed = 360f;

    [Header("Jump & Hover")]
    public float JumpForce = 1f;
    public float JumpCooldown = 2f;
    public float HoverHeight = 2f;

    [Header("Physics")]
    public float Gravity = 9.8f;

    private float jumpCooldownTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
    }

    void FixedUpdate()
    {
        float vertical = Input.GetAxis("Vertical");
        Vector3 forwardMove = transform.forward * vertical * Speed;
        if (rb.linearVelocity.magnitude < MaxSpeed)
            rb.AddForce(forwardMove, ForceMode.Acceleration);

        float horizontal = Input.GetAxis("Horizontal");
        rb.AddTorque(0f, horizontal * RotationSpeed * Time.fixedDeltaTime, 0f, ForceMode.VelocityChange);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, HoverHeight))
        {
            float hoverError = HoverHeight - hit.distance;
            float upwardSpeed = rb.linearVelocity.y;
            float lift = hoverError * Gravity - upwardSpeed;
            rb.AddForce(Vector3.up * lift, ForceMode.Acceleration);
        }

        if (Input.GetKey(KeyCode.Space) && jumpCooldownTimer <= 0f && IsGrounded())
        {
            rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            jumpCooldownTimer = JumpCooldown;
        }

        jumpCooldownTimer -= Time.fixedDeltaTime;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, HoverHeight + 0.1f);
    }
}
