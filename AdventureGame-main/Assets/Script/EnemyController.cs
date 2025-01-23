using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //Variables p�blicas
    public float speed;            //Velocidad de movimiento del enemigo
    public bool vertical;          //Si es verdadero, el enemigo se mueve en el eje Y
    public float changeTime;       //Tiempo para cambiar de direcci�n
    public ParticleSystem smokeEffect; //Efecto de humo al "romperse" o da�arse el enemigo
    public GameObject childObject; //Objeto hijo (parte del enemigo) que se apagar� al impacto

    //Variables privadas
    Rigidbody2D rigidbody2d; //Cuerpo r�gido del enemigo
    Animator animator;        //Controlador de animaciones
    float timer;             //Temporizador para cambiar de direcci�n
    int direction = 1;       //Direcci�n inicial (1 o -1)
    bool broken = true;      //Indica si est� "roto" (activo) o "arreglado"

    //Start se llama al comenzar la ejecuci�n (como encender el motor de un coche)
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        timer = changeTime;

        //Asegurarse de que el hijo del enemigo empiece activo, como encender la luz frontal de un veh�culo
        if (childObject != null)
        {
            childObject.SetActive(true);
        }
    }

    //Update se llama cada cuadro (como verificar la br�jula en todo momento)
    void Update()
    {
        //Contamos el tiempo para cambiar de direcci�n
        timer -= Time.deltaTime;

        //Si el temporizador llega a cero, invertimos la direcci�n
        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    //FixedUpdate se llama a la misma tasa que el sistema de f�sica (como ajustar la direcci�n del coche seg�n la carretera)
    void FixedUpdate()
    {
        //Si est� arreglado, no se mueve ni hace nada
        if (!broken)
        {
            return;
        }

        //Calculamos nueva posici�n
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

        //Aplicamos la nueva posici�n al cuerpo r�gido
        rigidbody2d.MovePosition(position);
    }

    //Detectar colisiones (como si el coche choca con otro objeto)
    void OnTriggerEnter2D(Collider2D other)
    {
        //Si colisiona con el jugador, le da�amos
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ChangeHealth(-1);
        }

        //Si el impacto es con un proyectil y este enemigo est� etiquetado como "Enemy"
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

    //M�todo para "arreglar" al enemigo (como reparar el motor de un coche averiado)
    public void Fix()
    {
        //Ya no est� roto
        broken = false;
        //No responde m�s a la f�sica
        rigidbody2d.simulated = false;
        //Disparamos la animaci�n de estar arreglado
        animator.SetTrigger("Fixed");

        //Si hay efecto de humo, lo detenemos
        if (smokeEffect != null)
        {
            smokeEffect.Stop();
        }
    }
}
