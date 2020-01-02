using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureQuestionDisplayer : QuestionDisplayer {

    List<string> shuffledList = new List<string>();
    public Image picture;
    public Image biggerPicture;

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
}
