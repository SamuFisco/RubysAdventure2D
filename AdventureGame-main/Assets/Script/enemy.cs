using UnityEngine;

//La clase 'enemy' representa a cada enemigo en el juego. Controla un contador global que registra cu�ntos existen en total.
public class enemy : MonoBehaviour
{
    //Variable est�tica que act�a como un "marcador" global, similar a un tablero de puntuaci�n donde anotas cu�ntos enemigos hay.
    public static int enemyCount = 0;
    //Contador de enemigos activos

    //Start se llama cuando el objeto se inicializa, como arrancar el motor de un nuevo coche en la carretera de los enemigos.
    void Start()
    {
        enemyCount++;
        //Incrementamos el contador, imaginando que a�adimos un coche m�s a esa carretera de enemigos.
    }

    //OnDestroy se llama cuando el objeto se destruye, similar a retirar ese coche de la carretera.
    void OnDestroy()
    {
        enemyCount--;
        //Reducimos el contador, es como sacar un coche del tr�fico y dejar la v�a m�s despejada.
    }
}