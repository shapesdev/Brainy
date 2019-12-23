using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataLoader {

    List<LevelType> list = new List<LevelType>();

    private void PopulateList()
    {
        LevelType[] objects = Resources.LoadAll<LevelType>("SO/Levels");
        foreach (LevelType obj in objects)
        {
            list.Add(obj);
        }
    }

    public List<LevelModel> LoadDefaultData()
    {
        List<LevelModel> levels = new List<LevelModel>();
        PopulateList();

        foreach (var obj in list)
        {
            if(obj.levelNumber == 1)
            {
                levels.Add(new LevelModel(obj, true, false, 0));
            }
            else
            {
                levels.Add(new LevelModel(obj, false, false, 0));
            }
        }
        return levels;
    }
}
