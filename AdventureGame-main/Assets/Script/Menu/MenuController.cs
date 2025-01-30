using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Referencia al men� de opciones que ser� animado
    public GameObject menuOpciones;

    // Bot�n de inicio: Carga la escena del juego
    public void BotonInicio()
    {
        SceneManager.LoadScene("MainScene"); // Cambia "MainScene" por el nombre exacto de tu escena
    }

    // Bot�n de opciones: Anima el men� desplegable de opciones
    public void BotonOpciones()
    {
        // Posici�n final para desplegar el men�
        Vector3 posicionFinal = new Vector3(64, 29, 0);

        // Comprueba si el men� ya est� en su posici�n final
        if (menuOpciones.transform.localPosition != posicionFinal)
        {
            // Animar el men� desde la posici�n inicial hasta la posici�n final
            LeanTween.moveLocal(menuOpciones, posicionFinal, 0.5f).setEase(LeanTweenType.easeInOutQuad);
        }
    }

    // Bot�n atr�s: Devuelve el men� de opciones a su posici�n inicial
    public void BotonAtras()
    {
        // Posici�n inicial para ocultar el men�
        Vector3 posicionInicial = new Vector3(64, -491, 0);

        // Animar el men� de vuelta a su posici�n inicial
        LeanTween.moveLocal(menuOpciones, posicionInicial, 0.5f).setEase(LeanTweenType.easeInOutQuad);
    }

    // Bot�n de salir: Cierra el juego
    public void BotonSalir()
    {
#if UNITY_EDITOR
        // Si est�s en el editor, detiene el modo de juego
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Si est�s en una build, cierra la aplicaci�n
        Application.Quit();
#endif
    }
}
