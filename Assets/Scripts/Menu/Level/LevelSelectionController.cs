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
        string tempSubject = "";
        float currrentHighest = 0;
        var categories = model.GetCategories();

        foreach(var item in categories)
        {
            int totalPoints = 0;
            int levelCount = 0;

            for(int i = 0; i < model.allLevels.Count; i++)
            {
                if (model.allLevels[i].levelType.GetType() == item.Value)
                {
                    tempSubject = model.allLevels[i].levelType.subjectName;
                    levelCount++;
                    totalPoints += model.allLevels[i].currentScore;
                }
            }

            float check = (totalPoints * levelCount) / 100;
            if(check > currrentHighest)
            {
                currrentHighest = check;
                subject = tempSubject;
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
