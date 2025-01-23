using UnityEngine;
using UnityEngine.UIElements;

public class BarraDeVida : MonoBehaviour
{
    //La barra de vida es como el indicador de combustible de un coche: muestra cuánto "combustible" (salud) te queda.
    private VisualElement m_Healthbar;

    //Esta propiedad estática es como un puesto de referencia global: nos permite acceder a la barra de vida desde cualquier parte.
    public static BarraDeVida instance { get; private set; }

    //Awake se llama cuando el script se está cargando, como encender la llave en el coche antes de arrancar.
    private void Awake()
    {
        instance = this; //Almacenamos nuestra referencia "singleton", como dejar las llaves en un sitio específico para encontrarlas siempre.
    }

    //Start se llama antes de la primera actualización del cuadro, como revisar el panel de control antes de comenzar el viaje.
    void Start()
    {
        //Obtenemos el UIDocument, como consultar el manual de instrucciones de nuestro coche para saber dónde está todo.
        UIDocument uiDocument = GetComponent<UIDocument>();

        //Buscamos el elemento "HealthBar" en la interfaz, como encontrar el indicador de combustible en el tablero.
        m_Healthbar = uiDocument.rootVisualElement.Q<VisualElement>("HealthBar");

        //Inicializamos la barra de vida al 100%, como llenar el depósito de combustible completamente.
        SetHealthValue(1.0f);
    }

    //SetHealthValue actualiza el ancho de la barra de vida, como mover la aguja del medidor de gasolina según la cantidad disponible.
    public void SetHealthValue(float percentage)
    {
        //Ajustamos el ancho en un porcentaje, como indicar la proporción de "combustible" que queda.
        m_Healthbar.style.width = Length.Percent(100 * percentage);
    }
}