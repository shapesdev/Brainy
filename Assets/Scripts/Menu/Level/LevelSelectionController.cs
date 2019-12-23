using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectionController {

    public LevelSelectionModel model;
    public LevelSelectionView view;
    public LevelSelectionController(LevelSelectionModel model, LevelSelectionView view)
    {
        this.model = model;
        this.view = view;

        AddFavoriteSubject();
        PrepareLevels();
        view.OnValueUpdate += HandleOnValueUpdate;
        MenuUIManager.allActions["Home"] += HandleOnHome;
    }

    private void HandleOnHome(bool obj)
    {
        if(obj == true)
        {
            PrepareLevels();
        }
        else
        {
            view.TurnOffPanel();
        }
    }

    private void AddFavoriteSubject()
    {
        string subject = "Unknown";
        int completedCount = 0;
        int points = 0;

        foreach (var item in model.GetLevels())
        {
            int temp = 0;
            int tempPoints = 0;
            foreach (var itemas in item)
            {
                if(itemas.completed == true)
                {
                    temp += 1;
                    tempPoints += itemas.currentScore;
                }
            }
            if(temp > completedCount && tempPoints > points)
            {
                completedCount = temp;
                points = tempPoints;
                subject = item[0].levelType.subjectName;
            }
        }
        PlayerConfig.instance.favSubject = subject;
    }

    private void PrepareLevels()
    {
        view.DisplayLevelSelections(model.GetLevels()[PlayerPrefs.GetInt("CurrentSubject")]);
        view.PopulateDropdown(model.GetCategories());
        LevelConfig.instance.model = model;
    }

    private void HandleOnValueUpdate(object sender, OnValueUpdateEventArgs e)
    {
        PlayerPrefs.SetInt("CurrentSubject", e.id);
        view.DisplayLevelSelections(model.GetLevels()[e.id]);
    }
}
