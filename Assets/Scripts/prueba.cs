using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class prueba : MonoBehaviour
{
    public static JSONNode responseData;

    IEnumerator Start()
    {
        string url = "http://localhost:8081/reading-control"; // URL de la API que quieres llamar

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error al llamar a la API: " + webRequest.error);
            }
            else
            {
                string responseText = webRequest.downloadHandler.text;
                responseData = JSON.Parse(responseText);
                Debug.Log("Respuesta de la API convertida a JSON y almacenada en 'responseData'.");

                // Ahora que tienes los datos, puedes acceder a ellos aquí
                JSONNode readingsArray = responseData["readings"];
                if (readingsArray != null && readingsArray.IsArray && readingsArray.Count > 0)
                {
                    JSONNode firstReading = readingsArray[0];
                    Debug.Log("El primer valor de lectura es: " + firstReading["title"]);
                }
                else
                {
                    Debug.LogError("El array 'readings' está vacío o no es un array válido.");
                }
            }
        }
    }

    void Update()
    {
        // Puedes agregar lógica aquí si necesitas algo que se actualice constantemente
    }
}
