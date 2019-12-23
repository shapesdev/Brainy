using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class LevelModel {

    public LevelType levelType;

    public bool unlocked;

    public bool completed;

    public int currentScore;

    public LevelModel(LevelType level, bool unlock, bool complete, int score)
    {
        levelType = level;
        unlocked = unlock;
        completed = complete;
        currentScore = score;
    }
}
