using System;
using System.Collections;
using System.Collections.Generic;

public class PlayFabModel {

    string myId;
    string mobileId;

    public void SetMobileId(string id)
    {
        mobileId = id;
    }

    public string GetMobileId()
    {
        return mobileId;
    }

    public void SetPlayFabId(string id)
    {
        myId = id;
        PlayerConfig.instance.playerId = id;
    }

    public string GetPlayFabId()
    {
        return myId;
    }
}
