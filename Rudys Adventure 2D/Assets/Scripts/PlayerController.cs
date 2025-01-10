using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void Update()
    {
        // 1. Obtener la posición actual del GameObject
        Vector2 position = transform.position;

        // 2. Incrementar la posición en el eje X
        position.x = position.x + 0.1f;

        // 3. Actualizar la posición del GameObject
        transform.position = position;
    }
}
