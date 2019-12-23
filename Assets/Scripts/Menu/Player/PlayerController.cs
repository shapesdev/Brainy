using System;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using UnityEngine;

public class PlayerController {

    public PlayerModel model;
    public PlayerView view;
    private LevelSelectionView levelView;
    RandomNameGenerator nameGenerator;

    public PlayerController(PlayerModel model, PlayerView view, LevelSelectionView levelView)
    {
        this.model = model;
        this.view = view;
        this.levelView = levelView;

        CheckIfUserNameExists();
        GetPlayerVC();
        view.OnChangeName += View_OnChangeName;
        MenuUIManager.allActions["Profile"] += HandleOnProfile;
    }

    private void View_OnChangeName(object sender, ChangeNameEventArgs e)
    {
        if(e.name != "" && e.name.Length > 2)
        {
            ChangeName(e.name);
        }
        else
        {
            view.NameTaken();
        }
    }

    private void HandleOnProfile(bool obj)
    {
        if (obj == true)
        {
            view.DisplayStats();
        }
        else
        {
            view.StopDisplayStats();
        }
    }

    private void CheckIfUserNameExists()
    {
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest { ProfileConstraints = new PlayerProfileViewConstraints { ShowDisplayName = true } },
        result =>
        {
            if (string.IsNullOrEmpty(result.PlayerProfile.DisplayName))
            {
                GiveName();
            }
            else
            {
                GetPlayerProfile(result.PlayerProfile.PlayerId);
                GetStats();
            }
        },
        error => { Debug.LogError(error.GenerateErrorReport()); });
    }

    private void GetPlayerProfile(string playFabId)
    {
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
        {
            PlayFabId = playFabId,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true
            }

        }, OnGetPlayerProfileSuccess, OnGetPlayerProfileFailure);
    }

    private void OnGetPlayerProfileFailure(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
        Debug.Log("COULD'nT GET PLAYER PROFILE");
    }

    void OnGetPlayerProfileSuccess(GetPlayerProfileResult result)
    {
        model.SetPlayerName(result.PlayerProfile.DisplayName);
    }

    private void GiveName()
    {
        nameGenerator = new RandomNameGenerator();
        string name = nameGenerator.GetRandomName();

        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = name }, OnGiveSucess, OnGiveFiilure);
    }

    private void OnGiveFiilure(PlayFabError error)
    {
        GiveName();
    }

    private void OnGiveSucess(UpdateUserTitleDisplayNameResult result)
    {
        model.SetPlayerName(result.DisplayName);
        StartCloudUpdatePlayerStats();
    }

    public void ChangeName(string name)
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = name }, OnChangeSucess, OnChangeFailure);
    }

    private void OnChangeFailure(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
        Debug.Log("COULD'NT UPDATE NAME");
        view.NameTaken();
    }

    private void OnChangeSucess(UpdateUserTitleDisplayNameResult result)
    {
        model.SetPlayerName(result.DisplayName);
        view.NameChanged();
    }

    public void GetStats()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStats,
            error => Debug.LogError(error.GenerateErrorReport())
            );
    }

    void OnGetStats(GetPlayerStatisticsResult result)
    {
        foreach (var eachStat in result.Statistics)
        {
            switch (eachStat.StatisticName)
            {
                case "HighScore":
                    if(eachStat.Value > PlayerPrefs.GetInt("IqPoints"))
                    {
                        PlayerPrefs.SetInt("IqPoints", eachStat.Value);
                    }
                    break;
                case "TotalGames":
                    model.SetTotalGames(eachStat.Value);
                    break;
                case "RoundsPlayed":
                    model.SetRoundsPlayed(eachStat.Value);
                    break;
                case "GamesWon":
                    model.SetGamesWon(eachStat.Value);
                    break;
            }
        }
        levelView.UpdateIqPoints();
    }

    public void StartCloudUpdatePlayerStats()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "UpdatePlayerStats",
            FunctionParameter = new
            {
                 high = PlayerPrefs.GetInt("IqPoints"),
                 won = model.GetGamesWon(),
                 total = model.GetTotalGames(),
                 rounds = model.GetRoundsPlayed(),
            },
            GeneratePlayStreamEvent = true,
        }, OnCloudUpdateStats, OnErrorShared);
    }

    private void OnCloudUpdateStats(ExecuteCloudScriptResult result)
    {
        JsonObject jsonResult = (JsonObject)result.FunctionResult;
        object messageValue;
        jsonResult.TryGetValue("messageValue", out messageValue);
        GetStats();
    }

    private void OnErrorShared(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
        Debug.Log("COULDNT UPDATE CLOUD");
    }

    private void GetPlayerVC()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), (GetUserInventoryResult result) =>
        {
            OnGetCurrency(result);
        }, OnGetCurrencyError);
    }

    private void OnGetCurrency(GetUserInventoryResult result)
    {
        var dmd = int.Parse(result.VirtualCurrency["DM"].ToString());
        PlayerPrefs.SetInt("Dmd", dmd);
    }

    private void OnGetCurrencyError(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
        Debug.Log("COULDNT GET VC");
    }
}
