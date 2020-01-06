using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController {

    public GameModel model;
    public GameView view;

    public GameController(GameModel model, GameView view)
    {
        this.model = model;
        this.view = view;

        view.DisplayLevel(this);
        ShowQuestion(model.GetQuestionNr());
    }

    public void RoundFinish()
    {
        model.IncreaseQuestionNr();
        ShowQuestion(model.GetQuestionNr());
    }

    public void CheckAnswer(string option)
    {
        if (option == model.GetlAllQuestions()[model.GetQuestionNr()].correctAnswer)
        {
            model.AddScore();
            view.Win();
        }
        else
        {
            view.Lose();
        }
    }

    public void CheckAnswer(Sprite sprite)
    {
        if (sprite == model.GetlAllQuestions()[model.GetQuestionNr()].picture)
        {
            model.AddScore();
            view.Win();
        }
        else
        {
            view.Lose();
        }
    }

    private void ShowQuestion(int questionNr)
    {
        if(questionNr > 0)
        {
            view.TurnOffInformation();
        }

        if(questionNr < model.GetlAllQuestions().Length)
        {
            view.ShowQuestion(model.GetlAllQuestions()[questionNr]);
        }
        else
        {
            if(model.GetScore() > LevelConfig.instance.levels[LevelConfig.instance.currentlevel].levelType.minScore)
            {
                LevelConfig.instance.levels[LevelConfig.instance.currentlevel].completed = true;

                if (LevelConfig.instance.levels[LevelConfig.instance.currentlevel].currentScore < model.GetScore())
                {
                    var diff = model.GetScore() - LevelConfig.instance.levels[LevelConfig.instance.currentlevel].currentScore;
                    PlayerPrefs.SetInt("IqPoints", PlayerPrefs.GetInt("IqPoints") + diff);
                    LevelConfig.instance.levels[LevelConfig.instance.currentlevel].currentScore = model.GetScore();
                }
                view.ShowWinPanel(model.GetScore());
            }
            else
            {
                view.ShowLosePanel2(LevelConfig.instance.levels[LevelConfig.instance.currentlevel].levelType.minScore);
            }
            UpdateLevelValues(LevelConfig.instance.model);
            StartCloudUpdatePlayerStats();
        }
    }

    private void UpdateLevelValues(LevelSelectionModel model)
    {
        var allLevels = model.allLevels;

        if (LevelConfig.instance.levels.Count > 0)
        {
            for (int i = 0; i < allLevels.Count; i++)
            {
                for (int j = 0; j < LevelConfig.instance.levels.Count; j++)
                {
                    var levels = LevelConfig.instance.levels;

                    if (allLevels[i].levelType.levelNumber == levels[j].levelType.levelNumber && allLevels[i].levelType.id == levels[j].levelType.id && levels[j].completed == true)
                    {
                        var index = i;
                        allLevels[index].completed = true;
                    }
                }
            }
        }
        string jsonString = JsonUtility.ToJson(model);
        PlayerPrefs.SetString("Levels", jsonString);
    }

    public void StartCloudUpdatePlayerStats()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "UpdatePlayerIq",
            FunctionParameter = new
            {
                high = PlayerPrefs.GetInt("IqPoints")
            },
            GeneratePlayStreamEvent = true,
        }, OnCloudUpdateStats, OnErrorShared);
    }

    private void OnCloudUpdateStats(ExecuteCloudScriptResult result)
    {
        JsonObject jsonResult = (JsonObject)result.FunctionResult;
        object messageValue;
        jsonResult.TryGetValue("messageValue", out messageValue);
    }

    private void OnErrorShared(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
        Debug.Log("COULDNT UPDATE CLOUD");
    }
}
