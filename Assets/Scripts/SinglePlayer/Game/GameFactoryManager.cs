using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameFactoryManager: MonoBehaviour {

    GameFactory gameFactory;

    private void OnEnable()
    {       
        InitializeGame();
    }

    private void OnDisable()
    {
        foreach (GameObject o in FindObjectsOfType<GameObject>())
        {
            Destroy(o);
        }
    }

    private void InitializeGame()
    {
        gameFactory = new GameFactory();
        gameFactory.Load();
    }
}

