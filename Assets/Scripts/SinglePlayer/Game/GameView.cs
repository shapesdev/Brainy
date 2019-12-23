using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameView : MonoBehaviour {

    public GameObject gamePanel;
    public GameObject winPanel;
    public GameObject losePanel;

    public Text winText;
    public Text loseText;

    public SoundEffect[] soundEffects;
    public AudioSource audioSource;

    public Image levelImage;
    public Text levelName;
    public Text winIqGained;
    public Text questionNr;

    public GameObject pictureObject;
    public GameObject generalObject;
    public GameObject pictureAnswersObject;

    private GameController gameController;

    int nr = 1;

    public void DisplayLevel(GameController gameController)
    {
        this.gameController = gameController;

        gamePanel.SetActive(true);
        levelImage.sprite = LevelConfig.instance.levels[LevelConfig.instance.currentlevel].levelType.levelImage;
        levelName.text = "Level " + LevelConfig.instance.levels[LevelConfig.instance.currentlevel].levelType.levelNumber + "\n"
            + LevelConfig.instance.levels[LevelConfig.instance.currentlevel].levelType.levelName;
    }

    public void ShowQuestion(Question question)
    {
        questionNr.text = "" + nr + "/" + LevelConfig.instance.levels[LevelConfig.instance.currentlevel].levelType.questions.Length;
        nr++;
        if (question.GetType().IsAssignableFrom(typeof(PictureQuestion)))
        {
            if(generalObject.activeInHierarchy)
            {
                generalObject.SetActive(false);
            }
            if(pictureAnswersObject.activeInHierarchy)
            {
                pictureAnswersObject.SetActive(false);
            }
            pictureObject.SetActive(true);
            pictureObject.GetComponent<PictureQuestionDisplayer>().DisplayQuestion(question.question, question.options, question.picture, question.correctAnswer, gameController);
        }
        else if(question.GetType().IsAssignableFrom(typeof(GeneralQuestion)))
        {
            if(pictureObject.activeInHierarchy)
            {
                pictureObject.SetActive(false);
            }
            if (pictureAnswersObject.activeInHierarchy)
            {
                pictureAnswersObject.SetActive(false);
            }
            generalObject.SetActive(true);
            generalObject.GetComponent<GeneralQuestionDisplayer>().DisplayQuestion(question.question, question.options, question.correctAnswer, gameController);
        }
        else if(question.GetType().IsAssignableFrom(typeof(PictureAnswersQuestion)))
        {
            if (pictureObject.activeInHierarchy)
            {
                pictureObject.SetActive(false);
            }
            if (generalObject.activeInHierarchy)
            {
                generalObject.SetActive(false);
            }
            pictureAnswersObject.SetActive(true);
            pictureAnswersObject.GetComponent<PictureAnswersDisplayer>().DisplayQuestion(question.question + question.correctAnswer,  question.imageOptions, question.picture, gameController);
        }
    }

    public void Win()
    {
        audioSource.clip = soundEffects[0].clip;
        audioSource.volume = soundEffects[0].volume;
        audioSource.Play();
        if (pictureObject.activeInHierarchy)
        {
            pictureObject.GetComponent<PictureQuestionDisplayer>().Win();
        }
        else if(generalObject.activeInHierarchy)
        {
            generalObject.GetComponent<GeneralQuestionDisplayer>().Win();
        }
        else if(pictureAnswersObject.activeInHierarchy)
        {
            pictureAnswersObject.GetComponent<PictureAnswersDisplayer>().Win();
        }
    }

    public void Lose()
    {
        audioSource.clip = soundEffects[1].clip;
        audioSource.volume = soundEffects[1].volume;
        audioSource.Play();
        if (pictureObject.activeInHierarchy)
        {
            pictureObject.GetComponent<PictureQuestionDisplayer>().Lose();
        }
        else if (generalObject.activeInHierarchy)
        {
            generalObject.GetComponent<GeneralQuestionDisplayer>().Lose();
        }
        else if (pictureAnswersObject.activeInHierarchy)
        {
            pictureAnswersObject.GetComponent<PictureAnswersDisplayer>().Lose();
        }
    }

    public void ShowLosePanel()
    {
        losePanel.SetActive(true);
        loseText.text = LevelConfig.instance.levels[LevelConfig.instance.currentlevel].levelType.loseMotivation;
    }

    public void ShowLosePanel2(int score)
    {
        losePanel.SetActive(true);
        loseText.text = "Get a minimum of: " + score + " points to unlock next level!";
    }

    public void ShowWinPanel(int score)
    {
        audioSource.clip = soundEffects[2].clip;
        audioSource.volume = soundEffects[2].volume;
        audioSource.Play();
        winPanel.SetActive(true);
        winIqGained.text = "IQ Points Gained: " + score;
        winText.text = LevelConfig.instance.levels[LevelConfig.instance.currentlevel].levelType.winMotivation;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlayNextLevel()
    {
        if(LevelConfig.instance.currentlevel + 1 < LevelConfig.instance.levels.Count && LevelConfig.instance.levels[LevelConfig.instance.currentlevel].completed == true)
        {
            LevelConfig.instance.currentlevel += 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            //Debug.Log("All levels in category finished!");
            GoToMainMenu();
        }
    }
}
