
using UnityEngine;
using UnityEngine.InputSystem;

public class FleeBehaviour2D : MonoBehaviour
{
    [Header("Movimiento")]
    public float maxSpeed = 5f;
    public float maxForce = 0.1f;
    public float mass = 1f;

    [Header("Detección del mouse")]
    public float detectionRadius = 3f;

    [Header("Evasión de obstáculos")]
    public float avoidDistance = 3f;
    public float avoidForce = 5f;

    private Vector2 velocity;

    void Update()
    {
        if (Mouse.current == null || Camera.main == null)
            return;

        // Obtener la posición del mouse en el mundo 2D
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(
                mouseScreenPos.x,
                mouseScreenPos.y,
                -Camera.main.transform.position.z
            )
        );

        Vector2 mousePos = mouseWorldPos;

        // Calcular distancia entre el personaje y el mouse
        Vector2 targetOffset = (Vector2)transform.position - mousePos;
        float distance = targetOffset.magnitude;

        Vector2 steering = Vector2.zero;

        // Solo escapar si el mouse entra en el radio
        if (distance <= detectionRadius && distance > 0.01f)
        {
            Vector2 desiredVelocity =
                targetOffset.normalized * maxSpeed;

            steering = desiredVelocity - velocity;
            steering = Vector2.ClampMagnitude(steering, maxForce);
        }
        else
        {
            // Fuera del radio, frenar progresivamente
            steering = -velocity;
            steering = Vector2.ClampMagnitude(steering, maxForce);
        }

        // Detectar obstáculos en la dirección del personaje
        Vector2 avoidance = Vector2.zero;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            transform.up,
            avoidDistance
        );

        if (hit.collider != null)
        {
            avoidance = hit.normal * avoidForce;
        }

        steering += avoidance;
        steering /= Mathf.Max(mass, 0.01f);

        velocity = Vector2.ClampMagnitude(
            velocity + steering,
            maxSpeed
        );

        transform.position +=
            (Vector3)(velocity * Time.deltaTime);

        if (velocity.sqrMagnitude > 0.001f)
        {
            transform.up = Vector3.Slerp(
                transform.up,
                velocity.normalized,
                Time.deltaTime * 15f
            );
        }
    }

    // Dibujar el radio en la ventana Scene
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}