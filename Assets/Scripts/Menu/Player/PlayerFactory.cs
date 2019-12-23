using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFactory {

    public PlayerController controller { get; private set; }
    public PlayerModel model { get; private set; }
    public PlayerView view { get; private set; }

    public void Load(PlayerView view)
    {
        this.model = new PlayerModel();
        this.view = view;
        this.controller = new PlayerController(model, view);
    }
}
