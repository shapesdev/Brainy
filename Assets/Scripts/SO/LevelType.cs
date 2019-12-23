using UnityEngine;

public class LevelType : ScriptableObject
{
    public int id;

    public string subjectName;

    public int levelNumber;

    public string levelName;

    public string winMotivation;

    public Sprite levelImage;

    public Question[] questions;

    public int maxScore;

    public int minScore;
}
