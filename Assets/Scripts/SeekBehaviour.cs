using UnityEngine;

public class SeekBehaviour2D : MonoBehaviour
{
    [Header("Configuración del Agente")]
    public Transform target;
    public float maxSpeed = 5f;
    public float maxForce = 0.1f;
    public float mass = 1f;

    [Header("Configuración de Arrival")]
    public float slowingRadius = 3f;

    [Header("Configuración de Evasión")]
    public float avoidDistance = 3f;
    public float avoidForce = 5f;

    private Vector2 velocity;

    void Update()
    {
        if (target == null) return;

        // 1. SEEK Y ARRIVAL
        Vector2 targetOffset = target.position - transform.position;
        float distance = targetOffset.magnitude;
        Vector2 desiredVelocity;

        if (distance < slowingRadius)
        {
            desiredVelocity = targetOffset.normalized * maxSpeed * (distance / slowingRadius);
        }
        else
        {
            desiredVelocity = targetOffset.normalized * maxSpeed;
        }

        Vector2 steering = desiredVelocity - velocity;
        steering = Vector2.ClampMagnitude(steering, maxForce);

        // 2. OBSTACLE AVOIDANCE (Versión 2D)
        Vector2 avoidance = Vector2.zero;

        // Physics2D devuelve la información directo, no ocupa el "out hit" del 3D
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, avoidDistance);

        // Si el collider no es nulo, significa que el rayo chocó con algo
        if (hit.collider != null)
        {
            avoidance = hit.normal * avoidForce;
        }

        steering += avoidance;
        steering /= mass;

        // 3. MOVIMIENTO
        velocity = Vector2.ClampMagnitude(velocity + steering, maxSpeed);

        // Se castea a Vector3 nomás porque transform.position así lo pide
        transform.position += (Vector3)velocity * Time.deltaTime;

        // 4. ROTACIÓN 2D
        if (velocity != Vector2.zero)
        {
            // En 2D, la parte de "arriba" del sprite es la que apunta hacia donde camina
            transform.up = Vector3.Slerp(transform.up, velocity.normalized,Time.deltaTime * 15f);
        }
    }

    private void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(target.position, slowingRadius);
        }

        Gizmos.color = Color.red;
        // Dibujamos el rayo usando transform.up para que coincida con la vista 2D
        Gizmos.DrawLine(transform.position, transform.position + transform.up * avoidDistance);
    }
}