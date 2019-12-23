using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardFactory {

    public LeaderboardController controller { get; private set; }
    public LeaderboardModel model { get; private set; }
    public LeaderboardView view { get; private set; }

    public void Load(LeaderboardView view)
    {
        this.model = new LeaderboardModel();
        this.view = view;
        this.controller = new LeaderboardController(model, view);
    }
}
