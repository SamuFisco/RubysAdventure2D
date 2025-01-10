using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void Update()
    {
        // 1. Obtener la posici�n actual del GameObject
        Vector2 position = transform.position;

        // 2. Incrementar la posici�n en el eje X
        position.x = position.x + 0.1f;

        // 3. Actualizar la posici�n del GameObject
        transform.position = position;
    }
}
