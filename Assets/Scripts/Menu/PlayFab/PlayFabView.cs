using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabView : MonoBehaviour, IPlayFabView {

    public event EventHandler<OnInternetEventArgs> OnInternet = (sender, e) => { };
    float timer = 1.0f;
    float waitTime = 0f;
    bool connected = false;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > waitTime && connected == false)
        {
            if(Application.internetReachability != NetworkReachability.NotReachable)
            {
                connected = true;
                var eventArgs = new OnInternetEventArgs();
                OnInternet(this, eventArgs);
            }
            timer = 0f;
        }
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            connected = false;
            //Debug.Log("NO INTERNET");
        }
    }

    public void PrintText(string str)
    {
       // Debug.Log(str);
    }
}
