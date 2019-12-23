using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question : ScriptableObject
{
    public string question;
    public string correctAnswer;

    public List<string> options = new List<string>();

    public List<Sprite> imageOptions = new List<Sprite>();

    public Sprite picture;
}
