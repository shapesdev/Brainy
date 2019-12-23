using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class LevelSelectionFactory
{
    public LevelSelectionController controller { get; private set; }
    public LevelSelectionModel model { get; private set; }
    public LevelSelectionView view { get; private set; }

    private LevelSelectionModel tempModel;
    private LevelDataLoader levelLoader;

    Dictionary<Achievement, int> achievements = new Dictionary<Achievement, int>();

    public void Load(LevelSelectionView view, Achievement[] array)
    {
        levelLoader = new LevelDataLoader();
        this.model = GetModel();
        LoadBadgesInformation(array);
        this.view = view;
        this.controller = new LevelSelectionController(model, view);
    }

    private LevelSelectionModel GetModel()
    {
        if (!PlayerPrefs.HasKey("Levels"))
        {
            model = new LevelSelectionModel(levelLoader.LoadDefaultData());
        }
        else
        {
            DeserializeFromJson();
            CheckUpdatesOnLevels();
            model.UnlockNewLevels();
        }
        model.SetUpCategories();
        SerializeToJson();
        PlayerConfig.instance.totalLevels = model.allLevels.Count;
        return model;
    }

    private void CheckUpdatesOnLevels()
    {
        tempModel = new LevelSelectionModel(levelLoader.LoadDefaultData());

        if (tempModel.allLevels.Count > model.allLevels.Count)
        {
            foreach (var list1 in tempModel.allLevels)
            {
                var instanceId = list1.levelType.GetInstanceID();

                if (model.allLevels.Exists(x => x.levelType.GetInstanceID() == instanceId) == false)
                {
                    model.allLevels.Add(list1);
                }
            }
        }

        if(model.allLevels.Count == tempModel.allLevels.Count)
        {
            for(int i = 0; i < model.allLevels.Count; i++)
            {
                model.allLevels[i].levelType = tempModel.allLevels[i].levelType;
            }
        }
    }

    private void SerializeToJson()
    {
        string jsonString = JsonUtility.ToJson(model);
        PlayerPrefs.SetString("Levels", jsonString);
    }

    private void DeserializeFromJson()
    {
        string jsonString = PlayerPrefs.GetString("Levels");
        model = (LevelSelectionModel)JsonUtility.FromJson(jsonString, typeof(LevelSelectionModel));
    }

    private void LoadBadgesInformation(Achievement[] objects)
    {
        foreach (Achievement obj in objects)
        {
            int temp = 0;
            for (int i = 0; i < model.GetLevels().Count; i++)
            {
                if (model.GetLevels()[i][0].levelType.subjectName == obj.subjectName)
                {
                    for(int j = 0; j < model.GetLevels()[i].Count; j++)
                    {
                        if(model.GetLevels()[i][j].completed == true)
                        {
                            temp += 1;
                        }
                    }
                }
            }
            achievements.Add(obj, temp);
        }
        achievements = achievements.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        PlayerConfig.instance.ShowAchievement(achievements);
    }
}