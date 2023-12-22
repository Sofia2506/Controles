using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using SimpleJSON;
using UnityEngine.Networking;
public class ObjectClicker : MonoBehaviour
{
    private string urlGET = "http://localhost:8081/reading-control";
    // private string urlPOST = "http://localhost:8081/reading-control/6584b297030b50f753d15a61/calculate_score";
    public TextMeshProUGUI objectNameText; // Referencia al texto UI
    public Dictionary<int, int> respuestasUsuario = new Dictionary<int, int>();

    private int preguntaActualId = -1; // ID de la pregunta actual

    private JSONNode jsonData;

    void Start()
    {
        objectNameText.text = "Haz clic en una pizarra para responder una pregunta";
    }


    void Update()
    {
        DetectarClickRaton();
        DetectarEntradaTeclado();
        if (Input.GetKeyDown(KeyCode.Space)) // Verificar si se presiona la barra espaciadora
        {
            MostrarRespuestas();
        }
        if (Input.GetKeyDown(KeyCode.Return)) // Verificar si se presiona la tecla Enter
        {
            StartCoroutine(ObtenerRespuestasDesdeEndpoint());
            Debug.Log("JSON obtenido: " + jsonData.ToString());
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
                        objectNameText.text = "Pregunta " + i;
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


    private JSONNode FindOptionById(JSONNode question, int optionId)
    {
        if (question != null && question["options"] != null)
        {
            foreach (JSONNode option in question["options"])
            {
                if (option["option_id"].AsInt == optionId)
                {
                    return option;
                }
            }
        }
        return null;
    }

 private JSONNode FindReadingByQuestionId(JSONNode jsonData, int questionId)
    {
        Debug.Log("Buscando lectura para la pregunta ID: " + questionId);
        Debug.Log("data: " + jsonData.ToString());
        if (jsonData != null && jsonData["readings"] != null)
        {
            foreach (JSONNode reading in jsonData["readings"])
            {
                if (reading["questions"] != null && reading["questions"].Count > 0)
                {
                    foreach (JSONNode question in reading["questions"])
                    {
                        if (question["question_id"].AsInt == questionId)
                        {
                            Debug.Log("Lectura encontrada para la pregunta ID: " + questionId);
                            Debug.Log("data: " + reading.ToString());
                            return reading;
                        }
                    }
                }
            }
        }
        Debug.Log("No se encontró la lectura para la pregunta ID: " + questionId);
        return null;
    }

    private JSONNode FindQuestionById(JSONNode reading, int questionId)
    {
        if (reading != null && reading["questions"] != null)
        {
            foreach (JSONNode question in reading["questions"])
            {
                if (question["question_id"].AsInt == questionId)
                {
                    return question;
                }
            }
        }
        return null;
    }

    private IEnumerator ObtenerRespuestasDesdeEndpoint()
    {
        // Crear una solicitud para obtener el JSON desde el endpoint
        UnityWebRequest request = UnityWebRequest.Get(urlGET);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || 
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error al obtener el JSON: " + request.error);
        }
        else
        {
            // Procesar el JSON obtenido
            jsonData = JSON.Parse(request.downloadHandler.text);
            
            Debug.Log("JSON obtenido: " + jsonData.ToString());

            string id = string.Empty;

            // Verificar si la respuesta inicial contiene el campo _id
            if (jsonData != null && jsonData["readings"] != null && jsonData["readings"].Count > 0)
            {
                JSONNode firstReading = jsonData["readings"][0];
                id = firstReading["_id"];
                Debug.Log("ID de la lectura: " + id);
            }

            // Aquí puedes hacer coincidir las respuestas del usuario con el texto de las opciones
            Dictionary<int, string> respuestasTextoOpciones = new Dictionary<int, string>();

            foreach (KeyValuePair<int, int> respuesta in respuestasUsuario)
            {
                int preguntaId = respuesta.Key;
                int opcionSeleccionada = respuesta.Value;

                if (jsonData != null) 
                {
                    Debug.Log("JSON DATA NO ES NULO");
                } else {
                    Debug.Log("JSON DATA ES NULO" + jsonData.ToString());
                }

                // Encontrar la lectura, pregunta y opción correspondientes en el JSON
                JSONNode lecturaActual = FindReadingByQuestionId(jsonData, preguntaId);   

                Debug.Log("Lectura actual: " + lecturaActual.ToString() + " - Pregunta ID: " + preguntaId);       

                if (lecturaActual != null)
                {
                    JSONNode pregunta = FindQuestionById(lecturaActual, preguntaId);

                    if (pregunta != null)
                    {
                        JSONNode opcion = FindOptionById(pregunta, opcionSeleccionada);

                        if (opcion != null)
                        {
                            string optionText = opcion["option_text"];
                            respuestasTextoOpciones.Add(preguntaId, optionText);
                            Debug.Log("Pregunta ID: " + preguntaId + " - Respuesta: " + optionText);
                        }
                    }
                } else {
                    Debug.LogError("No se encontró la lectura para la pregunta ID: " + preguntaId);
                }
            }

        List<Dictionary<string, object>> answersList = new List<Dictionary<string, object>>();

        Debug.Log("Respuestas del usuario: " + respuestasTextoOpciones.Count);

        // Crear un array JSON para las respuestas
        JSONArray answersArray = new JSONArray();

        // Supongamos que aquí llenas answersArray con los datos necesarios
        foreach (KeyValuePair<int, string> respuesta in respuestasTextoOpciones)
        {
            JSONObject answerObject = new JSONObject();
            answerObject["question_id"] = respuesta.Key;
            answerObject["answer_text"] = respuesta.Value;
            answersArray.Add(answerObject);
        }

        JSONObject requestBody = new JSONObject();
        requestBody["answers"] = answersArray;

        // Crear la solicitud POST
        using (UnityWebRequest requestPOST = new UnityWebRequest(urlGET + "/" + id + "/calculate_score", "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestBody.ToString());
            requestPOST.uploadHandler = new UploadHandlerRaw(bodyRaw);
            requestPOST.downloadHandler = new DownloadHandlerBuffer();
            requestPOST.SetRequestHeader("Content-Type", "application/json");

            yield return requestPOST.SendWebRequest();

            if (requestPOST.result == UnityWebRequest.Result.ConnectionError ||
                requestPOST.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error en la solicitud POST: " + requestPOST.error);
            }
            else
            {
                Debug.Log("Solicitud POST exitosa!");
                Debug.Log("Cuerpo de la solicitud: " + requestBody.ToString());
                Debug.Log("Respuesta del servidor: " + requestPOST.downloadHandler.text);
                JSONNode response = JSON.Parse(requestPOST.downloadHandler.text);

                int score = response["score"].AsInt;
                int correctQuestions = response["correctQuestions"].AsInt;
                int incorrectQuestions = response["incorrectQuestions"].AsInt;
                int totalQuestions = response["totalQuestions"].AsInt;

                string resultText = $"Score: {score}\n" +
                                    $"Preguntas correctas: {correctQuestions}\n" +
                                    $"Preguntas incorrectas: {incorrectQuestions}\n" +
                                    $"Total de preguntas: {totalQuestions}";

                // Actualizar el objeto de texto con la información recibida
                objectNameText.text = resultText;
            }
        }
        }
    }
}
