using UnityEngine;

public class enemy : MonoBehaviour
{
    public static int enemyCount = 0;
    //Contador de enemigos activos

    void Start()
    {
        enemyCount++;
    }

    void OnDestroy()
    {
        enemyCount--;
    }
}