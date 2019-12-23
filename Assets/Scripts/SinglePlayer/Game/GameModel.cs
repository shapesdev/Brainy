using System.Collections;
using System.Collections.Generic;

public class GameModel {

    Question[] allQuestions;
    int questionNr;
    int score;

    public GameModel (Question[] questions)
    {
        allQuestions = questions;
        questionNr = 0;
        score = 0;
    }

    public void AddScore()
    {
        score += 10;
    }

    public int GetScore()
    {
        return score;
    }

    public void QuestionNrToZero()
    {
        questionNr = 0;
    }

    public Question[] GetlAllQuestions()
    {
        return allQuestions;
    }

    public void IncreaseQuestionNr()
    {
        questionNr++;
    }

    public int GetQuestionNr()
    {
        return questionNr;
    }
}
