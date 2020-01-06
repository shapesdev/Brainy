using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FactoryManager : MonoBehaviour {

    public ShopView shopView;
    public PlayFabView playFabView;
    public PlayerView playerView;
    public LevelSelectionView levelSelectionView;
    public LeaderboardView leaderboardView;

    public SkinItem skinsArray;
    public Achievement[] achievementsArray;
    public ShopItem[] shopItemsArray;

    PlayFabController playFabController;

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        string deviceId = SystemInfo.deviceUniqueIdentifier;

        InitializePlayFab(deviceId);
        playFabController.OnLoggedIn += HandleOnLoggedIn;

        LoadSkins();
        InitializeLevelSelection();
    }

    private void OnDisable()
    {
        foreach (GameObject o in FindObjectsOfType<GameObject>())
        {
            Destroy(o);
        }
    }

    private void InitializeLevelSelection()
    {
        PlayerPrefs.SetInt("Rate", PlayerPrefs.GetInt("Rate") + 1);
        LevelSelectionFactory levelFactory = new LevelSelectionFactory();
        levelFactory.Load(levelSelectionView, achievementsArray);
    }

    private void InitializePlayFab(string id)
    {
       
        PlayFabFactory playFabFactory = new PlayFabFactory();
        playFabFactory.Load(playFabView, id);
        playFabController = playFabFactory.controller;
    }

    private void HandleOnLoggedIn(object sender, LoggedInEventArgs e)
    {
        InitializePlayer();
        InitializeShop();
        InitializeLeaderboard();
        Debug.Log("GAME LOADED");
    }

    private void InitializeLeaderboard()
    {
        LeaderboardFactory leaderboardFactory = new LeaderboardFactory();
        leaderboardFactory.Load(leaderboardView);
    }

    private void InitializePlayer()
    {
        PlayerFactory playerFactory = new PlayerFactory();
        playerFactory.Load(playerView, levelSelectionView);
    }

    private void InitializeShop()
    {
        List<ShopItem> items = new List<ShopItem>();
        foreach(var item in shopItemsArray)
        {
            items.Add(item);
        }
        ShopFactory shopFactory = new ShopFactory();
        shopFactory.Load(shopView, items);
    }

    private void LoadSkins()
    {
        Dictionary<int, Sprite> skins = new Dictionary<int, Sprite>();
        int count = 0;

        foreach (var obj in skinsArray.skins)
        {
            skins.Add(count, obj);
            count += 1;
        }
        PlayerConfig.instance.skins = skins;
    }
}
