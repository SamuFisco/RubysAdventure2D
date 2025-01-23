using System.Collections;
using UnityEngine;

public class Projectil : MonoBehaviour
{
    //Rigidbody2D del proyectil, como el chasis de un cohete que lo mantiene estable.
    Rigidbody2D rigidbody2d;

    //Awake se llama cuando el GameObject del proyectil se crea (como encender el cohete antes de lanzarlo).
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    //Start se llama antes del primer frame (aquí establecemos la autodestrucción, como un temporizador de autodestrucción en un cohete).
    void Start()
    {
        //Destruir el proyectil después de 1 segundo
        Destroy(gameObject, 1.0f);
    }

    //Update se llama cada cuadro (es como vigilar si el cohete se aleja demasiado de la base).
    void Update()
    {
        //Si el proyectil está fuera de rango, también destruirlo
        if (transform.position.magnitude > 100.0f)
        {
            Destroy(gameObject);
        }
    }

    //Método para "lanzar" el proyectil, como dar impulso inicial a una flecha o cohete.
    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    //OnTriggerEnter2D se llama cuando este proyectil colisiona con un collider (como si impactara contra un objetivo).
    void OnTriggerEnter2D(Collider2D other)
    {
        //Verifica si colisiona con un enemigo
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.Fix(); //Aplica la lógica de "reparación" o daño al enemigo (como reparar o desactivar el objetivo)
        }

        //Destruye el proyectil inmediatamente al colisionar (como el impacto final de un cohete).
        Destroy(gameObject);
    }
}
