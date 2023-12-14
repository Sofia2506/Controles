using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class Opcion
{
    public string texto_opcion;
    public int id_opcion;
}
[System.Serializable]
public class Pregunta
{
    public string pregunta;
    public int id_pregunta;
    public List<Opcion> opciones;
}

[System.Serializable]
public class Data
{
    public List<Pregunta> preguntas;
    public string titulo;
    public string texto;
}