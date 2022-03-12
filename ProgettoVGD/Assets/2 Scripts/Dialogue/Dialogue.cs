using UnityEngine;

// Classe per inserire le frasi dei dialoghi dall'inspector di unity
[System.Serializable]
public class Dialogue
{ 
    [TextArea(3, 10)]
    public string[] sentences;
}
