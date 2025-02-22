using System;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject dotReticle;  // El retículo por defecto
    [SerializeField] private GameObject pressButtonReticle;  // El retículo cuando está mirando un botón

    private GameObject player;
    private PlayerController playerController;
    
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();

    }

    private void Update()
    {
        ChangeReticle();
    }

    private void ChangeReticle()
    {
        if (playerController._objectObserved)
        {
            if (playerController._objectObserved.CompareTag("Button"))
            {

                // Desactivar el retículo por defecto
                dotReticle.SetActive(false);

                // Activar el retículo para el botón
                pressButtonReticle.SetActive(true);
            }
            else
            {
                dotReticle.SetActive(true);
                pressButtonReticle.SetActive(false);
            }
        }
        
    }
}
