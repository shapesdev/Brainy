using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureQuestionDisplayer : QuestionDisplayer {

    List<string> shuffledList = new List<string>();
    public Image picture;
    public Image biggerPicture;
    public GameObject boostError;

    void OnEnable()
    {
        gamePanel.SetActive(true);
    }

    void OnDisable()
    {
        gamePanel.SetActive(false);
    }

    public void MaximizePicture()
    {
        biggerPicture.transform.parent.gameObject.SetActive(true);
    }

    public void MinimizePicture()
    {
        biggerPicture.transform.parent.gameObject.SetActive(false);
    }

    public void DisplayQuestion(string question, List<string> listas, Sprite sprite, string answer, GameController game)
    {
        shuffledList = ShuffleOptions(listas);
        gameController = game;

        for (int i = 0; i < shuffledList.Count; i++)
        {
            optionsText[i].text = shuffledList[i];
        }
        this.question.text = question;
        picture.sprite = sprite;
        biggerPicture.sprite = sprite;
        correctAnswer = answer;

        TurnOnButtonInteract();
    }

    public void RemoveVertical()
    {
        int amount = 0;

        foreach(var btn in optionButtons)
        {
            if(btn.interactable == false)
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
            PlayerPrefs.GetInt("HP", PlayerPrefs.GetInt("HP") - 15);
        }
        else
        {
            boostError.SetActive(true);
            Invoke("TurnOffError", 2f);
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
            PlayerPrefs.GetInt("HP", PlayerPrefs.GetInt("HP") - 15);
        }
        else
        {
            boostError.SetActive(true);
            Invoke("TurnOffError", 2f);
        }
    }

    public void TurnOffError()
    {
        boostError.SetActive(false);
    }
}
