using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZonaPeligro : MonoBehaviour
{
    //Este script define una "zona peligrosa" en la que, si el jugador permanece dentro, perder� salud de manera continua.
    //Podr�amos imaginarlo como un charco de lava: mientras est�s metido, tu salud desciende.

    void OnTriggerStay2D(Collider2D other)
    {
        //Verificamos si el objeto que entra en el trigger es el jugador
        PlayerController controller = other.GetComponent<PlayerController>();

        //Si es el jugador, le reducimos la salud
        if (controller != null)
        {
            controller.ChangeHealth(-1);
        }
    }
}
