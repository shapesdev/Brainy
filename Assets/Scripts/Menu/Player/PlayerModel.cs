using System.Collections;
using System.Collections.Generic;

public class PlayerModel {

    string playerName;
    int gamesWon;
    int totalGames;
    int roundsPlayed;

    public void SetPlayerName(string name)
    {
        playerName = name;
        PlayerConfig.instance.playerName = name;
    }

    public string GetPlayerName()
    {
        return playerName;
    }

    public void SetGamesWon(int won)
    {
        PlayerConfig.instance.gamesWon = won;
        gamesWon = won;
    }

    public int GetGamesWon()
    {
        return gamesWon;
    }

    public void SetTotalGames(int total)
    {
        PlayerConfig.instance.totalGames = total;
        totalGames = total;
    }

    public int GetTotalGames()
    {
        return totalGames;
    }

    public void SetRoundsPlayed(int rounds)
    {
        PlayerConfig.instance.roundsPlayed = rounds;
        roundsPlayed = rounds;
    }

    public int GetRoundsPlayed()
    {
        return roundsPlayed;
    }
}
