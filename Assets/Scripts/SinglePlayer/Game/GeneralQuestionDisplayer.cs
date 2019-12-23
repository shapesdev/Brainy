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
}
