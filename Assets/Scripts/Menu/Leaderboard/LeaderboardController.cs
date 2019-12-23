using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;

public class LeaderboardController {

    public LeaderboardModel model;
    public LeaderboardView view;

    public LeaderboardController(LeaderboardModel model, LeaderboardView view)
    {
        this.model = model;
        this.view = view;

        GetLeaderboard();
        MenuUIManager.allActions["Leaderboard"] += HandleOnLeaderboard;
    }

    private void HandleOnLeaderboard(bool obj)
    {
        if (obj == true)
        {
            view.DisplayLeaderboard(model.GetPlayerList(), model.GetMyRankList());
        }
        else
        {
            view.StopDisplayLeaderboard();
        }
    }

    private void GetLeaderboard()
    {
        var requestLeaderboard = new GetLeaderboardRequest { StartPosition = 0, StatisticName = "HighScore", MaxResultsCount = 100 };
        PlayFabClientAPI.GetLeaderboard(requestLeaderboard, OnGetLeaderboard, OnErrorLeaderboard);
    }

    private void OnGetLeaderboard(GetLeaderboardResult result)
    {
        foreach (PlayerLeaderboardEntry player in result.Leaderboard)
        {
            if (player.PlayFabId == PlayerConfig.instance.playerId)
            {
                model.SetMyRank(player);
            }
            model.SetPlayerList(player);
        }
    }

    private void OnErrorLeaderboard(PlayFabError error)
    {

    }
}
