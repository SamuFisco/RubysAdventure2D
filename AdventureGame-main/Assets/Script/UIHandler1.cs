using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIHandler1 : MonoBehaviour
{
    //m_Healthbar es como el indicador de combustible: muestra la "vida" disponible
    private VisualElement m_Healthbar;

    //Una referencia estática (instance) similar a un punto de encuentro global: todos pueden llamarla
    public static UIHandler1 instance { get; private set; }

    //Variables para la ventana de diálogo
    //El displayTime es como el tiempo que estará encendida una luz de alerta en el tablero
    public float displayTime = 4.0f;
    private VisualElement m_NonPlayerDialogue; //La ventana de diálogo, como un aviso emergente
    private float m_TimerDisplay;             //Contador para cerrar el aviso tras unos segundos

    //Awake se llama al cargar el script, como poner un marcador para identificar que este objeto es el principal
    private void Awake()
    {
        instance = this;
    }

    //Start se llama antes del primer frame, como encender la consola central y preparar los indicadores
    private void Start()
    {
        //Obtenemos el UIDocument para interactuar con la interfaz
        UIDocument uiDocument = GetComponent<UIDocument>();

        //Buscamos el elemento "HealthBar" en la UI, como encontrar la barra de combustible en un panel
        m_Healthbar = uiDocument.rootVisualElement.Q<VisualElement>("HealthBar");
        SetHealthValue(1.0f); //Inicializamos la barra al 100%

        //Buscamos la ventana de diálogo (NPCDialogue), como un aviso que se muestra si necesitas un mensaje emergente
        m_NonPlayerDialogue = uiDocument.rootVisualElement.Q<VisualElement>("NPCDialogue");
        m_NonPlayerDialogue.style.display = DisplayStyle.None; //Está oculta inicialmente
        m_TimerDisplay = -1.0f; //Usamos valor negativo para indicar que no está mostrando diálogo
    }

    //Update se llama en cada frame, como revisar continuamente si hay que ocultar la ventana
    private void Update()
    {
        //Si el temporizador está activo (mayor que 0), reducimos el tiempo
        if (m_TimerDisplay > 0)
        {
            m_TimerDisplay -= Time.deltaTime;
            //Si se acaba el tiempo, ocultamos la ventana
            if (m_TimerDisplay < 0)
            {
                m_NonPlayerDialogue.style.display = DisplayStyle.None;
            }
        }
    }

    //Actualizamos el ancho de la barra de salud, es como mover la aguja del indicador de gasolina en el tablero
    public void SetHealthValue(float percentage)
    {
        m_Healthbar.style.width = Length.Percent(100 * percentage);
    }

    //Muestra el diálogo del NPC por un tiempo limitado, como encender una luz de alerta por unos segundos
    public void DisplayDialogue()
    {
        m_NonPlayerDialogue.style.display = DisplayStyle.Flex; //Hacer visible la ventana
        m_TimerDisplay = displayTime; //Reiniciar el contador de tiempo
    }
}
