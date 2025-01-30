using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Referencia al menú de opciones que será animado
    public GameObject menuOpciones;

    // Botón de inicio: Carga la escena del juego
    public void BotonInicio()
    {
        SceneManager.LoadScene("MainScene"); // Cambia "MainScene" por el nombre exacto de tu escena
    }

    // Botón de opciones: Anima el menú desplegable de opciones
    public void BotonOpciones()
    {
        // Posición final para desplegar el menú
        Vector3 posicionFinal = new Vector3(64, 29, 0);

        // Comprueba si el menú ya está en su posición final
        if (menuOpciones.transform.localPosition != posicionFinal)
        {
            // Animar el menú desde la posición inicial hasta la posición final
            LeanTween.moveLocal(menuOpciones, posicionFinal, 0.5f).setEase(LeanTweenType.easeInOutQuad);
        }
    }

    // Botón atrás: Devuelve el menú de opciones a su posición inicial
    public void BotonAtras()
    {
        // Posición inicial para ocultar el menú
        Vector3 posicionInicial = new Vector3(64, -491, 0);

        // Animar el menú de vuelta a su posición inicial
        LeanTween.moveLocal(menuOpciones, posicionInicial, 0.5f).setEase(LeanTweenType.easeInOutQuad);
    }

    // Botón de salir: Cierra el juego
    public void BotonSalir()
    {
#if UNITY_EDITOR
        // Si estás en el editor, detiene el modo de juego
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Si estás en una build, cierra la aplicación
        Application.Quit();
#endif
    }
}
