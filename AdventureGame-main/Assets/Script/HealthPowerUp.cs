using UnityEngine;

public class HealthPowerUp : MonoBehaviour
{
    [SerializeField]
    private int healthRestored = 3; // Cantidad de salud que otorga el Power-Up al jugador

    [SerializeField]
    private AudioClip pickupSound; // Sonido al recoger el Power-Up

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el objeto que colisiona es el jugador
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            // Incrementar la salud del jugador
            player.ChangeHealth(healthRestored);

            // Reproducir sonido si está configurado
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            // Destruir el Power-Up después de recogerlo
            Destroy(gameObject);
        }
    }
}
