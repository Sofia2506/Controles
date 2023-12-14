using UnityEngine;
using UnityEngine.UI;

public class TriggerScript : MonoBehaviour
{
    public GameObject canvasPlano; // Arrastra el objeto Plano aquí en el Inspector
    private bool canInteract = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
            canvasPlano.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
            canvasPlano.SetActive(false);
        }
    }

    // Puedes manejar la selección del jugador y realizar las acciones necesarias en otro método.
}
