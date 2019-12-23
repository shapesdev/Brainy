using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerConfig : MonoBehaviour {

    public static PlayerConfig instance;

    public string playerId;
    public string playerName;
    public Image playerAvatar;
    public int levelsCompleted;
    public int totalLevels;
    public string favSubject;
    public int gamesWon;
    public int totalGames;
    public int roundsPlayed;
    public int currentSkin;

    public Dictionary<int, Sprite> skins = new Dictionary<int, Sprite>();
    public Dictionary<Achievement, int> achievements = new Dictionary<Achievement, int>();


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetLevelsCompleted(int a)
    {
        if(a > 0 && PlayerPrefs.GetInt("ShowInfo") == 0)
        {
            MenuUIManager.menuUI.ShowInfoAboutAds();
            PlayerPrefs.SetInt("ShowInfo", 1);
        }
        levelsCompleted = a;
    }

    public void ShowAchievement(Dictionary<Achievement, int> achievements)
    {
        this.achievements = achievements;

        foreach(var item in this.achievements)
        {
            if(item.Key.goal == item.Value)
            {
                if(PlayerPrefs.GetInt(item.Key.achievementName) == 0)
                {
                    MenuUIManager.menuUI.OpenAchievement1(item.Key);
                    PlayerPrefs.SetInt(item.Key.achievementName, 1);
                }
            }
        }
    }

    public string GetRank()
    {
        string rank = "";

        if (levelsCompleted == 0 || ((levelsCompleted * 100) / totalLevels) <= 15)
        {
            rank = "Homo habilis";
        }
        else if (((levelsCompleted * 100) / totalLevels) <= 30)
        {
            rank = "Upright Man";
        }
        else if (((levelsCompleted * 100) / totalLevels) <= 45)
        {
            rank = "The Neanderthal";
        }
        else if (((levelsCompleted * 100) / totalLevels) <= 60)
        {
            rank = "Flores Man";
        }
        else if (((levelsCompleted * 100) / totalLevels) <= 75)
        {
            rank = "Homo naledi";
        }
        else if (((levelsCompleted * 100) / totalLevels) <= 90)
        {
            rank = "Homo sapiens";
        }
        else
        {
            rank = "Genius";
        }
        return rank;
    }
}
