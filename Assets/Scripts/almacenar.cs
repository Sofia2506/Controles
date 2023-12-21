using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class Opcion
{
    public string option_text;
    public int option_id;
}
[System.Serializable]
public class Pregunta
{
    public string question_text;
    public int question_id;
    public List<Opcion> options;
}

[System.Serializable]
public class Data
{
    public List<Pregunta> questions;
    public string title;
    public string text;
}