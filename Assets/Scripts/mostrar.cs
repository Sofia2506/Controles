using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using TMPro;
using System.Collections;
using System.Collections.Generic; 

public class mostrar : MonoBehaviour
{
    public TextMeshProUGUI lectura;
    public TextMeshProUGUI texto;
    public TextMeshProUGUI[] textoMostrar;

    private IEnumerator Start()
    {
        string url = "http://localhost:8081/reading-control";

        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        yield return webRequest.SendWebRequest();

        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error al llamar al endpoint: " + webRequest.error);
        }
        else
        {
            string responseText = webRequest.downloadHandler.text;

            JSONNode jsonResponse = JSON.Parse(responseText);
            JSONNode readingsArray = jsonResponse["readings"];

            if (readingsArray != null && readingsArray.IsArray && readingsArray.Count > 0)
            {
                JSONNode firstReading = readingsArray[0];
                lectura.text = "Lectura: " + firstReading["title"];
                texto.text = firstReading["text"];

                JSONNode questions = firstReading["questions"];

                if (questions != null && questions.IsArray && questions.Count > 0)
                {
                    for (int i = 0; i < Mathf.Min(textoMostrar.Length, questions.Count); i++) 
                    {
                        JSONNode question = questions[i];
                        string preguntaTexto = "Pregunta " + question["question_id"] + "\n" + question["question_text"] + "\n";
                        string opciones = "";

                        JSONNode optionsArray = question["options"];
                        if (optionsArray != null && optionsArray.IsArray)
                        {
                            for (int j = 0; j < optionsArray.Count; j++)
                            {
                                opciones += (j + 1) + ". " + optionsArray[j]["option_text"] + "\n";
                            }
                        }

                        textoMostrar[i].text = preguntaTexto + opciones;
                    }
                }
            }
        }
    }
}
