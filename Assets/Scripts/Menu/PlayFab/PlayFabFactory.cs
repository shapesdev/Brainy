using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabFactory {

    public PlayFabController controller { get; private set; }
    public PlayFabModel model { get; private set; }
    public PlayFabView view { get; private set; }

    public void Load(PlayFabView view, string deviceId)
    {
        this.model = new PlayFabModel();
        this.view = view;
        this.controller = new PlayFabController(model, view, deviceId);
    }
}
