
using System;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonInteraction : MonoBehaviour
{
    [SerializeField] private MonoBehaviour actionScript;
    
    private GameObject player;
    private PlayerController playerController;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    public void PressButton()
    {
        // Verifica que el attachedScript esté asignado
        if (actionScript != null)
        {
            // Llama al método mainAction() en el script adjunto
            actionScript.Invoke("mainAction", 0f);  // Esto ejecuta mainAction() en el attachedScript
            Debug.Log("Botón presionado y acción ejecutada.");
        }
        else
        {
            Debug.LogWarning("No se ha asignado un script al botón.");
        }
    }
}