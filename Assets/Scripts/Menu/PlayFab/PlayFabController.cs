using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabController : IPlayFabController
{
    public PlayFabModel model;
    public PlayFabView view;
    public event EventHandler<LoggedInEventArgs> OnLoggedIn = (sender, e) => { };

    public PlayFabController(PlayFabModel model, PlayFabView view, string deviceId)
    {
        this.model = model;
        this.view = view;

        model.SetMobileId(deviceId);
        view.OnInternet += HandleOnNoInternet;
    }

    private void HandleOnNoInternet(object sender, OnInternetEventArgs e)
    {
        Login();
    }

    private void Login()
    {
        var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = model.GetMobileId() , CreateAccount = true };
        PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnLoginAndroidSuccess, OnLoginAndroidFailure);
    }

    private void OnLoginAndroidSuccess(LoginResult result)
    {
        if(result.PlayFabId == "3F4D54B9DB7930DC")
        {
            MenuUIManager.menuUI.TurnOnDomisDx();
        }
        model.SetPlayFabId(result.PlayFabId);

        var eventArgs = new LoggedInEventArgs();
        OnLoggedIn(this, eventArgs);
    }

    private void OnLoginAndroidFailure(PlayFabError error)
    {
        Debug.Log("FAILED TO LOGIN");
    }
}

