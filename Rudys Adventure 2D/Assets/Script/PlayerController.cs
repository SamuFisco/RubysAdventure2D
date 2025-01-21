using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Variables related to player character movement
    public InputAction MoveAction;
    Rigidbody2D rigidbody2d;
    Vector2 move;
    public float speed = 3.0f;

    // Variables related to the health system
    public int maxHealth = 5;
    public int health { get { return currentHealth; } }
    int currentHealth;

    // Variables related to temporary invincibility
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float damageCooldown;

    // Variables related to Animation
    Animator animator;
    Vector2 moveDirection = new Vector2(1, 0);

    // Variables related to Projectile
    public GameObject projectilePrefab;

    // Variables related to audio
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        MoveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();

        // Actualizar moveDirection si hay movimiento
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
        }

        // Enviar valores al Animator
        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        // Manejo de invencibilidad temporal
        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown < 0)
            {
                isInvincible = false;
            }
        }

        // Lanzar proyectil con tecla C
        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        // Buscar NPC con tecla X
        if (Input.GetKeyDown(KeyCode.X))
        {
            FindFriend();
        }
    }

    // FixedUpdate has the same call rate as the physics system 
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position + move * speed * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        // Si el jugador recibe daño
        if (amount < 0)
        {
            if (isInvincible)
            {
                return;
            }
            isInvincible = true;
            damageCooldown = timeInvincible;
            animator.SetTrigger("Hit");
        }
        // Actualizar salud
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        // Actualizar barra de salud en la UI
        UIHandler1.instance.SetHealthValue(currentHealth / (float)maxHealth);
    }

    // Instanciar el proyectil y lanzarlo
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab,
                                    rigidbody2d.position + Vector2.up * 0.5f,
                                    Quaternion.identity);

        // Usar la clase Projectil, no Projectile
        Projectil projectile = projectileObject.GetComponent<Projectil>();
        if (projectile != null)
        {
            projectile.Launch(moveDirection, 300);
        }

        animator.SetTrigger("Launch");
    }

    // Buscar al NPC delante del jugador
    void FindFriend()
    {
        RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f,
                                             moveDirection,
                                             1.5f,
                                             LayerMask.GetMask("NPC"));
        if (hit.collider != null)
        {
            // Usar NonPlayerCharacter1, no NonPlayerCharacter
            NonPlayerCharacter1 character = hit.collider.GetComponent<NonPlayerCharacter1>();
            if (character != null)
            {
                UIHandler1.instance.DisplayDialogue();
            }
        }
    }

    // Función para reproducir un efecto de sonido puntual
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
