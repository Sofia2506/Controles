using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class prueba : MonoBehaviour
{
    private string responseData;

    IEnumerator CallAPI()
    {
        string url = "http://localhost:8081/reading-control"; // URL de la API que quieres llamar

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest(); // Envía la solicitud y espera la respuesta

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error al llamar a la API: " + webRequest.error);
            }
            else
            {
                Debug.Log("Respuesta de la API: " + webRequest.downloadHandler.text);
                // Procesa los datos de la API aquí (webRequest.downloadHandler.text)
            }
        }
    }
    public string GetResponseData()
    {
        return responseData;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CallAPI());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
