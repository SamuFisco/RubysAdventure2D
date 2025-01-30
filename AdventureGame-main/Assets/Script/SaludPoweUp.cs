using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaludPowerUp : MonoBehaviour
{
    //Al chocar con el jugador, este "power-up" restaurará su salud.
    void OnTriggerEnter2D(Collider2D other)
    {
        //Verificamos si el objeto que entra en el trigger es el jugador
        PlayerController controller = other.GetComponent<PlayerController>();

        //Si es el jugador y no tiene la salud al máximo, le sumamos salud
        if (controller != null && controller.health < controller.maxHealth)
        {
            //Recupera 1 punto de salud
            controller.ChangeHealth(1);

            //Destruir el power-up tras recogerlo
            Destroy(gameObject);
        }
    }
}
