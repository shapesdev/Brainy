using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionView : MonoBehaviour, ILevelSelectionView {

    public Button[] levelButtons;
    public Image[] levelBorders;
    public Dropdown dropdown;

    public GameObject levelInfo;
    public Text levelText;
    public Text scoreText;
    public Text iqPointsText;

    bool activateEvent = false;

    public event EventHandler<OnValueUpdateEventArgs> OnValueUpdate;

    public void TurnOnPanel()
    {
        if(this.gameObject.activeInHierarchy == false)
        {
            this.gameObject.SetActive(true);
        }
    }

    public void TurnOffPanel()
    {
        if(this.gameObject.activeInHierarchy == true)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void PopulateDropdown(Dictionary<int, Type> categories)
    {
        List<string> options = new List<string>();

        foreach(var option in categories)
        {
            var name = option.Value.Name.Substring(0, option.Value.Name.Length - 5);
            options.Add(name);
        }
        dropdown.ClearOptions();
        dropdown.AddOptions(options);

        dropdown.value = PlayerPrefs.GetInt("CurrentSubject");
        activateEvent = true;
    }

    public void DropdownValueChanged(Dropdown change)
    {
        if(activateEvent == true)
        {
            var eventArgs = new OnValueUpdateEventArgs(change.value);
            OnValueUpdate(this, eventArgs);
        }
    }

    public void DisplayLevelSelections(List<LevelModel> levels)
    {
        this.gameObject.SetActive(true);

        iqPointsText.text = PlayerPrefs.GetInt("IqPoints").ToString();

        for(int i = 0; i < levels.Count; i++)
        {
            levelButtons[i].gameObject.SetActive(true);
            levelBorders[i].gameObject.SetActive(true);

            if (levels[i].unlocked == true)
            {
                int index = i;
                levelButtons[index].interactable = true;
                levelButtons[index].onClick.AddListener(delegate { ShowLevel(levels, index, levelButtons[index].transform); });
            }
            else
            {
                levelButtons[i].interactable = false;
            }
            levelBorders[i].fillAmount = levels[i].currentScore / 100f;
            levelButtons[i].gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = levels[i].levelType.levelName;
            levelButtons[i].gameObject.GetComponent<Image>().sprite = levels[i].levelType.levelImage;
        }

        if(levels.Count != levelButtons.Length)
        {
            for(int i = levels.Count; i < levelButtons.Length; i++)
            {
                levelButtons[i].gameObject.SetActive(false);
                levelBorders[i].gameObject.SetActive(false);
            }
        }
    }

    public void ShowLevel(List<LevelModel> levels, int current, Transform levelPosition)
    {
        LevelConfig.instance.levels = levels;
        LevelConfig.instance.currentlevel = current;

        levelInfo.transform.parent.gameObject.SetActive(true);
        levelInfo.transform.localPosition = new Vector3(0, 180, 0);
        levelText.text = "Level " + levels[current].levelType.levelNumber + ": " + levels[current].levelType.levelName;
        scoreText.text = "Score: " + levels[current].currentScore + " of " + levels[current].levelType.maxScore;
    }

    public void StartLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void CloseLevelInfo()
    {
        levelInfo.transform.parent.gameObject.SetActive(false);
    }

    private void RemoveListeners()
    {
        foreach (var btn in levelButtons)
        {
            btn.onClick.RemoveAllListeners();
        }
    }
}
