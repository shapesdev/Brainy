using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;

public class LeaderboardModel {

    List<List<string>> playerList = new List<List<string>>();
    List<string> myRank = new List<string>();

    public List<string> GetMyRankList()
    {
        return myRank;
    }

    public List<List<string>> GetPlayerList()
    {
        return playerList;
    }

    public void SetPlayerList(PlayerLeaderboardEntry player)
    {
         playerList.Add(new List<string> { "#" + (player.Position + 1).ToString(), player.DisplayName, player.StatValue.ToString() });
    }

    public void SetMyRank(PlayerLeaderboardEntry player)
    {
         myRank.Add("#" + (player.Position + 1).ToString());
         myRank.Add(player.DisplayName);
         myRank.Add(player.StatValue.ToString());
    }
}
