using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionDisplayer : MonoBehaviour {

    public string correctAnswer;

    public GameObject gamePanel;
    public Text[] optionsText;
    public Text question;
    private Text pickedOptionText;
    public Button[] optionButtons;
    private Button pickedButton;

    public GameController gameController;

    public List<string> ShuffleOptions(List<string> listas)
    {
        Shuffler shuffler = new Shuffler();
        var list = shuffler.Shuffle(listas);
        return list;
    }

    public void TurnOffButtonInteract()
    {
        foreach (Button bt in optionButtons)
        { 
            bt.interactable = false;
            if(bt == pickedButton)
            {
                var buttonColor = bt.colors;
                buttonColor.disabledColor = new Color32(255, 255, 255, 255);
                bt.colors = buttonColor;
            }
            else
            {
                var buttonColor = bt.colors;
                buttonColor.disabledColor = new Color32(200, 200, 200, 200);
                bt.colors = buttonColor;
            }
        }
    }

    public void TurnOnButtonInteract()
    {
        foreach (Text txt in optionsText) { txt.color = new Color32(50, 50, 50, 255); }
        foreach (Button bt in optionButtons) { bt.interactable = true; }
    }

    public void Win()
    { 
        pickedOptionText.color = new Color32(32, 176, 77, 255);

        Invoke("NextRound", 1.5f);
    }

    public void Lose()
    {
        if (pickedOptionText != null)
        {
            pickedOptionText.color = new Color32(255, 0, 0, 255);
            for(int i = 0; i < optionsText.Length; i++)
            {
                if(optionsText[i].text == correctAnswer)
                {
                    optionsText[i].color = new Color32(32, 176, 77, 255);

                    var buttonColor = optionButtons[i].colors;
                    buttonColor.disabledColor = new Color32(255, 255, 255, 230);
                    optionButtons[i].colors = buttonColor;
                    break;
                }
            }
        }
        Invoke("NextRound", 1.5f);
    }

    public void ButtonClick(Text text)
    {
        pickedOptionText = text;
        pickedButton = text.transform.parent.gameObject.GetComponent<Button>();
        TurnOffButtonInteract();

        gameController.CheckAnswer(text.text);
    }

    private void NextRound()
    {
        pickedOptionText = null;
        pickedButton = null;

        gameController.RoundFinish();
    }
}
