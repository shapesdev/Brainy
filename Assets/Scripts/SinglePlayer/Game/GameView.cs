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
    public Text hintAmount;

    public SoundEffect[] soundEffects;
    public AudioSource audioSource;

    public Image levelImage;
    public Text levelName;
    public Text winIqGained;
    public Text questionNr;
    public Image soundImage;

    public GameObject pictureObject;
    public GameObject generalObject;
    public GameObject pictureAnswersObject;
    public GameObject hintPanel;

    public Sprite SoundOff;
    public Sprite SoundOn;

    private GameController gameController;

    int nr = 1;

    public void DisplayLevel(GameController gameController)
    {
        this.gameController = gameController;

        gamePanel.SetActive(true);
        levelImage.sprite = LevelConfig.instance.levels[LevelConfig.instance.currentlevel].levelType.levelImage;
        levelName.text = "Level " + LevelConfig.instance.levels[LevelConfig.instance.currentlevel].levelType.levelNumber + "\n"
            + LevelConfig.instance.levels[LevelConfig.instance.currentlevel].levelType.levelName;

        Debug.Log(PlayerPrefs.GetInt("NoSounds"));
        if(PlayerPrefs.GetInt("NoSounds") == 0)
        {
            soundImage.sprite = SoundOn;
            AudioListener.volume = 1;
        }
        else
        {
            soundImage.sprite = SoundOff;
            AudioListener.volume = 0;
        }
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

    public void TurnSound(Image img)
    {
        if(PlayerPrefs.GetInt("NoSounds") == 0)
        {
            img.sprite = SoundOff;
            AudioListener.volume = 0;
            PlayerPrefs.SetInt("NoSounds", 1);
        }
        else
        {
            img.sprite = SoundOn;
            AudioListener.volume = 1;
            PlayerPrefs.SetInt("NoSounds", 0);
        }
    }

    public void ShowLosePanel()
    {
        audioSource.clip = soundEffects[2].clip;
        audioSource.volume = soundEffects[2].volume;
        audioSource.Play();
        losePanel.SetActive(true);
        loseText.text = "Already done? We will be waiting for you!";
    }

    public void ShowLosePanel2(int score)
    {
        audioSource.clip = soundEffects[2].clip;
        audioSource.volume = soundEffects[2].volume;
        audioSource.Play();
        losePanel.SetActive(true);
        loseText.text = "You just gonna take this massive L? Answer " + score / 10 + " questions to unlock next level!";
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

    public void OpenHintPanel()
    {
        hintAmount.text = "Hint Points: " + PlayerPrefs.GetInt("HP").ToString();
        hintPanel.SetActive(true);
    }

    public void CloseHintPanel()
    {
        hintPanel.SetActive(false);
    }

    public void RemoveOneWrongOption()
    {
        if (pictureObject.activeInHierarchy)
        {
            pictureObject.GetComponent<PictureQuestionDisplayer>().RemoveOneWrong();
        }
        else if (generalObject.activeInHierarchy)
        {
            generalObject.GetComponent<GeneralQuestionDisplayer>().RemoveOneWrong();
        }
        else if (pictureAnswersObject.activeInHierarchy)
        {
            pictureAnswersObject.GetComponent<PictureAnswersDisplayer>().RemoveOneWrong();
        }
        hintPanel.SetActive(false);
    }

    public void RemoveVerticalOptions()
    {
        if (pictureObject.activeInHierarchy)
        {
            pictureObject.GetComponent<PictureQuestionDisplayer>().RemoveVertical();
        }
        else if (generalObject.activeInHierarchy)
        {
            generalObject.GetComponent<GeneralQuestionDisplayer>().RemoveVertical();
        }
        else if (pictureAnswersObject.activeInHierarchy)
        {
            pictureAnswersObject.GetComponent<PictureAnswersDisplayer>().RemoveVertical();
        }
        hintPanel.SetActive(false);
    }

    public void RemoveHorizontalOptions()
    {
        if (pictureObject.activeInHierarchy)
        {
            pictureObject.GetComponent<PictureQuestionDisplayer>().RemoveHorizontal();
        }
        else if (generalObject.activeInHierarchy)
        {
            generalObject.GetComponent<GeneralQuestionDisplayer>().RemoveHorizontal();
        }
        else if (pictureAnswersObject.activeInHierarchy)
        {
            pictureAnswersObject.GetComponent<PictureAnswersDisplayer>().RemoveHorizontal();
        }
        hintPanel.SetActive(false);
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
            GoToMainMenu();
        }
    }
}
