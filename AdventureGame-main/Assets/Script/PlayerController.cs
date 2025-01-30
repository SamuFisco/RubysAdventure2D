using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
//using UnityEngine.SceneManager; // Para reiniciar la partida

public class PlayerController : MonoBehaviour
{
    // 🎮 Controles del jugador (Movimiento, Disparo, Interacción)
    public InputAction MoveAction; // Acción para mover al personaje
    public InputAction _fire; // Acción para disparar
    public InputAction _interact; // Acción para interactuar con NPCs

    private Rigidbody2D rigidbody2d; // 💪 Cuerpo del jugador, como el chasis de un coche que recibe movimiento
    private Vector2 move; // 📍 Dirección en la que se mueve el jugador
    public float speed = 3.0f; // 🚗 Velocidad del jugador

    // ❤️ Sistema de salud del jugador
    public int maxHealth = 5; // Salud máxima del jugador
    public int health { get { return currentHealth; } } // Propiedad para obtener la salud actual
    private int currentHealth; // Salud actual del jugador

    public float timeInvincible = 2.0f; // ⏳ Tiempo de invulnerabilidad tras recibir daño
    private bool isInvincible; // 🛡️ Indica si el jugador es invulnerable
    private float damageCooldown; // ⏳ Contador del tiempo de invulnerabilidad

    private Animator animator; // 🎭 Controlador de animaciones del personaje
    private Vector2 moveDirection = new Vector2(1, 0); // 📍 Dirección en la que el jugador está mirando

    // 🔫 Proyectiles y disparo
    public GameObject projectilePrefab; // Prefab del proyectil que dispara el jugador
    public float projectileForce = 500f; // 💥 Fuerza con la que el proyectil es disparado

    // 🔊 Audio
    private AudioSource audioSource; // 📢 Fuente de sonido para el jugador
    public AudioClip damageSound; // 🔊 Sonido de daño
    public AudioClip walkSound; // 🎵 Sonido al caminar
    public float damageSoundVolume = 1.0f; // 🔊 Volumen del sonido de daño
    public float walkSoundVolume = 1.0f; // 🎵 Volumen del sonido al caminar

    void Start()
    {
        // 🔄 Activa las acciones del jugador (teclado y mando)
        MoveAction.Enable();
        _fire.Enable();
        _interact.Enable();

        // 📌 Obtiene componentes esenciales
        rigidbody2d = GetComponent<Rigidbody2D>(); // Obtiene el Rigidbody2D
        currentHealth = maxHealth; // 💖 Inicializa la salud del jugador al máximo
        animator = GetComponent<Animator>(); // Obtiene el Animator
        audioSource = GetComponent<AudioSource>(); // Obtiene el AudioSource
    }

    void Update()
    {
        // 📍 Captura el movimiento del jugador
        move = MoveAction.ReadValue<Vector2>();

        // 📏 Si el jugador se está moviendo, actualiza la dirección en la que mira
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize(); // Normaliza la dirección (para que siempre sea de longitud 1)
        }

        // 🎭 Actualiza los parámetros de la animación (dirección y velocidad)
        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        // ⏳ Reduce el tiempo de invulnerabilidad si está activo
        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown < 0)
            {
                isInvincible = false; // 🔓 Vuelve a ser vulnerable
            }
        }

        // 🎵 Si el jugador se mueve, reproduce el sonido de caminar
        if (move.magnitude > 0.1f && audioSource != null && !audioSource.isPlaying)
        {
            if (walkSound != null)
            {
                audioSource.PlayOneShot(walkSound, walkSoundVolume);
            }
        }

        // 🔫 Si el jugador presiona el botón de disparo, lanza un proyectil
        if (_fire.WasPressedThisFrame())
        {
            Launch();
        }

        // 🤝 Si el jugador presiona el botón de interactuar, busca NPCs cercanos
        if (_interact.WasPressedThisFrame())
        {
            FindFriend();
        }
    }

    void FixedUpdate()
    {
        // 🏃 Mueve al jugador en la dirección deseada con una velocidad constante
        Vector2 position = rigidbody2d.position + move * speed * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        // 💥 Si el daño es negativo, verifica si el jugador es invulnerable
        if (amount < 0)
        {
            if (isInvincible) return; // ⛔ No recibe daño si es invulnerable

            isInvincible = true;
            damageCooldown = timeInvincible;
            animator.SetTrigger("Hit"); // 🎭 Activa la animación de daño

            if (damageSound != null)
            {
                audioSource.PlayOneShot(damageSound, damageSoundVolume);
            }
        }

        // 💖 Ajusta la salud del jugador dentro de los límites
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHandler1.instance.SetHealthValue(currentHealth / (float)maxHealth);

        Debug.Log("Vida actual: " + currentHealth);

        // ☠️ Si la salud llega a 0, reinicia la partida
        if (currentHealth <= 0)
        {
            Debug.Log("¡Jugador sin vida! Reiniciando partida...");
            RestartGame();
        }
    }

    void RestartGame()
    {
        StartCoroutine(RestartCoroutine()); // ⏳ Espera antes de reiniciar la escena
    }

    IEnumerator RestartCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Launch()
    {
        // 🎯 Instancia un proyectil en la dirección en la que el jugador está mirando
        GameObject projectileObject = Instantiate(projectilePrefab,
                                    rigidbody2d.position + Vector2.up * 0.5f,
                                    Quaternion.identity);

        // 🚀 Aplica fuerza al proyectil
        Rigidbody2D projectileRb = projectileObject.GetComponent<Rigidbody2D>();
        if (projectileRb != null)
        {
            projectileRb.AddForce(moveDirection * projectileForce);
        }
        else
        {
            Debug.LogError("El proyectil no tiene un Rigidbody2D asignado.");
        }

        animator.SetTrigger("Launch"); // 🎭 Activa la animación de disparo
    }

    void FindFriend()
    {
        // 🔍 Lanza un rayo en la dirección del jugador para encontrar NPCs
        RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f,
                                             moveDirection,
                                             1.5f,
                                             LayerMask.GetMask("NPC"));

        if (hit.collider != null)
        {
            NPC1 npc1 = hit.collider.GetComponent<NPC1>();
            if (npc1 != null)
            {
                Debug.Log("Jugador interactuando con NPC1");
                UIHandler1.instance.DisplayDialogueNPC1();
                return;
            }

            NPC2 npc2 = hit.collider.GetComponent<NPC2>();
            if (npc2 != null)
            {
                Debug.Log("Jugador interactuando con NPC2");
                UIHandler1.instance.DisplayDialogueNPC2();
                return;
            }
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("AudioClip no asignado.");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            ChangeHealth(-1); // 💥 Recibe daño si colisiona con ciertos objetos
        }
    }
}
