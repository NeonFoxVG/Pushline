using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    public float moveSpeed = 50f;
    public float maxSpeed = 15f;
    public float drag = 0.98f;
    public float steerAngle = 20f;
    public float traction = 1f;

    private Vector3 moveForce;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void FixedUpdate()
    {
        // Car Motion
        moveForce += transform.forward * moveSpeed * Input.GetAxis("Vertical") * Time.fixedDeltaTime;

        // Move with Rigidbody instead of transform.position
        rb.MovePosition(rb.position + moveForce * Time.fixedDeltaTime);

        // Steering
        float steerInput = Input.GetAxis("Horizontal");
        Quaternion turn = Quaternion.Euler(
            0,
            steerInput * moveForce.magnitude * steerAngle * Time.fixedDeltaTime,
            0
        );
        rb.MoveRotation(rb.rotation * turn);

        // Drag & Speed Limiter
        moveForce *= drag;
        moveForce = Vector3.ClampMagnitude(moveForce, maxSpeed);

        // Car Traction
        moveForce = Vector3.Lerp(
            moveForce.normalized,
            transform.forward,
            traction * Time.fixedDeltaTime
        ) * moveForce.magnitude;

        Debug.DrawRay(transform.position, moveForce.normalized * 3);
        Debug.DrawRay(transform.position, transform.forward * 3, Color.blue);
    }
}