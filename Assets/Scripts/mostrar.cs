using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;
using System.Collections;

public class mostrar : MonoBehaviour
{
    public TextMeshProUGUI lectura;
    public TextMeshProUGUI texto;
    public TextMeshProUGUI textoMostrar;
    public TextMeshProUGUI textoMostrar2;
    public TextMeshProUGUI textoMostrar3;

    private IEnumerator Start()
    {
        string url = "http://localhost:8081/reading-control"; // Reemplaza "URL_DEL_ENDPOINT" con tu URL

        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error al llamar al endpoint: " + webRequest.error);
        }
        else
        {
            string responseText = webRequest.downloadHandler.text;

            // Parsear la respuesta JSON utilizando SimpleJSON
            JSONNode jsonResponse = JSON.Parse(responseText);

            // Mostrar los datos obtenidos en pantalla
            JSONNode readingsArray = jsonResponse["readings"];
            if (readingsArray != null && readingsArray.IsArray && readingsArray.Count > 0)
            {
                JSONNode firstReading = readingsArray[0];
                lectura.text = "Lectura: " + firstReading["title"];
                texto.text = firstReading["text"];

                JSONNode questions = firstReading["questions"];
                if (questions != null && questions.IsArray && questions.Count > 0)
                {
                    for (int i = 0; i < Mathf.Min(3, questions.Count); i++) // Mostrar solo las primeras 3 preguntas
                    {
                        JSONNode question = questions[i];
                        string opciones = "";
                        JSONNode optionsArray = question["options"];
                        if (optionsArray != null && optionsArray.IsArray)
                        {
                            for (int j = 0; j < optionsArray.Count; j++)
                            {
                                opciones += (j + 1) + ". " + optionsArray[j]["option_text"] + "\n";
                            }
                        }
                        switch (i)
                        {
                            case 0:
                                textoMostrar.text = "Pregunta 0" + question["question_id"] + "\n" + question["question_text"] + "\n" + opciones;
                                break;
                            case 1:
                                textoMostrar2.text = "Pregunta 0" + question["question_id"] + "\n" + question["question_text"] + "\n" + opciones;
                                break;
                            case 2:
                                textoMostrar3.text = "Pregunta 0" + question["question_id"] + "\n" + question["question_text"] + "\n" + opciones;
                                break;
                        }
                    }
                }
            }
        }
    }
}
