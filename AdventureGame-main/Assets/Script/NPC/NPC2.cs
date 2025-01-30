using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC2 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("NPC2 detectó al jugador - Mostrando diálogo");
            UIHandler1.instance.DisplayDialogueNPC2();
        }
    }
}
