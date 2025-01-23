using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Variables relacionadas con el movimiento del jugador
    //Es como el volante y el acelerador de un coche: controlan hacia d�nde y a qu� velocidad te mueves.
    public InputAction MoveAction;
    private Rigidbody2D rigidbody2d;
    private Vector2 move;
    public float speed = 3.0f;

    //Variables relacionadas con el sistema de salud
    //Piensa en la salud como el tanque de combustible: cuando llega a cero, el viaje termina.
    public int maxHealth = 5;
    public int health { get { return currentHealth; } }
    private int currentHealth;

    //Variables relacionadas con la invencibilidad temporal
    //Es como un escudo pasajero que te protege un tiempo y despu�s se desactiva.
    public float timeInvincible = 2.0f;
    private bool isInvincible;
    private float damageCooldown;

    //Variables relacionadas con la animaci�n
    //Es como el panel de control que muestra en qu� estado est� el jugador (caminando, atacando, recibiendo da�o).
    private Animator animator;
    private Vector2 moveDirection = new Vector2(1, 0);

    //Variables relacionadas con el proyectil
    //Piensa en el proyectil como un disparo o piedra que lanzas para atacar a distancia.
    public GameObject projectilePrefab;

    //Variables relacionadas con el audio
    //AudioSource es como la radio del coche: aqu� reproduces canciones (efectos de sonido en este caso).
    private AudioSource audioSource;
    public AudioClip damageSound; //Sonido que se reproduce cuando el jugador recibe da�o
    public float damageSoundVolume = 1.0f; //Volumen para el sonido de da�o
    public float walkSoundVolume = 1.0f; //Volumen para el sonido al caminar

    //Se llama antes de que comience el primer frame
    //Es como encender el coche y revisar los sistemas b�sicos.
    void Start()
    {
        MoveAction.Enable(); //Habilitar la entrada de movimiento del jugador
        rigidbody2d = GetComponent<Rigidbody2D>(); //Obtener el componente Rigidbody2D para interacciones f�sicas
        currentHealth = maxHealth; //Inicializar la salud del jugador al m�ximo
        animator = GetComponent<Animator>(); //Obtener el componente Animator para las animaciones
        audioSource = GetComponent<AudioSource>(); //Obtener el componente AudioSource para la reproducci�n de audio
        audioSource.volume = walkSoundVolume; //Ajustar el volumen del sonido de caminar
    }

    //Se llama en cada frame
    //Es como verificar constantemente hacia d�nde est� girando el volante y cu�nto aceleras.
    void Update()
    {
        move = MoveAction.ReadValue<Vector2>(); //Leer la entrada de movimiento del jugador

        //Actualizar la direcci�n de movimiento si hay movimiento
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            moveDirection.Set(move.x, move.y); //Establecer la direcci�n basada en la entrada
            moveDirection.Normalize(); //Normalizar el vector de direcci�n para mantener un movimiento consistente
        }

        //Enviar valores al Animator
        animator.SetFloat("Look X", moveDirection.x); //Actualizar la direcci�n X en el animator
        animator.SetFloat("Look Y", moveDirection.y); //Actualizar la direcci�n Y en el animator
        animator.SetFloat("Speed", move.magnitude); //Actualizar el par�metro de velocidad en el animator

        //Manejo de la invencibilidad temporal
        //Como un escudo que desaparece con el tiempo.
        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime; //Reducir el tiempo de invencibilidad
            if (damageCooldown < 0)
            {
                isInvincible = false; //Deshabilitar la invencibilidad cuando termine el tiempo
            }
        }

        //Sonido al caminar
        //Si te mueves, es como el ruido de pasos que haces. Si te paras, lo dejas de hacer.
        if (move.magnitude > 0.1f && !audioSource.isPlaying)
        {
            audioSource.loop = true; //Habilitar el bucle para el sonido continuo de caminar
            audioSource.volume = walkSoundVolume; //Ajustar el volumen del sonido de caminar
            audioSource.Play(); //Reproducir el sonido de caminar
        }
        else if (move.magnitude <= 0.1f && audioSource.isPlaying)
        {
            audioSource.loop = false; //Detener el bucle cuando el jugador deja de moverse
            audioSource.Stop(); //Detener el sonido de caminar
        }

        //Lanzar proyectil con la tecla C
        //Es como arrojar una piedra o disparar una flecha.
        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch(); //Llamar al m�todo para disparar un proyectil
        }

        //Buscar NPC con la tecla X
        //Como pedir ayuda o interactuar con alguien delante de ti.
        if (Input.GetKeyDown(KeyCode.X))
        {
            FindFriend(); //Llamar al m�todo para interactuar con un NPC
        }
    }

    //Se llama a la misma tasa que el sistema de f�sica
    //Como ajustar la posici�n del coche seg�n el volante y el acelerador de forma suave.
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position + move * speed * Time.deltaTime; //Calcular la nueva posici�n basada en la entrada y la velocidad
        rigidbody2d.MovePosition(position); //Mover al jugador a la nueva posici�n
    }

    public void ChangeHealth(int amount)
    {
        //Si el jugador recibe da�o
        //Es como quitar combustible del tanque cuando te golpean.
        if (amount < 0)
        {
            if (isInvincible)
            {
                return; //Ignorar el da�o si el jugador es invencible
            }
            isInvincible = true; //Habilitar la invencibilidad
            damageCooldown = timeInvincible; //Restablecer el tiempo de invencibilidad
            animator.SetTrigger("Hit"); //Activar la animaci�n de da�o

            //Reproducir sonido de da�o
            //Como un quejido al recibir un golpe.
            if (damageSound != null)
            {
                audioSource.PlayOneShot(damageSound, damageSoundVolume); //Reproducir el sonido de da�o con volumen ajustable
            }
        }

        //Actualizar la salud
        //Mantener el rango de salud entre 0 y el m�ximo, como un tanque de gasolina que no puede pasar de su capacidad.
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        //Actualizar la barra de salud en la UI
        //Como mover la aguja del indicador de combustible.
        UIHandler1.instance.SetHealthValue(currentHealth / (float)maxHealth);
    }

    //Instanciar y lanzar el proyectil
    //Como arrojar una piedra o disparar un proyectil desde tu posici�n.
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab,
                                    rigidbody2d.position + Vector2.up * 0.5f,
                                    Quaternion.identity);

        //Usar la clase Projectil, no Projectile
        //Como verificar que la piedra lanzada sea del tipo correcto.
        Projectil projectile = projectileObject.GetComponent<Projectil>();
        if (projectile != null)
        {
            projectile.Launch(moveDirection, 300); //Lanzar el proyectil en la direcci�n especificada con fuerza
        }

        animator.SetTrigger("Launch"); //Activar la animaci�n de disparo
    }

    //Buscar NPC frente al jugador
    //Como mirar al frente para ver si hay alguien con quien interactuar.
    void FindFriend()
    {
        RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f,
                                             moveDirection,
                                             1.5f,
                                             LayerMask.GetMask("NPC"));
        if (hit.collider != null)
        {
            //Usar la clase NonPlayerCharacter1, no NonPlayerCharacter
            //Como verificar si el objeto frente a ti es realmente un NPC.
            NonPlayerCharacter1 character = hit.collider.GetComponent<NonPlayerCharacter1>();
            if (character != null)
            {
                UIHandler1.instance.DisplayDialogue(); //Mostrar el di�logo en la UI
            }
        }
    }

    //Funci�n para reproducir un efecto de sonido �nico
    //Como pulsar una bocina puntual, sin mantenerlo en bucle.
    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip); //Reproducir el efecto de sonido una vez
        }
        else
        {
            Debug.LogWarning("AudioClip no asignado."); //Advertir si no se ha asignado un AudioClip
        }
    }

    //Manejar colisiones con la capa Default
    //Es como chocar con objetos normales en el escenario.
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            ChangeHealth(-1); //Reducir la salud en 1
        }
    }
}
