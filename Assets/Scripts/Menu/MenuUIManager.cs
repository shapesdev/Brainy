using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour {

    public static MenuUIManager menuUI;

    public Image[] navigationButtons;
    public Sprite[] pressedButtons;
    public Sprite[] notPressedButtons;
    public GameObject noInternet;
    public GameObject achievement;
    public Text achievementText;
    public GameObject info;

    event Action<bool> OnLeaderboard = delegate { };
    event Action<bool> OnProfile = delegate { };
    event Action<bool> OnShop = delegate { };
    event Action<bool> OnHome = delegate { };
    public static Dictionary<string, Action<bool>> allActions = new Dictionary<string, Action<bool>>();

    private void Awake()
    {
        menuUI = this;

        if(allActions.Count == 0)
        {
            allActions.Add("Home", OnHome);
            allActions.Add("Shop", OnShop);
            allActions.Add("Leaderboard", OnLeaderboard);
            allActions.Add("Profile", OnProfile);
        }
        navigationButtons[0].sprite = pressedButtons[0];
    }

    public void ShowInfoAboutAds()
    {
        info.SetActive(true);
    }

    public void CloseInfoAboutAds()
    {
        info.SetActive(false);
    }

    private void OnDisable()
    {
        allActions.Clear();
    }

    public void ChangeNavigationFocus(Image img)
    {
        for(int i = 0; i < navigationButtons.Length; i++)
        {
            if(navigationButtons[i].name != img.name)
            {
                navigationButtons[i].sprite = notPressedButtons[i];
            }
            else
            {
                navigationButtons[i].sprite = pressedButtons[i];
            }
        }
    }

    public void ButtonClick(Button btn)
    {
        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            foreach (KeyValuePair<string, Action<bool>> action in allActions)
            {
                if (btn.name != "HomeButton")
                {
                    OpenNoInternet();
                    action.Value.Invoke(false);
                }

                allActions["Home"].Invoke(true);
            }
        }
        else
        {
            foreach (KeyValuePair<string, Action<bool>> action in allActions)
            {
                if (!btn.name.Contains(action.Key))
                {
                    action.Value.Invoke(false);
                }
                else
                {
                    action.Value.Invoke(true);
                }
            }
        }
    }

    public void OpenAchievement1(Achievement ach)
    {
        achievementText.text = "New Achievement Unlocked - " + ach.achievementName;
        achievement.SetActive(true);
    }

    private void OpenNoInternet()
    {
        noInternet.SetActive(true);
        Invoke("CloseInternet", 1.5f);
    }

    private void CloseInternet()
    {
        noInternet.SetActive(false);
    }
}
