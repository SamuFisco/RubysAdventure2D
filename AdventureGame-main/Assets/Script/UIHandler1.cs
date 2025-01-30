using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIHandler1 : MonoBehaviour
{
    private VisualElement m_Healthbar;
    public static UIHandler1 instance { get; private set; }

    public float displayTime = 4.0f;
    private VisualElement m_NonPlayerDialogue;
    private Label npcDialogueText;
    private float m_TimerDisplay;

    private bool firstTimeNPC1 = true;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Obtener el UI Document correctamente
        UIDocument uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("UIHandler1: No se encontró UIDocument en este GameObject.");
            return;
        }

        // Inicializar la barra de salud
        m_Healthbar = uiDocument.rootVisualElement.Q<VisualElement>("HealthBar");
        if (m_Healthbar == null)
        {
            Debug.LogError("UIHandler1: No se encontró 'HealthBar' en el UI Document.");
        }
        else
        {
            SetHealthValue(1.0f);
        }

        // Obtener el NPCDialogue correctamente
        m_NonPlayerDialogue = uiDocument.rootVisualElement.Q<VisualElement>("NPCDialogue");
        if (m_NonPlayerDialogue == null)
        {
            Debug.LogError("UIHandler1: No se encontró 'NPCDialogue' en la UI.");
            return;
        }

        // Obtener el Label que mostrará el texto del NPC
        npcDialogueText = m_NonPlayerDialogue.Q<Label>("DialogueText");
        if (npcDialogueText == null)
        {
            Debug.LogError("UIHandler1: 'DialogueText' no se encontró dentro de 'NPCDialogue'. Asegúrate de agregarlo en UI Builder.");
            return;
        }

        // Asegurar que el diálogo inicie oculto
        m_NonPlayerDialogue.style.display = DisplayStyle.None;
        m_TimerDisplay = -1.0f;
    }

    private void Update()
    {
        if (m_TimerDisplay > 0)
        {
            m_TimerDisplay -= Time.deltaTime;
            if (m_TimerDisplay <= 0)
            {
                m_NonPlayerDialogue.style.display = DisplayStyle.None;
            }
        }
    }

    public void SetHealthValue(float percentage)
    {
        if (m_Healthbar != null)
        {
            m_Healthbar.style.width = Length.Percent(100 * percentage);
        }
        else
        {
            Debug.LogError("UIHandler1: No se puede actualizar la barra de salud porque es NULL.");
        }
    }

    public void DisplayDialogueNPC2()
    {
        Debug.Log("NPC2 ACTIVADO - Mostrando su diálogo");

        if (m_NonPlayerDialogue == null || npcDialogueText == null)
        {
            Debug.LogError("UIHandler1: No se puede mostrar el diálogo de NPC1 porque 'NPCDialogue' o 'DialogueText' es NULL.");
            return;
        }

        m_NonPlayerDialogue.style.display = DisplayStyle.Flex;
        npcDialogueText.text = firstTimeNPC1
            ? "¡Hola, Ruby! He oído que hay muchas máquinas descompuestas por aquí. Sería genial si pudieras\r\nayudarnos a repararlas. Usa tu tuerca mágica para hacerlo, y no olvides presionar la tecla \"C\" \r\npara lanzarla. ¡Buena suerte, arreglarlas tiene premio!"
            : "Recuerda que cuentas con 5 vidas, si las pierdes vuelves al inicio";

        firstTimeNPC1 = false;
        m_TimerDisplay = displayTime;
    }

    public void DisplayDialogueNPC1()
    {
        Debug.Log("NPC1 ACTIVADO - Mostrando su diálogo");

        if (m_NonPlayerDialogue == null || npcDialogueText == null)
        {
            Debug.LogError("UIHandler1: No se puede mostrar el diálogo de NPC2 porque 'NPCDialogue' o 'DialogueText' es NULL.");
            return;
        }

        m_NonPlayerDialogue.style.display = DisplayStyle.Flex;
        npcDialogueText.text = "La recompensa de arreglar las máquinas no desaparece, recuerdalo.";
        m_TimerDisplay = displayTime;
    }
}
