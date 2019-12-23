using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFactory {

    public GameController controller { get; private set; }
    public GameModel model { get; private set; }
    public GameView view { get; private set; }

    public void Load()
    {
        var questions = LevelConfig.instance.levels[LevelConfig.instance.currentlevel].levelType.questions;

        GameObject prefab = Resources.Load<GameObject>("GameManager");
        GameObject instance = Object.Instantiate(prefab);
        this.model = new GameModel(questions);
        this.view = instance.GetComponent<GameView>();
        this.controller = new GameController(model, view);
    }
}
