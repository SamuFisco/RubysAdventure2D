using UnityEngine;

public class Projectil : MonoBehaviour
{
    // Rigidbody2D del proyectil, como el chasis de un cohete que lo mantiene estable.
    private Rigidbody2D rigidbody2d;

    // Efecto de part�culas para la explosi�n
    public GameObject explosionPrefab;

    // Awake se llama cuando el GameObject del proyectil se crea (como encender el cohete antes de lanzarlo).
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Start se llama antes del primer frame
    void Start()
    {
        // Destruir el proyectil despu�s de 1 segundo
        Destroy(gameObject, 1.0f);
    }

    // Update se llama cada cuadro
    void Update()
    {
        // Si el proyectil est� fuera de rango, tambi�n destruirlo
        if (transform.position.magnitude > 100.0f)
        {
            Destroy(gameObject);
        }
    }

    // M�todo para "lanzar" el proyectil
    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    // OnTriggerEnter2D se llama cuando este proyectil colisiona con un collider
    void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si colisiona con un enemigo
        if (other.CompareTag("Enemy"))
        {
            // Instanciar la explosi�n en la posici�n de impacto
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }

            // Aplica la l�gica de "reparaci�n" o da�o al enemigo
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.Fix();
            }
        }

        // Destruye el proyectil inmediatamente al colisionar
        Destroy(gameObject);
    }
}
