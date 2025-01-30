using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Necesario para usar Slider e Image

public class Volumen : MonoBehaviour
{
    public Slider slider; // Control deslizante para el volumen
    public float sliderValue; // Valor actual del slider
    public Image imagenMute; // Icono de mute

    void Start()
    {
        // Carga el valor del volumen guardado o establece un valor predeterminado
        slider.value = PlayerPrefs.GetFloat("VolumenAudio", 0.5f);
        AudioListener.volume = slider.value; // Ajusta el volumen del AudioListener
        RevisarSiEstoyMute(); // Revisa si debe mostrar el icono de mute
    }

    // Cambia el volumen cuando el slider se ajusta
    public void ChangeSlider(float valor)
    {
        sliderValue = valor; // Actualiza el valor del slider
        PlayerPrefs.SetFloat("VolumenAudio", sliderValue); // Guarda el valor del volumen
        AudioListener.volume = slider.value; // Ajusta el volumen global
        RevisarSiEstoyMute(); // Revisa si debe mostrar el icono de mute
    }

    // Revisa si el volumen está en cero y muestra/oculta el icono de mute
    public void RevisarSiEstoyMute()
    {
        if (sliderValue == 0)
        {
            imagenMute.enabled = true; // Muestra el icono de mute
        }
        else
        {
            imagenMute.enabled = false; // Oculta el icono de mute
        }
    }
}
