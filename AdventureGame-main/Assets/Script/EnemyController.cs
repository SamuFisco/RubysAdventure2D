using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //Variables públicas
    public float speed;            //Velocidad de movimiento del enemigo
    public bool vertical;          //Si es verdadero, el enemigo se mueve en el eje Y
    public float changeTime;       //Tiempo para cambiar de dirección
    public ParticleSystem smokeEffect; //Efecto de humo al "romperse" o dañarse el enemigo
    public GameObject childObject; //Objeto hijo (parte del enemigo) que se apagará al impacto

    //Variables privadas
    Rigidbody2D rigidbody2d; //Cuerpo rígido del enemigo
    Animator animator;        //Controlador de animaciones
    float timer;             //Temporizador para cambiar de dirección
    int direction = 1;       //Dirección inicial (1 o -1)
    bool broken = true;      //Indica si está "roto" (activo) o "arreglado"

    //Start se llama al comenzar la ejecución (como encender el motor de un coche)
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        timer = changeTime;

        //Asegurarse de que el hijo del enemigo empiece activo, como encender la luz frontal de un vehículo
        if (childObject != null)
        {
            childObject.SetActive(true);
        }
    }

    //Update se llama cada cuadro (como verificar la brújula en todo momento)
    void Update()
    {
        //Contamos el tiempo para cambiar de dirección
        timer -= Time.deltaTime;

        //Si el temporizador llega a cero, invertimos la dirección
        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    //FixedUpdate se llama a la misma tasa que el sistema de física (como ajustar la dirección del coche según la carretera)
    void FixedUpdate()
    {
        //Si está arreglado, no se mueve ni hace nada
        if (!broken)
        {
            return;
        }

        //Calculamos nueva posición
        Vector2 position = rigidbody2d.position;

        //Si se mueve en vertical, cambiamos Y; si no, cambiamos X
        if (vertical)
        {
            position.y = position.y + speed * direction * Time.deltaTime;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + speed * direction * Time.deltaTime;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }

        //Aplicamos la nueva posición al cuerpo rígido
        rigidbody2d.MovePosition(position);
    }

    //Detectar colisiones (como si el coche choca con otro objeto)
    void OnTriggerEnter2D(Collider2D other)
    {
        //Si colisiona con el jugador, le dañamos
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ChangeHealth(-1);
        }

        //Si el impacto es con un proyectil y este enemigo está etiquetado como "Enemy"
        if (other.CompareTag("Projectil") && CompareTag("Enemy"))
        {
            //Desactivamos el objeto hijo (simil a apagar la luz frontal de un coche)
            if (childObject != null)
            {
                childObject.SetActive(false);
            }
            //Destruimos el proyectil
            Destroy(other.gameObject);
        }
    }

    //Método para "arreglar" al enemigo (como reparar el motor de un coche averiado)
    public void Fix()
    {
        //Ya no está roto
        broken = false;
        //No responde más a la física
        rigidbody2d.simulated = false;
        //Disparamos la animación de estar arreglado
        animator.SetTrigger("Fixed");

        //Si hay efecto de humo, lo detenemos
        if (smokeEffect != null)
        {
            smokeEffect.Stop();
        }
    }
}
