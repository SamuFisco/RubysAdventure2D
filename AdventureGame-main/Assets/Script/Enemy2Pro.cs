using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Pro : MonoBehaviour
{
    public float speed = 2.0f; // Velocidad del enemigo
    public bool vertical = false; // Movimiento vertical u horizontal (solo informativo, siempre dispara hacia -y)
    public int damage = 1; // Daño al jugador
    public GameObject projectilePrefab; // Prefab del proyectil
    public float shootInterval = 2.0f; // Intervalo de disparo en segundos
    public float projectileImpulse = 5.0f; // Impulso inicial del proyectil

    private float direction = 1.0f; // Dirección de movimiento
    private Rigidbody2D rigidbody2d;
    private float movementTimer;
    private float shootTimer;
    public float changeDirectionTime = 3.0f; // Tiempo para cambiar dirección

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>(); // Obtener Rigidbody2D
        movementTimer = changeDirectionTime; // Inicializar temporizador de movimiento
        shootTimer = shootInterval; // Inicializar temporizador de disparo
        gameObject.layer = LayerMask.NameToLayer("Default"); // Asignar la capa Default
    }

    void Update()
    {
        // Gestionar cambio de dirección
        movementTimer -= Time.deltaTime;
        if (movementTimer <= 0)
        {
            direction = -direction; // Cambiar dirección
            movementTimer = changeDirectionTime; // Reiniciar temporizador de movimiento
        }

        // Gestionar disparos
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {
            ShootProjectile(); // Disparar proyectil
            shootTimer = shootInterval; // Reiniciar temporizador de disparo
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position; // Obtener posición actual

        if (vertical)
        {
            position.y += speed * direction * Time.deltaTime; // Movimiento vertical
        }
        else
        {
            position.x += speed * direction * Time.deltaTime; // Movimiento horizontal
        }

        rigidbody2d.MovePosition(position); // Actualizar posición
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Character")) // Verificar colisión con el jugador
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>(); // Obtener script del jugador
            if (player != null)
            {
                player.ChangeHealth(-damage); // Reducir la salud del jugador
            }
        }
    }

    void ShootProjectile()
    {
        if (projectilePrefab != null)
        {
            // Instanciar el proyectil en la posición actual del enemigo
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();

            if (projectileRb != null)
            {
                // Darle impulso hacia abajo al proyectil
                projectileRb.AddForce(Vector2.down * projectileImpulse, ForceMode2D.Impulse);
            }

            // Destruir el proyectil si no impacta con nada en 2 segundos
            Destroy(projectile, 0.50f);
        }
    }
}
