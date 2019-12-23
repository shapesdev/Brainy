using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class LevelSelectionModel {

    public List<LevelModel> allLevels = new List<LevelModel>();
    List<List<LevelModel>> categoriesWithLevels;
    Dictionary<int, Type> types;

    public LevelSelectionModel(List<LevelModel> levels)
    {
        allLevels = levels;
    }

    public void UnlockNewLevels()
    {
        int amount = 0;
        for (int i = 0; i < allLevels.Count; i++)
        {
            if (allLevels[i].completed == true)
            {
                amount += 1;
                if (allLevels.Exists(x => x.levelType.levelNumber == allLevels[i].levelType.levelNumber + 1 && x.levelType.id == allLevels[i].levelType.id) == true)
                {
                    var index2 = allLevels.FindIndex(x => x.levelType.levelNumber == allLevels[i].levelType.levelNumber + 1 && x.levelType.id == allLevels[i].levelType.id);
                    allLevels[index2].unlocked = true;
                }
                else
                {

                }
            }
        }
        PlayerConfig.instance.SetLevelsCompleted(amount);
    }

    public void SetUpCategories()
    {
        types = new Dictionary<int, Type>();
        int count = 0;

        for(int i = 0; i < allLevels.Count; i++)
        {
            var type = allLevels[i].levelType.GetType();

            if (!types.ContainsValue(type))
            {
                types.Add(count, type);
                count++;
            }
        }
        SetUpLevelsForCategories();
    }

    public void SetUpLevelsForCategories()
    {
        categoriesWithLevels = new List<List<LevelModel>>();

        for(int i = 0; i < types.Count; i++)
        {
            List<LevelModel> level = new List<LevelModel>();

            foreach(var item in allLevels)
            {
                if(types[i] == item.levelType.GetType())
                {
                    level.Add(item);
                }
            }
            level = level.OrderBy(x => x.levelType.levelNumber).ToList();
            categoriesWithLevels.Add(level);
        }
    }

    public List<List<LevelModel>> GetLevels()
    {
        return categoriesWithLevels;
    }

    public Dictionary<int, Type> GetCategories()
    {
        return types;
    }
}
