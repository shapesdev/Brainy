using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardView : MonoBehaviour {

    public GameObject dataPrefab;
    public Transform dataContainer;
    public Text[] firstPlace;
    public Text[] secondPlace;
    public Text[] thirdPlace;
    public Text[] myRankText;

    public void DisplayLeaderboard(List<List<string>> playerList, List<string> myPlace)
    {
        if(dataContainer.childCount > 0)
        {
            for (int i = dataContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(dataContainer.GetChild(i).gameObject);
            }
        }

        if (playerList.Count == 0)
        {
            //Debug.Log("EMPTY LEADERBOARD!");
        }
        else
        {
            foreach (List<string> subList in playerList)
            {
                if (subList[0] == "#1")
                {
                    DisplayFirstPlace(subList);
                }
                else if (subList[0] == "#2")
                {
                    DisplaySecondPlace(subList);
                }
                else if (subList[0] == "#3")
                {
                    DisplayThirdPlace(subList);
                }
                else
                {
                    GameObject place = Instantiate(dataPrefab, dataContainer);
                    LeaderboardTemplate info = place.GetComponent<LeaderboardTemplate>();
                    info.number.text = subList[0];
                    info.userName.text = subList[1];
                    info.highscore.text = subList[2];
                }
            }
            DisplayMyPlace(myPlace);
            this.gameObject.SetActive(true);
        }
    }

    void DisplayFirstPlace(List<string> place)
    {
        firstPlace[0].text = place[1];
        firstPlace[1].text = place[2];
    }

    void DisplaySecondPlace(List<string> place)
    {
        secondPlace[0].text = place[1];
        secondPlace[1].text = place[2];
    }

    void DisplayThirdPlace(List<string> place)
    {
        thirdPlace[0].text = place[1];
        thirdPlace[1].text = place[2];
    }

    void DisplayMyPlace(List<string> myPlace)
    {
        myRankText[0].text = myPlace[0];
        myRankText[1].text = myPlace[1];
        myRankText[2].text = myPlace[2];
    }

    public void StopDisplayLeaderboard()
    {
        for (int i = dataContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(dataContainer.GetChild(i).gameObject);
        }
        this.gameObject.SetActive(false);
    }
}
