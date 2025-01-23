using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaludPowerUp : MonoBehaviour
{
    //Clip de audio que se reproducirá al recoger el power-up (como el sonido de abrir una botella de medicina)
    public AudioClip pickupSound;

    //Al chocar con el jugador, este "power-up" restaurará su salud, como una medicina que te sube la energía.
    void OnTriggerEnter2D(Collider2D other)
    {
        //Verificamos si el objeto que entra en el trigger es el jugador
        PlayerController controller = other.GetComponent<PlayerController>();

        //Si es el jugador y no tiene la salud al máximo, le sumamos salud y también reproducimos un sonido
        if (controller != null && controller.health < controller.maxHealth)
        {
            //Recupera 1 punto de salud
            controller.ChangeHealth(1);

            //Reproducir el sonido en el AudioSource del jugador
            //De esta forma, aunque este objeto se destruya, el sonido no se corta.
            if (pickupSound != null)
            {
                controller.PlaySound(pickupSound);
            }

            //Destruir el power-up tras recogerlo
            Destroy(gameObject);
        }
    }
}
