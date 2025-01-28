using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public int healthRestored = 5; // Cantidad de vida que restaura
    public AudioClip pickupSound; // Sonido al recoger el Power-Up

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            // Restaurar salud al jugador
            player.ChangeHealth(healthRestored);

            // Reproducir sonido de recolección si está configurado
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            // Destruir el Power-Up
            Destroy(gameObject);
        }
    }
}
