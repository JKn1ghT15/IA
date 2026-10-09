using UnityEngine;

public class WanderBehaviour2D : MonoBehaviour
{
    public float maxSpeed = 3f;
    public float maxForce = 0.1f;
    public float mass = 1f;

    [Header("Configuración Wander")]
    public float wanderDistance = 2f;
    public float wanderRadius = 1.5f;
    public float angleChange = 15f;
    private float wanderAngle = 0f;

    [Header("Configuración de Evasión")]
    public float avoidDistance = 3f;
    public float avoidForce = 5f;

    private Vector2 velocity;

    void Update()
    {
        // Matemáticas para el círculo imaginario del Wander
        wanderAngle += Random.Range(-angleChange, angleChange);
        Vector2 circleCenter = (Vector2)transform.position + ((Vector2)transform.up * wanderDistance);

        float angleRad = wanderAngle * Mathf.Deg2Rad;
        Vector2 displacement = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * wanderRadius;

        Vector2 targetPoint = circleCenter + displacement;

        // Seek hacia el punto imaginario
        Vector2 desiredVelocity = (targetPoint - (Vector2)transform.position).normalized * maxSpeed;

        Vector2 steering = desiredVelocity - velocity;
        steering = Vector2.ClampMagnitude(steering, maxForce);

        Vector2 avoidance = Vector2.zero;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, avoidDistance);
        if (hit.collider != null) avoidance = hit.normal * avoidForce;

        steering += avoidance;
        steering /= mass;

        velocity = Vector2.ClampMagnitude(velocity + steering, maxSpeed);
        transform.position += (Vector3)velocity * Time.deltaTime;

        if (velocity != Vector2.zero)
            transform.up = Vector3.Slerp(transform.up, velocity.normalized, Time.deltaTime * 15f);
    }
}
