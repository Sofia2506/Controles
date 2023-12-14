using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class mostrar : MonoBehaviour
{
    public TextMeshProUGUI textoMostrar;
    public TextMeshProUGUI textoMostrar2;
    public TextMeshProUGUI textoMostrar3;
    public TextMeshProUGUI lectura;
    public TextMeshProUGUI texto;

    private void Start()
    {
        // Cargar el archivo JSON desde la carpeta Resources
        TextAsset jsonFile = Resources.Load<TextAsset>("data");

        // Deserializar el JSON en la clase Data
        Data data = JsonUtility.FromJson<Data>(jsonFile.text);

        int cantPreguntas = data.preguntas.Count;
        // Mostrar los datos en pantalla (en este ejemplo, solo se mostrará el primer personaje)
        lectura.text = "Lectura: "+data.titulo;
        texto.text = data.texto;
        if (cantPreguntas > 0)
        {
            Pregunta pregunta1 = data.preguntas[0];
            string opciones="";
            for(int i=0; i<pregunta1.opciones.Count; i++){
                opciones += (i+1)+". "+pregunta1.opciones[i].texto_opcion+"\n";
            }
            textoMostrar.text = "Pregunta 0"+pregunta1.id_pregunta+"\n"+pregunta1.pregunta +"\n"+opciones;

            Pregunta pregunta2 = data.preguntas[1];
            opciones="";
            for(int i=0; i<pregunta2.opciones.Count; i++){
                opciones += (i+1)+". "+pregunta2.opciones[i].texto_opcion+"\n";
            }
            textoMostrar2.text = "Pregunta 0"+pregunta2.id_pregunta+"\n"+pregunta2.pregunta +"\n"+opciones;

            Pregunta pregunta3 = data.preguntas[2];
            opciones="";
            for(int i=0; i<pregunta3.opciones.Count; i++){
                opciones += (i+1)+". "+pregunta3.opciones[i].texto_opcion+"\n";
            }
            textoMostrar3.text = "Pregunta 0"+pregunta3.id_pregunta+"\n"+pregunta3.pregunta +"\n"+opciones;
        }
    }
}