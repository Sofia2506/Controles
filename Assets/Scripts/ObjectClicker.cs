using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ObjectClicker : MonoBehaviour
{
    public TextMeshProUGUI objectNameText; // Referencia al texto UI
    public Dictionary<int, int> respuestasUsuario = new Dictionary<int, int>();

    private int preguntaActualId = -1; // ID de la pregunta actual

    void Update()
    {
        DetectarClickRaton();
        DetectarEntradaTeclado();
        if (Input.GetKeyDown(KeyCode.Space)) // Verificar si se presiona la barra espaciadora
        {
            MostrarRespuestas();
        }
    }

    void DetectarClickRaton()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.collider != null)
            {
                for (int i = 1; i <= 8; i++)
                {
                    if (hit.collider.gameObject.name == "Pizarra" + i)
                    {
                        objectNameText.text = "Respondiendo Pregunta " + i;
                        Debug.Log(objectNameText.text);
                        preguntaActualId = i; // Actualiza la pregunta actual
                        break; // Salir del bucle una vez que se encuentra la pizarra correcta
                    }
                }
            }
        }
    }

    void DetectarEntradaTeclado()
    {
        if (preguntaActualId == -1) return; // No hacer nada si no hay una pregunta actual

        if (Input.GetKeyDown(KeyCode.Alpha1)) AlmacenarRespuesta(1);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) AlmacenarRespuesta(2);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) AlmacenarRespuesta(3);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) AlmacenarRespuesta(4);
        else if (Input.GetKeyDown(KeyCode.Alpha5)) AlmacenarRespuesta(5);
    }

    void AlmacenarRespuesta(int respuesta)
    {
        if (respuestasUsuario.ContainsKey(preguntaActualId))
            respuestasUsuario[preguntaActualId] = respuesta;
        else
            respuestasUsuario.Add(preguntaActualId, respuesta);

        // Opcional: Resetear preguntaActualId si deseas que el usuario haga clic en otra pizarra para responder otra pregunta
        preguntaActualId = -1; 
    }

    public void MostrarRespuestas()
    {
        foreach (KeyValuePair<int, int> respuesta in respuestasUsuario)
        {
            Debug.Log("Pregunta ID: " + respuesta.Key + " - Respuesta: " + respuesta.Value);
        }
    }
}
