using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiviCollisionEffect : MonoBehaviour
{
    public AudioClip kiviSound; // Sonido al colisionar
    public Sprite destroyedSprite; // Sprite que se mostrará antes de destruirse
    public float destroyDelay = 2.0f; // Tiempo antes de eliminar el objeto
    public ParticleSystem destructionParticles; // 🔥 Prefab de partículas de destrucción

    private SpriteRenderer spriteRenderer;
    private Collider2D collider2D;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();

        if (spriteRenderer == null)
        {
            Debug.LogError("KiviCollisionEffect: No se encontró SpriteRenderer en " + gameObject.name);
        }
        if (collider2D == null)
        {
            Debug.LogError("KiviCollisionEffect: No se encontró Collider2D en " + gameObject.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            // 🔥 Verificar si el jugador tiene menos de la vida máxima antes de recoger el Kivi
            if (player != null && player.health < player.maxHealth)
            {
                player.ChangeHealth(1); // 🏥 Recupera 1 punto de vida

                if (kiviSound != null)
                {
                    AudioSource.PlayClipAtPoint(kiviSound, Camera.main.transform.position, 1.5f);
                }

                if (destroyedSprite != null)
                {
                    spriteRenderer.sprite = destroyedSprite; // Cambia al sprite de destrucción
                }

                if (collider2D != null)
                {
                    collider2D.enabled = false; // Desactiva el collider para evitar colisiones repetidas
                }

                StartCoroutine(DestroyWithParticles()); // 🔥 Inicia la animación de partículas antes de destruir
            }
            else
            {
                Debug.Log("⛔ Vida completa, Kivi no se puede recoger.");
            }
        }
    }

    IEnumerator DestroyWithParticles()
    {
        if (destructionParticles != null)
        {
            // 🔥 Instanciar las partículas en la posición de Kivi
            ParticleSystem particles = Instantiate(destructionParticles, transform.position, Quaternion.identity);
            particles.Play(); // Activar las partículas
            Destroy(particles.gameObject, particles.main.duration); // Destruir las partículas después de su duración
        }

        yield return new WaitForSeconds(destroyDelay); // Esperar antes de eliminar el objeto
        Destroy(gameObject); // 🔥 Destruir Kivi después del efecto de partículas
    }
}
