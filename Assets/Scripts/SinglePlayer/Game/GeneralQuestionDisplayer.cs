using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralQuestionDisplayer : QuestionDisplayer {

    List<string> shuffledList = new List<string>();

    void OnEnable()
    {
        gamePanel.SetActive(true);
    }

    void OnDisable()
    {
        gamePanel.SetActive(false);
    }

    public void DisplayQuestion(string question, List<string> listas, string answer, GameController game)
    {
        shuffledList = ShuffleOptions(listas);
        gameController = game;

        for (int i = 0; i < shuffledList.Count; i++)
        {
            optionsText[i].text = shuffledList[i];
        }
        this.question.text = question;
        correctAnswer = answer;

        TurnOnButtonInteract();
    }

    public void RemoveVertical()
    {
        int amount = 0;

        foreach (var btn in optionButtons)
        {
            if (btn.interactable == false)
            {
                amount++;
            }
        }

        if(amount < 1)
        {
            if (optionsText[0].text == correctAnswer || optionsText[3].text == correctAnswer)
            {
                optionButtons[1].interactable = false;
                optionButtons[2].interactable = false;
            }
            else
            {
                optionButtons[0].interactable = false;
                optionButtons[3].interactable = false;
            }
            PlayerPrefs.SetInt("HP", PlayerPrefs.GetInt("HP") - 15);
        }
        else if(amount == 1)
        {
            for(int i = 0; i < optionsText.Length; i++)
            {
                if(optionsText[i].text != correctAnswer)
                {
                    optionButtons[i].interactable = false;
                }
            }
            PlayerPrefs.SetInt("HP", PlayerPrefs.GetInt("HP") - 15);
        }
    }

    public void RemoveHorizontal()
    {
        int amount = 0;

        foreach (var btn in optionButtons)
        {
            if (btn.interactable == false)
            {
                amount++;
            }
        }

        if(amount < 1)
        {
            if (optionsText[0].text == correctAnswer || optionsText[1].text == correctAnswer)
            {
                optionButtons[2].interactable = false;
                optionButtons[3].interactable = false;
            }
            else
            {
                optionButtons[0].interactable = false;
                optionButtons[1].interactable = false;
            }
            PlayerPrefs.SetInt("HP", PlayerPrefs.GetInt("HP") - 15);
        }
        else if (amount == 1)
        {
            for (int i = 0; i < optionsText.Length; i++)
            {
                if (optionsText[i].text != correctAnswer)
                {
                    optionButtons[i].interactable = false;
                }
            }
            PlayerPrefs.SetInt("HP", PlayerPrefs.GetInt("HP") - 15);
        }
    }
}
