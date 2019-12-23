using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour, IPlayerView {

    public Image avatar;
    public Text iqText;
    public Text rankText;
    public Text topicText;
    public Text levelsCompleted;
    public InputField usernameText;

    public GameObject nameChanged;
    public GameObject nameTaken;

    public event EventHandler<ChangeNameEventArgs> OnChangeName;

    public void DisplayStats()
    {
        iqText.text = "IQ Points: " + PlayerPrefs.GetInt("IqPoints");
        rankText.text = "Rank: " + PlayerConfig.instance.GetRank();
        topicText.text = "Favorite Subject: " + PlayerConfig.instance.favSubject;
        levelsCompleted.text = "Levels Completed: " + PlayerConfig.instance.levelsCompleted + " of " + PlayerConfig.instance.totalLevels;
        usernameText.text = PlayerConfig.instance.playerName;
        avatar.sprite = PlayerConfig.instance.skins[PlayerConfig.instance.currentSkin];
        usernameText.onEndEdit.AddListener(delegate { ValueChangeCheck(); });
        this.gameObject.SetActive(true);
    }

    public void StopDisplayStats()
    {
        this.gameObject.SetActive(false);
    }

    private void ValueChangeCheck()
    {
        var eventArgs = new ChangeNameEventArgs(usernameText.text);
        OnChangeName(this, eventArgs);
    }

    public void NameTaken()
    {
        usernameText.text = PlayerConfig.instance.playerName;
        nameTaken.SetActive(true);
        Invoke("CloseGameobject", 1.5f);
    }

    public void NameChanged()
    {
        usernameText.text = PlayerConfig.instance.playerName;
        nameChanged.SetActive(true);
        Invoke("CloseGameobject", 1.5f);
    }

    private void CloseGameobject()
    {
        if(nameTaken.activeInHierarchy == true)
        {
            nameTaken.SetActive(false);
        }
        if(nameChanged.activeInHierarchy == true)
        {
            nameChanged.SetActive(false);
        }
    }
}
