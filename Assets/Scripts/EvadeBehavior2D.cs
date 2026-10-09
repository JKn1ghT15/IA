
using UnityEngine;

public class EvadeBehaviour2D : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform target;

    [Header("Movimiento")]
    public float maxSpeed = 5.5f;
    public float maxForce = 2f;
    public float mass = 1f;

    [Header("Predicción")]
    public float maxPredictionTime = 2f;

    [Header("Evasión de obstáculos")]
    public float avoidDistance = 2f;
    public float avoidForce = 8f;
    public LayerMask obstacleMask = ~0;

    private Vector2 velocity;
    private Vector2 lastTargetPos;
    private Vector2 targetVelocity;

    void Start()
    {
        if (target != null)
            lastTargetPos = target.position;
    }

    void Update()
    {
        if (target == null)
            return;

        float dt = Time.deltaTime;
        if (dt <= 0f)
            return;

        // 1. Calcular la velocidad del perseguidor
        Vector2 currentTargetPos = target.position;

        targetVelocity = (currentTargetPos - lastTargetPos) / dt;
        lastTargetPos = currentTargetPos;

        // 2. Predecir dónde estará el perseguidor
        float distance = Vector2.Distance(
            transform.position, currentTargetPos);

        float predictionTime = Mathf.Min(
            distance / Mathf.Max(targetVelocity.magnitude, 0.1f),
            maxPredictionTime);

        Vector2 futurePosition =
            currentTargetPos + targetVelocity * predictionTime;

        // 3. Flee: alejarse de la posición futura
        Vector2 fleeDirection =
            (Vector2)transform.position - futurePosition;

        Vector2 desiredVelocity = fleeDirection.sqrMagnitude > 0.001f
            ? fleeDirection.normalized * maxSpeed
            : Vector2.zero;

        Vector2 steering = desiredVelocity - velocity;
        steering = Vector2.ClampMagnitude(steering, maxForce);

        // 4. Detectar y evitar obstáculos
        Vector2 moveDirection = velocity.sqrMagnitude > 0.01f
            ? velocity.normalized
            : fleeDirection.normalized;

        steering += CalculateObstacleAvoidance(moveDirection);

        // 5. Actualizar velocidad y posición
        velocity += steering / Mathf.Max(mass, 0.01f) * dt;
        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);

        transform.position += (Vector3)(velocity * dt);

        // 6. Orientar el agente hacia su movimiento
        if (velocity.sqrMagnitude > 0.001f)
        {
            transform.up = Vector3.Slerp(
                transform.up,
                velocity.normalized,
                dt * 15f);
        }
    }

    Vector2 CalculateObstacleAvoidance(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.001f)
            return Vector2.zero;

        direction.Normalize();

        Vector2 origin = transform.position;
        Vector2 leftDir = Quaternion.Euler(0, 0, 30f) * direction;
        Vector2 rightDir = Quaternion.Euler(0, 0, -30f) * direction;

        RaycastHit2D centerHit = Physics2D.Raycast(
            origin, direction, avoidDistance, obstacleMask);

        RaycastHit2D leftHit = Physics2D.Raycast(
            origin, leftDir, avoidDistance, obstacleMask);

        RaycastHit2D rightHit = Physics2D.Raycast(
            origin, rightDir, avoidDistance, obstacleMask);

        Vector2 avoidance = Vector2.zero;

        if (centerHit.collider != null)
            avoidance += centerHit.normal * 2f;

        if (leftHit.collider != null)
            avoidance += leftHit.normal;

        if (rightHit.collider != null)
            avoidance += rightHit.normal;

        if (avoidance.sqrMagnitude > 0.001f)
            avoidance = avoidance.normalized * avoidForce;

        return avoidance;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(transform.position,
            transform.up * avoidDistance);
    }
}